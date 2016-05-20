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

namespace CRM.admin.Attendance.AttendanceEvent
{
    public partial class Details : AdminPage
    {
        public CRM_AttendanceEvent Entity;

        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);

            int id = 0;
            if(int.TryParse(Request.QueryString["id"], out id))
            {
                Entity = db.CRM_AttendanceEvents.FirstOrDefault(a => a.ID == id);
            }

            // buttons //
            btnDelete.EventHandler = btnDelete_Click;
            btnSubmit.EventHandler = btnSubmit_Click;
            btnSubmitChanges.EventHandler = btnSubmitChanges_Click;
            confirmationDelete.StandardDeleteHidden("Attendance Event", btnRealDelete_Click);

            if (!Page.IsPostBack)
            {
                if (Entity != null)
                { PopulateFields(); }
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
                Entity = new CRM_AttendanceEvent();

                db.CRM_AttendanceEvents.InsertOnSubmit(Entity);
            }

            Entity.Name = txtName.Text;

            db.SubmitChanges();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            confirmationDelete.ShowPopup();
        }


        protected void btnRealDelete_Click(object sender, EventArgs e)
        {
            db.CRM_AttendanceEvents.DeleteOnSubmit(Entity);
            db.SubmitChanges();

            NoticeManager.SetMessage("Item removed", "list.aspx");
        }


    }
}