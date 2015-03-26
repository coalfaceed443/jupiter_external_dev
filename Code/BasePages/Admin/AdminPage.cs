using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Reflection;
using CRM.Code.Models;
using CRM.Code.Auth;
using CRM.Code.Managers;
using System.Linq.Dynamic;
using CRM.Code.Interfaces;
using CRM.Controls.Admin.SharedObjects.List;
using CRM.Code.Helpers;
using CRM.Code.Utils.List;
using System.IO;
using CRM.Admin;
using CRM.Controls.Forms;
namespace CRM.Code.BasePages.Admin
{
    public class AdminPage : BasePage
    {


        public MainDataContext DataContext
        {
            get
            {
                return db;
            }
        }

        public ICRMContext CRMContext { get; set; }

        public CRM.Code.Models.Admin AdminUser { get; set; }

        private static string GetExactPathName(string pathName)
        {
            if (!(File.Exists(pathName) || Directory.Exists(pathName)))
                return pathName;

            var di = new DirectoryInfo(pathName);

            if (di.Parent != null)
            {
                return Path.Combine(
                    GetExactPathName(di.Parent.FullName),
                    di.Parent.GetFileSystemInfos(di.Name)[0].Name);
            }
            else
            {
                return di.Name.ToUpper();
            }
        }

        public string GetPageTitle()
        {
            string inner = GetCurrentURL();
            inner = inner.Replace("\\", " - ").Replace("- admin - ", "").Replace(".aspx", "") + " | " + Constants.WebsiteName;

            return Split.SplitCamelCase(inner);

        }



        private PermissionManager _PermissionManager;
        /// <summary>
        /// This contains the security information for the admin system for the logged in user.
        /// Populated using the 'RunSecurity' method
        /// </summary>
        public PermissionManager PermissionManager { get { return _PermissionManager; } }

        public bool IsAuthorised = false;
        public string AjaxManager = "/admin/adminajaxhandler.ashx";

        public CRM_SystemAccessAdmin AdminPermission;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            AuthAdmin authAdmin = new AuthAdmin(db);

            AdminUser = authAdmin.Authorise();


            if (AdminUser == null)
            {
                Session.Remove("IsAuthorized");
                Response.Redirect("/admin/login.aspx?redirect=" + Request.RawUrl);
            }
            else
            {
                string currentURL = GetCurrentURL();

                if (currentURL.StartsWith("\\admin"))
                {
                    int adminLength = "\\admin".Length;
                    currentURL = currentURL.Substring(adminLength, currentURL.Length - adminLength);
                }
                AdminPermission = AdminUser.CRM_SystemAccessAdmins.SingleOrDefault(s => s.CRM_SystemAccess != null && s.CRM_SystemAccess.Path.ToLower() == currentURL.ToLower());

                bool CanView = true;

                if (!Request.RawUrl.StartsWith("/admin/default.aspx"))
                {

                    if (AdminPermission == null || !AdminPermission.IsRead)
                    {
                        CanView = false;

                        AdminPermission = AdminUser.CRM_SystemAccessAdmins.SingleOrDefault(s => s.CRM_SystemAccess == null && ((string)s.BespokeURL).ToLower() == Request.RawUrl);


                        if (AdminPermission == null || !AdminPermission.IsRead)
                            CanView = false;
                        else if (AdminPermission != null)
                            CanView = true;
                    }
                }

                if (!CanView)
                    NoticeManager.SetMessage("You do not have permission to view this page, please contact a Master Admin", "/admin");


                IsAuthorised = true;
                Session["IsAuthorized"] = true;
            }
        }

        protected string GetCurrentURL()
        {
            return GetExactPathName(Request.PhysicalPath).Replace(GetExactPathName(Request.PhysicalApplicationPath), "");
        }

        /// <summary>
        /// This checks the permissions for the currently logged in user and creates the PermissionManager
        /// </summary>
        /// <param name="section">The current section</param>
        protected void RunSecurity(Models.Admin.AllowedSections section)
        {

            _PermissionManager = new PermissionManager(section);
            /*
            if (!PermissionManager.CanView)
                Response.Redirect("/admin/nopermission.aspx?url=" + Request.RawUrl);*/
        }

        public void OverrideDataSource()
        {

        }
    }
    public class AdminPage<T> : AdminPage
    {

        public IEnumerable<T> BaseSet { get; set; }
        public UtilListView CheckQuery(UtilListView sender)
        {

            sender.ParsedViaQuery = true;
            sender.DataSet = BaseSet.Select(p => (object)p);
            if (!String.IsNullOrEmpty(Request.QueryString["query"]))
            {
                AdminDataQuery dq = db.AdminDataQueries.Single(c => c.ID.ToString() == Request.QueryString["query"]);

                if (!dq.AdminDataQueryFilters.Any())
                {
                    NoticeManager.SetMessage("This query has no filters.", "list.aspx");
                }

                IEnumerable<T> items = sender.DataSet.Cast<T>();

                IEnumerable<T> baseQuery = items; // make a copy so we can requery it later

                if (dq.AdminDataQueryFilters.Any(a => a.IsCustomField))
                {

                    try
                    {
                        if (dq.AdminDataQueryFilters.Any(a => !a.IsCustomField))
                            items = items.AsQueryable().Where(dq.ParsedQuery, dq.AdminDataQueryFilters.Select(a => a.Value)).ToArray();
                    }
                    catch(System.Linq.Dynamic.ParseException ex)
                    {
                        NoticeManager.SetMessage("Is not does not support True/false values.  Please use Is the same as.", "list.aspx");
                    }

                    AdminDataQueryFilter previousFilter = null;


                    foreach (AdminDataQueryFilter filter in dq.AdminDataQueryFilters.Where(c => c.IsCustomField))
                    {
                        int result = 0;
                        bool IsCustomField = Int32.TryParse(filter.DataFieldName, out result);

                        CRM_FormField field = db.CRM_FormFields.Single(s => s.ID == result);

                        IEnumerable<CRM_FormFieldAnswer> FormFieldAnswers = null;

                        var possibleAnswers = field.CRM_FormFieldAnswers;

                        if (filter.Operator == "==")
                        {
                            FormFieldAnswers = possibleAnswers
                                    .Where(cfa => cfa.Answer.ToLower() == filter.Value.ToLower() && cfa.CRM_FormFieldID == result);
                        }
                        else if (filter.Operator == "Contains")
                        {
                            FormFieldAnswers = possibleAnswers
                                                               .Where(cfa => cfa.Answer.ToLower().Contains(filter.Value.ToLower()) && cfa.CRM_FormFieldID == result);
                        }
                        else if (filter.Operator == "!Contains")
                        {
                            FormFieldAnswers = possibleAnswers
                                   .Where(cfa => !cfa.Answer.ToLower().Contains(filter.Value.ToLower()) && cfa.CRM_FormFieldID == result);
                        }
                        else if (filter.Operator == ">")
                        {
                            FormFieldAnswers = possibleAnswers.Where(cfa => Convert.ToDecimal(cfa.Answer) > Convert.ToDecimal(filter.Value) && cfa.CRM_FormFieldID == result);
                        }
                        else if (filter.Operator == ">=")
                        {
                            FormFieldAnswers = possibleAnswers.Where(cfa => Convert.ToDecimal(cfa.Answer) >= Convert.ToDecimal(filter.Value) && cfa.CRM_FormFieldID == result);
                        }
                        else if (filter.Operator == "<")
                        {
                            FormFieldAnswers = possibleAnswers.Where(cfa => Convert.ToDecimal(cfa.Answer) < Convert.ToDecimal(filter.Value) && cfa.CRM_FormFieldID == result);
                        }
                        else if (filter.Operator == "<=")
                        {
                            FormFieldAnswers = possibleAnswers.Where(cfa => Convert.ToDecimal(cfa.Answer) <= Convert.ToDecimal(filter.Value) && cfa.CRM_FormFieldID == result);
                        }


                        if (FormFieldAnswers != null && FormFieldAnswers.Any())
                        {
                            if (previousFilter != null && previousFilter.Concat == "OR")
                            {
                                items = items.Concat(baseQuery.Where(x => FormFieldAnswers.Any(fa => ((ICustomField)x).Reference == fa.TargetReference)).Cast<T>());
                            }
                            else
                            {
                                // if previous item is an OR, then concat, otherwise continue to query current set
                                items = items.Where(x => FormFieldAnswers.Any(fa => ((ICustomField)x).Reference == fa.TargetReference)).Cast<T>();
                            }
                        }

                        previousFilter = filter;
                    }


                    sender.DataSet = items.Select(c => (object)c);

                }
                else
                {
                    try
                    {

                        items = items.AsQueryable().Where(dq.ParsedQuery);
                    }
                    catch (System.Linq.Dynamic.ParseException ex)
                    {
                        if (AdminUser.ID == 1)
                            throw ex;

                        NoticeManager.SetMessage("There was a problem running this query - did you try and compare a number to a display value?", "list.aspx");
                    }
                    sender.DataSet = items.Select(c => (object)c);
                }

            }

            return sender;
        }
    }

}