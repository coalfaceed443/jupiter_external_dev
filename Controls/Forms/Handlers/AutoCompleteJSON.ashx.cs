using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;
using CRM.Code.Helpers;
using CRM.Code.Utils.Enumeration;
using CRM.Code.BasePages.Admin;
using System.Web.UI;
using CRM.Code.Interfaces;

namespace CRM.Controls.Forms.Handlers
{
    /// <summary>
    /// Summary description for AutoCompleteJSON
    /// </summary>
    public class AutoCompleteJSON : IHttpHandler
    {
        private string EmptyOutput = "NO DATASET";
        public void ProcessRequest(HttpContext context)
        {
            string JSONOutput = EmptyOutput;

            context.Response.AddHeader("Content-type", "text/json");
            using (MainDataContext db = new MainDataContext())
            {
                string dataset = context.Request.QueryString["dataset"];

             
                int parseddataset = Enumeration.GetEnumValueByName<JSONSet.DataSets>(dataset);
                string searchCriteria = HttpUtility.HtmlDecode(context.Request.QueryString["query"]).ToLower();

                IEnumerable<JSONSet> jsonItems = Enumerable.Empty<JSONSet>();

                switch (parseddataset)
                {
                    case (int)JSONSet.DataSets.venue:
                        {
                            var venues = CRM_Venue.BaseSet(db);
                            jsonItems = from p in venues
                                            where p.Tokens.Any(t => t.Contains(searchCriteria))
                                            select new JSONSet(p.Name + " : " + p.CRM_Address.FormattedAddressBySep(", "), p.CRM_Address.Postcode, p.Reference.ToString(), "", p.Reference);
                        }
                        break;

                    case (int)JSONSet.DataSets.person:
                        {
                            var persons = CRM_Person.BaseSet(db);
                            jsonItems = from p in persons
                                        where !p.IsArchived
                                        where p.Tokens.Any(t => t.Contains(searchCriteria))
                                        select new JSONSet(p.Fullname + " : " + p.CRM_Address.FormattedAddressBySep(", "), p.DateOfBirthOutput, p.ID.ToString(), p.Photo, p.Reference);
                        }
                        break;


                    case (int)JSONSet.DataSets.contact:
                        {
                            var unarchivedContacts = from p in CRM.Code.Utils.SharedObject.SharedObject.GetSharedObjects<IContact>(db)                                                     
                                        where !p.IsArchived
                                        select p;

                            jsonItems = from p in unarchivedContacts
                                        where p.Tokens.Any(t => t.ToLower().Contains(searchCriteria.ToLower()))
                                        orderby p.Lastname, p.Firstname
                                        select new JSONSet(p.Fullname + " : " + p.PrimaryAddress.FormattedAddressBySep(", "), p.OutputTableName, p.Reference, p.Photo, p.CRM_PersonReference);
                        }
                        break;

                    case (int)JSONSet.DataSets.organisation:
                        {
                            var orgs = CRM_Organisation.BaseSet(db);
                            jsonItems = from p in orgs
                                        where p.Tokens.Any(t => t.Contains(searchCriteria))
                                        select new JSONSet(p.Name + " : " + p.CRM_Address.FormattedAddressBySep(", "), p.CRM_OrganisationType.Name, p.ID.ToString(), "", p.Reference);
                        }
                        break;

                    case (int)JSONSet.DataSets.school:
                        {
                            var schools = CRM_School.BaseSet(db);
                            jsonItems = from p in schools
                                        where p.Tokens.Any(t => t.Contains(searchCriteria))
                                        select new JSONSet(p.Name + " : " + p.CRM_Address.FormattedAddressBySep(", "), p.CRM_SchoolType.Name, p.ID.ToString(), "", p.Reference);
                        }
                        break;

                    case (int)JSONSet.DataSets.schoolperson:
                        {
                            var schoolsp= CRM_PersonSchool.BaseSet(db);
                            jsonItems = from p in schoolsp
                                        where !p.IsArchived
                                        where p.Tokens.Any(t => t.Contains(searchCriteria))
                                        select new JSONSet(p.Name + " : " + p.PrimaryAddress.FormattedAddressBySep(", "), p.CRM_School.Name, p.ID.ToString(), p.CRM_Person.Photo, p.Reference);
                        }
                        break;

                    case (int)JSONSet.DataSets.orgperson:
                        {
                            var orgpers = CRM_PersonOrganisation.BaseSet(db);
                            jsonItems = from p in orgpers
                                        where !p.IsArchived
                                        where p.Tokens.Any(t => t.Contains(searchCriteria))
                                        select new JSONSet(p.Name + " : " + p.PrimaryAddress.FormattedAddressBySep(", "), p.CRM_Organisation.Name, p.ID.ToString(), p.CRM_Person.Photo, p.Reference);
                        }
                        break;

                    case (int)JSONSet.DataSets.family:
                        {
                            var families = CRM_Family.BaseSet(db);
                            jsonItems = from p in families
                                        where p.Tokens.Any(t => t.Contains(searchCriteria))
                                        select new JSONSet(p.Name + " : " + p.CRM_FamilyPersons.Count() + " members", p.MemberList, p.ID.ToString(), "", p.Reference);
                        }
                        break;

                    case (int)JSONSet.DataSets.mytasks:
                        {
                            var tasks = db.CRM_Tasks.ToArray().Where(c => c.IsVisible(Convert.ToInt32(HttpContext.Current.Request.QueryString["adminuserid"])));
                            jsonItems = from p in tasks
                                        where p.Tokens.Any(t => t.Contains(searchCriteria))
                                        select new JSONSet(p.Name + "", p.DueDate.ToString("dd/MM/yyyy"), p.ID.ToString(), "", p.Reference);
                        }
                        break;


                    case (int)JSONSet.DataSets.admin:
                        {
                            var admins = db.Admins.ToArray();
                            jsonItems = from p in admins
                                        where p.Tokens.Any(t => t.Contains(searchCriteria))
                                        select new JSONSet(p.DisplayName + "", 
                                            p.LastLogin == null ? "Never logged in" : "Last Login: " + ((DateTime)p.LastLogin).ToString("dd/MM/yyyy HH:mm"),
                                            p.ID.ToString(), "", p.ID.ToString());
                        }
                        break;

                    case (int)JSONSet.DataSets.schoolorgs:
                        {
                            var schoolorgs = CRM.Code.Utils.SharedObject.SharedObject.GetSharedObjects<ISchoolOrganisation>(db);

                            jsonItems = from p in schoolorgs
                                        where p.Tokens.Any(t => t.Contains(searchCriteria))
                                        select new JSONSet(p.Name + " : " + p.CRM_Address.FormattedAddressBySep(", "), p.OutputTableName,
                                            p.Reference, "", p.Reference.ToString());

                        }
                        break;
                }

                if (jsonItems.Any())
                    JSONOutput = JSONSet.ConvertToJSON(jsonItems);
                else
                {
                    jsonItems = Enumerable.Concat(jsonItems, new []{new JSONSet("No results found", "Please alter your search", "", "", "")});
                    JSONOutput = JSONSet.ConvertToJSON(jsonItems);
                }
                context.Response.Write(JSONOutput);

            }

            context.Response.End();

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}