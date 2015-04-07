using CRM.Code.BasePages.Admin;
using CRM.Code.Managers;
using CRM.Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM.admin.RelationshipCode
{
    public partial class Details : AdminPage
    {
        protected CRM_RelationCode Entity = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);
            Entity = db.CRM_RelationCodes.SingleOrDefault(c => c.ID.ToString() == Request.QueryString["id"]);

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

            confirmationDelete.StandardDeleteHidden("role", btnRealDelete_Click);

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
                Entity = new CRM_RelationCode();
                db.CRM_RelationCodes.InsertOnSubmit(Entity);
            }

            Entity.Name = txtName.Text;

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
            if (Entity.CRM_PersonRelationships.Any() || Entity.CRM_PersonRelationships1.Any())
            {
                NoticeManager.SetMessage("This code still has people attached.  Please search for and reassign these codes before deleting.");
            }
            else
            {
                confirmationDelete.ShowPopup();
            }
        }


        protected void btnRealDelete_Click(object sender, EventArgs e)
        {
            db.CRM_RelationCodes.DeleteOnSubmit(Entity);
            db.SubmitChanges();

            NoticeManager.SetMessage("Item removed", "list.aspx");
        }


    }
}