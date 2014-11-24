using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;
using CRM.Code.Managers;
using System.Web.UI.HtmlControls;

namespace CRM.admin.Setting.AdminUser.Permissions
{
    public partial class List : AdminPage
    {
        protected CRM.Code.Models.Admin Entity;
        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.AdminUsers);

            int adminUserID = 0;
            if (Int32.TryParse(Request.QueryString["id"], out adminUserID) && adminUserID > 0)
            {
                Entity = db.Admins.SingleOrDefault(a => a.ID == adminUserID);
                if (Entity == null)
                    Response.Redirect("/admin/adminuser/list.aspx");
            }

            btnSubmitChangesTop.EventHandler = btnSubmitChanges_Click;
            btnSubmitChangesBottom.EventHandler = btnSubmitChanges_Click;

            if (!Page.IsPostBack)
            {

                bool newAdded = false;
                foreach (CRM_SystemAccess access in db.CRM_SystemAccesses)
                {
                    CRM_SystemAccessAdmin adminAccess = db.CRM_SystemAccessAdmins.SingleOrDefault(s => s.CRM_SystemAccessID == access.ID && s.AdminID == adminUserID);

                    if (adminAccess == null)
                    {
                        adminAccess = new CRM_SystemAccessAdmin()
                        {
                            AdminID = adminUserID,
                            CRM_SystemAccessID = access.ID,
                            IsAdd = false,
                            IsDelete = false,
                            IsRead = false,
                            IsWrite = false
                        };

                        db.CRM_SystemAccessAdmins.InsertOnSubmit(adminAccess);
                        db.SubmitChanges();
                        newAdded = true;
                    }

                }


                if (newAdded)
                    Response.Redirect(Request.RawUrl);

                rptItems.DataSource = from saa in db.Admins.SingleOrDefault(a => a.ID == adminUserID).CRM_SystemAccessAdmins
                                      where saa.CRM_SystemAccess != null
                                      where saa.CRM_SystemAccess.IsPermissable                                     
                                      orderby saa.CRM_SystemAccess.Path
                                      select saa;
                rptItems.DataBind();

            }
        }

        protected void btnSubmitChanges_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rptItems.Items)
            {
                CheckBox chkRead = (CheckBox)item.FindControl("chkRead");
                CheckBox chkAdd = (CheckBox)item.FindControl("chkAdd");
                CheckBox chkEdit = (CheckBox)item.FindControl("chkEdit");
                CheckBox chkDelete = (CheckBox)item.FindControl("chkDelete");
                HtmlInputHidden id = (HtmlInputHidden)item.FindControl("hdnPage");

                CRM_SystemAccessAdmin accessAdmin = db.CRM_SystemAccessAdmins.Single(c => c.ID.ToString() == id.Value);

                accessAdmin.IsAdd = chkAdd.Checked;
                accessAdmin.IsDelete = chkDelete.Checked;
                accessAdmin.IsWrite = chkEdit.Checked;
                accessAdmin.IsRead = chkRead.Checked;
                db.SubmitChanges();
            }

            NoticeManager.SetMessage("Permissions Saved");
        }


    }
}