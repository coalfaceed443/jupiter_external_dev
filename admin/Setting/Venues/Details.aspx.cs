using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Managers;
using CRM.Code.Models;

namespace CRM.admin.Setting.Venues
{
    public partial class Details : AdminPage
    {
        protected CRM.Code.Models.CRM_Venue Entity = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);
            Entity = db.CRM_Venues.SingleOrDefault(c => c.ID.ToString() == Request.QueryString["id"]);
            
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

            confirmationDelete.StandardDeleteHidden("venue", btnRealDelete_Click);

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
            txtCapacity.Text = Entity.Capacity.ToString();
            chkIsInternal.Checked = Entity.IsInternal;
            ucAddress.Populate(Entity.CRM_Address);
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
            object oldAddress = null;
            if (newRecord)
            {
                Entity = new CRM_Venue();
                Entity.CRM_Address = new CRM_Address();
                db.CRM_Venues.InsertOnSubmit(Entity);
            }

            Entity.Name = txtName.Text;
            Entity.Capacity = Convert.ToInt32(txtCapacity.Text);
            Entity.IsInternal = chkIsInternal.Checked;
            ucAddress.Save(Entity.CRM_Address);
            db.SubmitChanges();

            if (oldEntity != null)
            {
                CRM.Code.History.History.RecordLinqUpdate(AdminUser, oldEntity, Entity);
                CRM.Code.History.History.RecordLinqUpdate(AdminUser, oldAddress, Entity.CRM_Address);
                db.SubmitChanges();
            }
            else
            {
                CRM.Code.History.History.RecordLinqInsert(AdminUser, Entity);
                CRM.Code.History.History.RecordLinqInsert(AdminUser, Entity.CRM_Address);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            confirmationDelete.ShowPopup();
        }


        protected void btnRealDelete_Click(object sender, EventArgs e)
        {
            db.SubmitChanges();

            NoticeManager.SetMessage("Item removed", "list.aspx");
        }


    }
}