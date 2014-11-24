using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Models;
using CRM.Code.History;
using CRM.Code.Managers;
using CRM.Code.Utils.WebControl;
using CRM.Code.Helpers;
using CRM.Code.BasePages.Admin.Calendar;
using CRM.Code.Utils.Time;

namespace CRM.admin.Calendar.Venues
{
    public partial class Default : CRM_CalendarPage<CRM_CalendarVenue>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.Calendar);

            ucLogHistory.TableName = typeof(CRM_CalendarVenue).Name;
            ucLogHistory.ParentID = Entity.ID.ToString();


            btnSubmit.EventHandler = btnSubmit_Click;
            acVenue.EventHandler = lnkSelect_Click;
            acVenue.Config = new AutoCompleteConfig(JSONSet.DataSets.venue);
            ucNavCal.Entity = Entity;

            btnSendChange.EventHandler = btnSendChange_Click;

            CRMContext = Entity;
            if (!Page.IsPostBack)
            {
                txtFrom.Value = Entity.StartDateTime;
                txtTo.Value = Entity.EndDateTime;



                LoadData();
            }
        }

        protected void LoadData()
        {
            lvItems.DataSource = Entity.CRM_CalendarVenues.OrderBy(v => v.DateTimeFrom);
            lvItems.DataBind();
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                string id = acVenue.SelectedID;
                CRM_CalendarVenue venue = new CRM_CalendarVenue()
                {
                    DateTimeTo = txtTo.Value,
                    DateTimeFrom = txtFrom.Value,
                    CRM_CalendarID = Entity.ID,
                    CRM_VenueID = db.CRM_Venues.Single(v => v.Reference.ToString() == acVenue.SelectedID).ID
                };

                db.CRM_CalendarVenues.InsertOnSubmit(venue);
                db.SubmitChanges();

                CRM.Code.History.History.RecordLinqInsert(AdminUser, venue);

                NoticeManager.SetMessage("Venue added");
            }
        }

        protected void btnSendChange_Click(object sender, EventArgs e)
        {
            IEnumerable<CRM_CalendarAdmin> users = Entity.CRM_CalendarAdmins;
            foreach (CRM_CalendarAdmin invite in users)
            {
                EmailManager manager = new EmailManager();
                manager.AddTo(invite.Admin.Email);
                manager.SendVenueChange(txtMessageToTags.Text, invite, db, AdminUser);
                //invite.Status = Request.QueryString["attend"] == "false" ? (byte)CRM_CalendarAdmin.StatusTypes.NotAttending : (byte)CRM_CalendarAdmin.StatusTypes.Attending;
            }

            CRM_Note note = new CRM_Note();
            note.Body = txtMessageToTags.Text;
            note.Title = "Users notified of venue change";
            note.DateCreated = UKTime.Now;
            note.TargetReference = Entity.Reference;
            note.OwnerAdminID = 1;
            db.CRM_Notes.InsertOnSubmit(note);
            db.SubmitChanges();

            NoticeManager.SetMessage(users.Count() + " user(s) notified");
        }

        protected void lnkSelect_Click(object sender, EventArgs e)
        {
            CRM_Venue venue = db.CRM_Venues.Single(v => v.Reference.ToString() == acVenue.SelectedID);
            acVenue.Populate(venue);
        }

        protected void lnkRemove_Click(object sender, EventArgs e)
        {
            CRM_CalendarVenue calven = db.CRM_CalendarVenues.Single(cv => cv.ID.ToString() == ((LinkButton)sender).CommandArgument);
            calven.DeleteFromDatabase(db, AdminUser);
            LoadData();
        }

        protected void lvItems_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            LinkButton lnkRemove = (LinkButton)e.Item.FindControl("lnkRemove");

            ListViewDataItem row = (ListViewDataItem)e.Item;
            CRM_CalendarVenue calvenue = (CRM_CalendarVenue)row.DataItem;

            lnkRemove.CommandArgument = calvenue.ID.ToString();
        }

    }
}