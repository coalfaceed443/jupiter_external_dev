using System;
using System.Web.UI;
using System.Linq;
using System.IO;
using System.Web.UI.WebControls;
using CRM.Code.Models;
using CRM.Code.Utils.Ordering;
using CRM.Code.Utils;
using System.Threading;
using CodeCarvings.Piczard;
using CRM.Code.Utils.Error;
using System.Collections.Generic;
using System.Collections;
using CRM.Code.Managers;
using CRM.Code.Utils.Enumeration;
using CRM.Code.History;
using System.Web;
using CRM.Code.Utils.WebControl;
using CRM.Code.Helpers;
using CRM.Code.Interfaces;
using CRM.Code;
using CRM.Code.BasePages.Admin.Calendar;
namespace CRM.Admin.Calendar
{
    public partial class Details : CRM_CalendarPage<CRM_Calendar>
    {
        protected CRM.Code.Models.CRM_Calendar Entity;

        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.Calendar);

            Entity = db.CRM_Calendars.SingleOrDefault(c => c.ID.ToString() == Request.QueryString["id"]);

            ucLogHistory.IHistory = Entity;

            btnSubmitChanges.Visible = PermissionManager.CanUpdate;

            ucNavCal.Entity = Entity;
            ucLogNotes.INotes = Entity;
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

            acPersons.EventHandler = lnkSelect_Click;
            acPersons.Config = new AutoCompleteConfig(JSONSet.DataSets.contact);
      
            // confirmations //

            CRMContext = Entity;
            confirmationDelete.StandardDeleteHidden("Calendar", btnRealDelete_Click);
            
            if (!Page.IsPostBack)
            {
                ddlStatus.DataSource = Enumeration.GetAll<CRM_Calendar.StatusTypes>();
                ddlStatus.DataBind();

                ddlPrivacy.DataSource = Enumeration.GetAll<CRM_Calendar.PrivacyTypes>();
                ddlPrivacy.DataBind();

                ddlCalendarType.DataSource = from c in db.CRM_CalendarTypes
                                             orderby c.OrderNo
                                             select c;
                ddlCalendarType.DataBind();

                if (Entity != null)
                {
                    lblCreatedBy.Text = db.Admins.First(c => c.ID == Entity.CreatedByAdminID).DisplayName;
                    PopulateFields();
                }
                else
                {
                    lblCreatedBy.Text = AdminUser.DisplayName;
                    DateTime startDate = CRM.Code.Utils.Text.Text.FormatInputDateTime(HttpUtility.UrlDecode(Request.QueryString["slot"]));
                    txtStartDate.Value = startDate;
                    txtEndDate.Value = startDate.AddHours(1);
                }
            }
        }

        protected void lnkSelect_Click(object sender, EventArgs e)
        {
            IAutocomplete contact = (IAutocomplete)CRM.Code.Utils.SharedObject.SharedObject.GetSharedObjects<IContact>(db).Single(v => v.Reference.ToString() == acPersons.SelectedID);
            acPersons.Populate(contact);
        }

        private void PopulateFields()
        {
            txtDisplayName.Text = Entity.DisplayName;
            txtEndDate.Value = Entity.EndDateTime;
            txtStartDate.Value = Entity.StartDateTime;
            ddlCalendarType.SelectedValue = Entity.CRM_CalendarTypeID.ToString();
            ddlStatus.SelectedValue = Entity.Status.ToString();
            ddlPrivacy.SelectedValue = Entity.PrivacyStatus.ToString();
            chkIsCancelled.Checked = Entity.IsCancelled;
            if (Entity.PrimaryContactReference != String.Empty)
                acPersons.Populate((IAutocomplete)CRM.Code.Utils.SharedObject.SharedObject.GetSharedObjects<IContact>(db).Single(s => s.Reference == Entity.PrimaryContactReference));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(true);

                NoticeManager.SetMessage("Calendar Added", Entity.NextStageURL);
            }
        }

        protected void btnSubmitChanges_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(false);

                NoticeManager.SetMessage("Calendar Updated");
            }
        }

        protected void SaveRecord(bool newRecord)
        {
            
            // new record / exiting record //
            object oldEntity = null;

            if (newRecord)
            {
                Entity = new CRM_Calendar();
                Entity.CancellationReason = "";
                Entity.TargetReference = CRM_Calendar.DefaultTargetReference;
                Entity.PriceAgreed = 0M;
                Entity.PriceType = 0;
                Entity.InvoiceTitle = "";
                Entity.InvoiceFirstname = "";
                Entity.InvoiceLastname = "";
                Entity.DatePaid = null;
                Entity.PONumber = "";
                Entity.PrimaryContactReference = "";
                db.CRM_Calendars.InsertOnSubmit(Entity);
            }
            else
            {
                oldEntity = Entity.ShallowCopy();
            }

            Entity.CRM_CalendarType = db.CRM_CalendarTypes.Single(c => c.ID == Convert.ToInt32(ddlCalendarType.SelectedValue));
            Entity.CreatedByAdminID = AdminUser.ID;
            Entity.DisplayName = txtDisplayName.Text;
            Entity.EndDateTime = txtStartDate.Value.Date.AddHours(txtEndDate.Value.Hour).AddMinutes(txtEndDate.Value.Minute);
            Entity.StartDateTime = txtStartDate.Value;
            Entity.Status = Convert.ToByte(ddlStatus.SelectedValue);
            Entity.RequiresCatering = false;
            Entity.PrivacyStatus = Convert.ToByte(ddlPrivacy.SelectedValue);
            Entity.IsCancelled = chkIsCancelled.Checked;

            if (acPersons.SelectedID != "")
                Entity.PrimaryContactReference = acPersons.SelectedID;
            
            db.SubmitChanges();

            if (!newRecord)
            {

                if (((CRM_Calendar)oldEntity).StartDateTime != Entity.StartDateTime || ((CRM_Calendar)oldEntity).EndDateTime != Entity.EndDateTime)
                {
                    foreach (CRM_CalendarAdmin invitation in Entity.CRM_CalendarAdmins)
                    {
                        EmailManager manager = new EmailManager();
                        manager.SendTimeChange((CRM_Calendar)oldEntity, invitation, db, AdminUser);
                    }
                }

                // if the entry has moved days, then updated all the venue allocations also to the new date.
                if (((CRM_Calendar)oldEntity).StartDateTime.Date != Entity.StartDateTime.Date)
                {
                    foreach (CRM_CalendarVenue venue in Entity.CRM_CalendarVenues)
                    {
                        venue.DateTimeFrom = new DateTime(Entity.StartDateTime.Year, Entity.StartDateTime.Month, Entity.StartDateTime.Day, venue.DateTimeFrom.Hour, venue.DateTimeFrom.Minute, 0);
                        venue.DateTimeTo = new DateTime(Entity.EndDateTime.Year, Entity.EndDateTime.Month, Entity.EndDateTime.Day, venue.DateTimeTo.Hour, venue.DateTimeTo.Minute, 0);
                        db.SubmitChanges();
                    }
                }
            }


            if (oldEntity != null)
            {
                History.RecordLinqUpdate(AdminUser, oldEntity, Entity);
                db.SubmitChanges();
            }
            else
            {
                History.RecordLinqInsert(AdminUser, Entity);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            confirmationDelete.ShowPopup();
        }

        protected void btnRealDelete_Click(object sender, EventArgs e)
        {
            Entity.DeleteFromDatabase(db, AdminUser);

            NoticeManager.SetMessage("Entry Deleted", "/admin/calendar");
        }


       
    }
}
