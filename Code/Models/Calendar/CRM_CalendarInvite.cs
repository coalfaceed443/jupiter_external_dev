using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Controls.Forms.Handlers;

namespace CRM.Code.Models
{
    public partial class CRM_CalendarInvite
    {

        public string DetailsURL
        {
            get
            {
                return "/admin/calendar/invite/list.aspx?id=" + this.CRM_CalendarID;
            }
        }


        public string IsAttendedActionLinkURL
        {
            get
            {
                return ActionLink.FormURL(ActionLink.Route.ToggleInviteIsAttended, this.ID, this.DetailsURL);
            }
        }

        public string IsInvitedActionLinkURL
        {
            get
            {
                return ActionLink.FormURL(ActionLink.Route.ToggleInviteIsInvited, this.ID, this.DetailsURL);
            }
        }

        public string IsCancelledActionLinkURL
        {
            get
            {
                return ActionLink.FormURL(ActionLink.Route.ToggleInviteIsCancelled, this.ID, this.DetailsURL);
            }
        }

        public string IsBookedActionLinkURL
        {
            get
            {
                return ActionLink.FormURL(ActionLink.Route.ToggleInviteIsBooked, this.ID, this.DetailsURL);
            }
        }

        [IsListData("Is Invited")]
        public string IsInvitedDisplay
        {
            get
            {
                return Utils.Text.Text.ConvertUrlsToLinks(IsInvitedActionLinkURL, this.IsInvited ? "Yes" : "No");
            }
        }

        [IsListData("Is Cancelled")]
        public string IsCancelledDisplay
        {
            get
            {
                return Utils.Text.Text.ConvertUrlsToLinks(IsCancelledActionLinkURL, this.IsCancelled ? "Yes" : "No");
            }
        }

        [IsListData("Calendar Event")]
        public string CalendarEventDisplay
        {
            get
            {
                return this.CRM_Calendar.DisplayName;
            }
        }
        
        [IsListData("Is Booked")]
        public string IsBookedDisplayed
        {
            get
            {
                return Utils.Text.Text.ConvertUrlsToLinks(IsBookedActionLinkURL, this.IsBooked ? "Yes" : "No");
            }
        }


        [IsListData("Is Attended")]
        public string IsAttendedDisplayed
        {
            get
            {
                return Utils.Text.Text.ConvertUrlsToLinks(IsAttendedActionLinkURL, this.IsAttended ? "Yes" : "No");
            }
        }

        [IsListData("Delete")]
        public string DeleteActionURL
        {
            get
            {
                return Utils.Text.Text.ConvertUrlsToLinks(ActionLink.FormURL(ActionLink.Route.DeleteInvite, this.ID, this.DetailsURL), "Delete");
            }
        }

        [IsListData("Last amended by")]
        public string LastAmendedAdmin
        {
            get
            {
                return this.Admin.DisplayName;
            }
        }

    }
}