using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Utils.Enumeration;
using CRM.Code.Interfaces;
using CRM.Code.History;
using CRM.Code.Helpers;
using System.Web.UI.WebControls;

namespace CRM.Code.Models
{
    public partial class CRM_Calendar : IHistory, ICRMRecord, INotes, ICRMContext
    {
        private const int SlotHeightpxPerMinute = 1;

        public static IEnumerable<CRM_Calendar> BaseSet(MainDataContext db)
        {
            return db.CRM_Calendars;
        }

        [IsListData("Has Facilitator")]
        public string HasFacilitatorOutput
        {
            get
            {
                return HasFacilitator ? "Yes" : "No";
            }
        }

        public List<string> FlatList
        {
            get
            {
                return this.CRM_CalendarAdmins.Select(s => s.AdminName).ToList();
            }
        }

        [IsListData("Access List")]
        public string AccessList
        {
            get
            {               
                return JSONSet.FlattenList(FlatList);
            }
        }


        [IsListData("Owned By")]
        public string OwnedBy
        {
            get
            {
                return this.Admin.DisplayName;
            }
        }

        public bool HasFacilitator
        {
            get
            {
                return this.CRM_CalendarGroupBookings.Any(g => !String.IsNullOrEmpty(g.Facilitator) || !String.IsNullOrEmpty(g.FacilitatorTwo) || !String.IsNullOrEmpty(g.FacilitatorThree)) || this.CRM_CalendarOutreaches.Any(g => !String.IsNullOrEmpty(g.Facilitator));
            }
        }
    
    

        public Code.Calendar.CalendarSlot CalendarSlot
        {
            get
            {
                return new Calendar.CalendarSlot(this.StartDateTime.ToString("HH:mm"))
                {
                    CalendarItem = this,
                    NewDetailsURL = "/admin/calendar/details.aspx?id=" + this.ID
                };
            }
        }

        public string AbsoluteDetailsURL
        {
            get
            {
                var detailsURL = DetailsURL;
                return Constants.DomainName + detailsURL.Substring(1, detailsURL.Length - 1);
            }
        }

        public string VenueDetailsURL
        {
            get
            {
                return Constants.DomainName + "admin/calendar/venues/default.aspx?id=" + this.ID;
            }
        }

        public string RSVPAttend
        {
            get
            {
                return Constants.DomainName + "admin/calendar/rsvp/default.aspx?id=" + this.ID + "&attend=true";
            }
        }

        public string RSVPNotAttend
        {
            get
            {
                return Constants.DomainName + "admin/calendar/rsvp/default.aspx?id=" + this.ID + "&attend=false";
            }
        }

        public string OutputDate
        {
            get
            {
                return this.StartDateTime.ToString(Constants.DefaultDateStringFormat) + " - " + this.EndDateTime.ToString("HH:mm");
            }
        }

        public string SlotAdjustmentMargin
        {
            get
            {
                double adjustment = 0;

                adjustment = SlotHeightpxPerMinute * this.StartDateTime.Minute;

                return adjustment.ToString();
            }
        }

        public string SlotHeight
        {
            get
            {
                double minuteheight = (SlotHeightpxPerMinute * (this.EndDateTime - this.StartDateTime).TotalMinutes);
                double borders = (this.EndDateTime - this.StartDateTime).TotalHours;

                return (minuteheight + borders).ToString();
            }
        }

        public object ShallowCopy()
        {
            return (CRM_Calendar)this.MemberwiseClone();
        }

        public string OutputTableName
        {
            get
            {
                return "Calendar Record";
            }
        }

        public string TableName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        public int ParentID
        {
            get
            {
                return 0;
            }
        }

        public bool IsInvolvedWith(int adminID, int CurrentAdminID)
        {
            return (this.CreatedByAdminID == adminID || this.CRM_CalendarAdmins.Any(c => c.AdminID == adminID)) && (
                ((this.PrivacyStatus == (byte)CRM_Calendar.PrivacyTypes.Private && this.CreatedByAdminID == CurrentAdminID)) || this.PrivacyStatus != (byte)CRM_Calendar.PrivacyTypes.Private);
        }

        public enum PrivacyTypes
        {   
            [StringValue("Editable to all")]
            Editable,
            [StringValue("Viewable without Editing")]
            Viewable,
            [StringValue("Private - Creator Only")]
            Private
        }

        public enum StatusTypes
        {
            [StringValue("Provisional")]
            Provisional,
            [StringValue("Confirmed")]
            Confirmed
        }

        [IsListData("Start Date")]
        public string StartDateOutput
        {
            get
            {
                return this.StartDateTime.ToString(Constants.DefaultDateStringFormat);
            }
        }

        public long StartDateOutput_T
        {
            get
            {
                return this.StartDateTime.Ticks;
            }
        }

        [IsListData("End Date")]
        public string EndDateOutput
        {
            get
            {
                return this.EndDateTime.ToString(Constants.DefaultDateStringFormat);
            }
        }

        public long EndDateOutput_T
        {
            get
            {
                return this.EndDateTime.Ticks;
            }
        }


        public string DetailsURL
        {
            get
            {
                return "/admin/calendar/details.aspx?id=" + this.ID;
            }
        }


        [IsListData("Is Invoiced")]
        public string IsInvoicedOutput
        {
            get
            {
                return this.IsInvoiced ? "Yes" : "No";
            }
        }

        [IsListData("Is Cancelled")]
        public string IsCancelledOutput
        {
            get
            {
                return this.IsCancelled ? "Yes" : "No";
            }
        }


        [IsListData("Type")]
        public string TypeOutput
        {
            get
            {
                return this.CRM_CalendarType.Name;
            }
        }


        [IsListData("Invoice Address")]
        public string InvoiceAddressOutput
        {
            get
            {
                return this.CRM_Address == null ? "None set" : this.CRM_Address.FormattedAddress;
            }
        }

        [IsListData("View")]
        public string View
        {
            get
            {
                return Utils.Text.Text.ConvertUrlsToLinks(DetailsURL, "View");
            }
        }

        public const string DefaultTargetReference = "";

        public void DeleteFromDatabase(MainDataContext db, Models.Admin AdminUser)
        {
            foreach (CRM_CalendarVenue venue in this.CRM_CalendarVenues)
            {
                venue.DeleteFromDatabase(db, AdminUser);
            }

            foreach (CRM_CalendarInvite invite in this.CRM_CalendarInvites)
            {
                db.CRM_CalendarInvites.DeleteOnSubmit(invite);
            }

            foreach (CRM_CalendarAdmin admin in this.CRM_CalendarAdmins)
            {
                db.CRM_CalendarAdmins.DeleteOnSubmit(admin);
            }

            foreach (CRM_CalendarAttendance admin in this.CRM_CalendarAttendances)
            {
                db.CRM_CalendarAttendances.DeleteOnSubmit(admin);
            }

            db.CRM_CalendarCPDs.DeleteAllOnSubmit(this.CRM_CalendarCPDs);
            db.CRM_CalendarGroupBookings.DeleteAllOnSubmit(this.CRM_CalendarGroupBookings);
            db.CRM_CalendarOutreaches.DeleteAllOnSubmit(this.CRM_CalendarOutreaches);
            db.CRM_CalendarParties.DeleteAllOnSubmit(this.CRM_CalendarParties);
            db.CRM_CalendarPerHeads.DeleteAllOnSubmit(this.CRM_CalendarPerHeads);
            db.CRM_Tasks.DeleteAllOnSubmit(this.CRM_Tasks);
            db.CRM_CalendarAdmins.DeleteAllOnSubmit(this.CRM_CalendarAdmins);


            History.History.RecordLinqDelete(AdminUser, this);
            db.CRM_Calendars.DeleteOnSubmit(this);
            db.SubmitChanges();
        }

        public const string DayVisit_FixedRef = "Reception - Day Visit";

        public string NextStageURL
        {
            get
            {
                switch (this.CRM_CalendarType.FixedRef)
                {
                    case (int)CRM_CalendarType.TypeList.task:
                        return "/admin/calendar/task/details.aspx?id=" + this.ID;
                    case (int)CRM_CalendarType.TypeList.groupbooking:
                    case (int)CRM_CalendarType.TypeList.schoolvisit:
                        return "/admin/calendar/groupbookings/details.aspx?id=" + this.ID;
                    case (int)CRM_CalendarType.TypeList.party:
                        return "/admin/calendar/parties/details.aspx?id=" + this.ID;
                    case (int)CRM_CalendarType.TypeList.cpd:
                        return "/admin/calendar/cpd/details.aspx?id=" + this.ID;
                    case (int)CRM_CalendarType.TypeList.outreach:
                        return "/admin/calendar/outreach/details.aspx?id=" + this.ID;   
                    default:
                        return "/admin/calendar";
                }
            }
        }
    }
}