using CRM.Code.BasePages.Admin;
using CRM.Code.Managers;
using CRM.Code.Models;
using CRM.Code.Utils.Ordering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM.admin.Attendance.AttendanceTypes
{
    public partial class Details : AdminPage
    {
        public CRM_AttendancePersonType Entity;

        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);

            int id = 0;
            if(int.TryParse(Request.QueryString["id"], out id))
            {
                Entity = db.CRM_AttendancePersonTypes.FirstOrDefault(a => a.ID == id);
            }

            // buttons //
            btnDelete.EventHandler = btnDelete_Click;
            btnSubmit.EventHandler = btnSubmit_Click;
            btnSubmitChanges.EventHandler = btnSubmitChanges_Click;
            btnReinstate.EventHandler = btnReinstate_Click;

            confirmationDelete.StandardDeleteHidden("Attendance Type", btnRealDelete_Click);

            if (!Page.IsPostBack)
            {
                if (Entity != null)
                { PopulateFields(); }
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

                NoticeManager.SetMessage("Item Added", "list.aspx");
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
            if (newRecord)
            {
                Entity = new CRM_AttendancePersonType();

                Entity.OrderNo = Ordering.GetNextOrderID(db.CRM_AttendancePersonTypes);
                Entity.IsArchived = false;

                db.CRM_AttendancePersonTypes.InsertOnSubmit(Entity);
            }

            Entity.Name = txtName.Text;
            Entity.IsActive = chkIsActive.Checked;

            db.SubmitChanges();
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

        protected void btnReinstate_Click(object sender, EventArgs e)
        {
            Entity.IsArchived = false;
            db.SubmitChanges();

            NoticeManager.SetMessage("Item reinstated");
        }
    }
}