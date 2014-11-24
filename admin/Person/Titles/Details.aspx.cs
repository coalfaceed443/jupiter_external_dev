using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Managers;
using CRM.Code.Models;

namespace CRM.admin.Person.Titles
{
    public partial class Details : AdminPage
    {
        protected CRM.Code.Models.CRM_Title Entity = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);
            Entity = db.CRM_Titles.SingleOrDefault(c => c.ID.ToString() == Request.QueryString["id"]);
            
            btnSubmitChanges.Visible = PermissionManager.CanUpdate;
            if (!PermissionManager.CanAdd && Entity == null)
                Response.Redirect("list.aspx");

            // buttons //

            btnSubmit.EventHandler = btnSubmit_Click;
            btnSubmitChanges.EventHandler = btnSubmitChanges_Click;
            btnDelete.EventHandler = btnDelete_Click;

            // Security //

            btnSubmitChanges.Visible = PermissionManager.CanUpdate;
            btnDelete.Visible = PermissionManager.CanDelete;
            if (!PermissionManager.CanAdd && Entity == null)
                Response.Redirect("list.aspx");

            // confirmations //

            confirmationDelete.StandardDeleteHidden("title", btnRealDelete_Click);

            // process //
            CRMContext = Entity;

            if (!IsPostBack)
            {
                if (Entity != null)
                    PopulateFields();
            }
        }



        private void PopulateFields()
        {
            txtName.Text = Entity.Name;
            chkIsActive.Checked = Entity.IsActive;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(true);

                NoticeManager.SetMessage("Item Added", "details.aspx?id=" + Entity.ID);
            }
        }

        protected void btnSubmitChanges_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(false);

                NoticeManager.SetMessage("Item Updated");
            }
        }

        protected void SaveRecord(bool newRecord)
        {
            // new record / exiting record //

            object oldEntity = null;

            if (newRecord)
            {
                Entity = new CRM_Title();
                Entity.OrderNo = CRM.Code.Utils.Ordering.Ordering.GetNextOrderID(db.CRM_Titles);
                db.CRM_Titles.InsertOnSubmit(Entity);
            }

            Entity.Name = txtName.Text;
            Entity.IsActive = chkIsActive.Checked;

            db.SubmitChanges();

            if (oldEntity != null)
            {
                CRM.Code.History.History.RecordLinqUpdate(AdminUser, oldEntity, Entity);
                db.SubmitChanges();
            }
            else
            {
                CRM.Code.History.History.RecordLinqInsert(AdminUser, Entity);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            confirmationDelete.ShowPopup();
        }


        protected void btnRealDelete_Click(object sender, EventArgs e)
        {
            Entity.IsArchived = true;
            db.SubmitChanges();

            NoticeManager.SetMessage("Item removed", "list.aspx");
        }


    }
}