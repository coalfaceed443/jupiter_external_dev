using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Managers;
using CRM.Code.Models;
using CRM.Code.BasePages.Admin.Calendar;

using CRM.Code.Helpers;
using CRM.Code.Utils.Time;

namespace CRM.admin.Calendar.Task
{
    public partial class Details : CRM_CalendarPage<CRM_TaskParticipant>
    {
        protected CRM.Code.Models.CRM_Task Task = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);
            Task = db.CRM_Tasks.FirstOrDefault(f => f.Reference == Entity.TargetReference && Entity.TargetReference != CRM_Calendar.DefaultTargetReference); 
            
            btnSubmitChanges.Visible = PermissionManager.CanUpdate;
            if (!PermissionManager.CanAdd && Entity == null)
                Response.Redirect("list.aspx");

            ucNavCal.Entity = Entity;
            ucLogNotes.INotes = Entity;

            CRMContext = Entity;
            // buttons //

            btnSubmit.EventHandler = btnSubmit_Click;
            btnSubmitChanges.EventHandler = btnSubmitChanges_Click;

            // Security //

            btnSubmitChanges.Visible = PermissionManager.CanUpdate;
            if (!PermissionManager.CanAdd && Entity == null)
                Response.Redirect("list.aspx");

            ucListView.Type = typeof(CRM_TaskParticipant);
            ucListView.ItemsPerPage = 100;
            ucListView.ShowExport = false;

            if (Task != null)
            {
                
                ucListView.DataSet = db.CRM_TaskParticipants.Where(t => t.CRM_TaskID == Task.ID).Select(p => (object)p);

            }

            // process //

            if (!IsPostBack)
            {

                if (Task != null)
                    PopulateFields();
                else
                {
                    txtDueDate.Value = Entity.StartDateTime;
                    txtName.Text = Entity.DisplayName;
                    lblOwner.Text = AdminUser.DisplayName;
                    pnlAddParticipant.Visible = false;
                }

            }

            LoadParticipantList();

        }

        private void PopulateFields()
        {
            lblOwner.Text = db.Admins.Single(a => a.ID == Task.OwnerAdminID).DisplayName;
            txtName.Text = Task.Name;
            chkIsCancelled.Checked = Task.IsCancelled;
            chkIsPublic.Checked = Task.IsPublic;
            chkIsCompleted.Checked = Task.IsComplete;
            txtDueDate.Value = Task.DueDate;
            txtDescription.Text = Task.Description;
            ucLogNotes.INotes = Entity;
            ucLogHistory.IHistory = Task;
            ucLogHistory.ParentID = Entity.ID.ToString();
            
            pnlAddParticipant.Visible = true;

            LoadParticipantList();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(true);

                NoticeManager.SetMessage("Item Added", "details.aspx?id=" + Entity.ID + "&pid=" + Task.ID);
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
                Task = new CRM_Task();
                Task.OwnerAdminID = AdminUser.ID;
                Task.CRM_CalendarID = Entity.ID;
                db.CRM_Tasks.InsertOnSubmit(Task);
            }
            else
            {
                oldEntity = Task.ShallowCopy();
            }

            Task.Name = txtName.Text;
            Task.IsCancelled = chkIsCancelled.Checked;
            Task.IsPublic = chkIsPublic.Checked;
            Task.DueDate = txtDueDate.Value;
            Task.Description = txtDescription.Text;
            Task.IsComplete = chkIsCompleted.Checked;
            db.SubmitChanges();

            if (oldEntity != null)
            {
                CRM.Code.History.History.RecordLinqUpdate(db, AdminUser, oldEntity, Task);
                db.SubmitChanges();
            }
            else
            {
                Entity.TargetReference = Task.Reference;
                db.SubmitChanges();

                AddAdminToTask(AdminUser.ID);

                CRM.Code.History.History.RecordLinqInsert(AdminUser, Task);
            }
        }

        protected void AddAdminToTask(int AdminID)
        {
            CRM_TaskParticipant taskPart = new CRM_TaskParticipant()
            {
                AdminID = AdminID,
                CRM_TaskID = Task.ID,
                DateAssigned = UKTime.Now,
                IsArchived = false
            };

            db.CRM_TaskParticipants.InsertOnSubmit(taskPart);
            db.SubmitChanges();
        }

        protected void ddlAddParticipant_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAddParticipant.SelectedValue != "")
            {
                AddAdminToTask(Convert.ToInt32(ddlAddParticipant.SelectedValue));
                NoticeManager.SetMessage(ddlAddParticipant.SelectedItem.Text + " added");
            }
        }

        private void LoadParticipantList()
        {


            ddlAddParticipant.DataSource = from a in db.Admins.ToArray()
                                           where (Task != null && !Task.CRM_TaskParticipants.Any(t => t.AdminID == a.ID)) || Task == null
                                           orderby a.FirstName
                                           select a;

            ddlAddParticipant.DataBind();
        }



    }
}