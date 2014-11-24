using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;

namespace CRM.Code.Models
{
    public partial class CRM_CalendarVenue : IHistory, ICRMRecord, IClash
    {
        public string _clashColourID = "";
        public string ClashColourID
        {
            get
            {
                return _clashColourID;
            }
            set
            {
                _clashColourID = value;
            }
        }

        [IsListData("Calendar Event")]
        public string CalendarEventName
        {
           get
           {
               return this.CRM_Calendar.DisplayName;
           }
        }

        [IsListData("Event Type")]
        public string EventType
        {
            get
            {
                return this.CRM_Calendar.CRM_CalendarType.Name;
            }
        }

        [IsListData("View Event")]
        public string CalendarEventURL
        {
            get
            {
                return Utils.Text.Text.ConvertUrlsToLinks(this.CRM_Calendar.DetailsURL, "View Event");
            }
        }


        [IsListData("Clash Visual")]
        public string ClashColourOutput
        {
            get
            {
                return "<div style=\"background-color:" + this.ClashColourID + "\">&nbsp;</div>";
            }
        }

        public object ShallowCopy()
        {
            return (CRM_CalendarVenue)this.MemberwiseClone();
        }

        public string TableName
        {
            get
            {
                return this.GetType().Name;
            }
        }


        public string OutputTableName
        {
            get
            {
                return "Calendar Venue";
            }
        }

        [IsListData("Time Range")]
        public string DisplayTimeRange
        {
            get
            {
                return this.DateTimeFrom.ToString("dd/MM/yyyy HH:mm") + " - " + this.DateTimeTo.ToString("HH:mm");
            }
        }

        [IsListData("Venue")]
        public string Venue
        {
            get
            {
                return this.CRM_Venue.Name + " (" + this.CRM_Venue.CRM_Address.Postcode + ")";
            }
        }

        [IsListData("Edit Calendar Venue")]
        public string EditCalendarVenue
        {
            get
            {
                return Utils.Text.Text.ConvertUrlsToLinks(EditCalendarVenueURL, "Edit");
            }
        }

        public string EditCalendarVenueURL
        {
            get
            {
                return "/admin/venues/default.aspx?id=" + this.CRM_CalendarID;
            }
        }

        public int ParentID
        {
            get
            {
                return this.CRM_CalendarID;
            }
        }

    }
}