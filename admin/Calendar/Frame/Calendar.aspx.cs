using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Managers;
using CRM.Code.Calendar;
using CRM.Code.Utils.Time;
using CRM.Code.Models;

namespace CRM.admin.Calendar.Frame
{
    public partial class Calendar : CRM.Code.BasePages.Admin.AdminPage
    {
        protected DateTime CurrentDate;
        protected CRM_Venue CRM_Venue;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<CalendarItem> Calendar = new List<CalendarItem>();

            DateTime time = UKTime.Now.Date;

            if (!String.IsNullOrEmpty(Request.QueryString["date"]))
            {
                Session["calendardate"] = Request.QueryString["date"];
                time = CRM.Code.Utils.Text.Text.FormatInputDate(Request.QueryString["date"]);
            }
            else if (Session["calendardate"] != null)
            {
                time = CRM.Code.Utils.Text.Text.FormatInputDate((string)Session["calendardate"]);
            }

            if (Request.QueryString["venue"] != null)
            {
                CRM_Venue = db.CRM_Venues.SingleOrDefault(v => v.ID.ToString() == Request.QueryString["venue"]);
            }

            CurrentDate = time;

            CalendarManager calendarManager = new CalendarManager();

            bool HideExternal = Query("hideexternal");
            bool HideInternal = Query("hideinternal");
            bool HideUntagged = Query("hideNonTagged");
            
            int tryAdmin = 0;
            Int32.TryParse(Request.QueryString["privacy"], out tryAdmin);

            int? calendarAdmin = null;

            if (tryAdmin != 0)
            {
                calendarAdmin = tryAdmin;
            }

            int type = Convert.ToInt32(Request.QueryString["type"]);

            int count = 0;
            while (time.Day < (CurrentDate.Day + 1))
            {
                count++;
                IEnumerable<CalendarSlot> slots = calendarManager.FetchSlotsForTime(time, HideExternal, HideInternal, HideUntagged, type, calendarAdmin);

                Calendar.Add(new CalendarItem()
                {
                    Hour = time,
                    Slots = slots
                });

                time = time.AddHours(1);

                if (count > 25)
                    break;
            };

            rptCalendar.DataSource = Calendar;
            rptCalendar.DataBind();

        }

        private bool Query(string key)
        {
            return Request.QueryString[key] == "true";
        }

        protected void rptCalendar_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            CalendarItem item = (CalendarItem)e.Item.DataItem;
            Repeater rptSlots = (Repeater)e.Item.FindControl("rptSlots");
            rptSlots.DataSource = item.Slots;
            rptSlots.DataBind();
        }

        protected void rptSlots_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Panel pnlExisting = (Panel)e.Item.FindControl("pnlExisting");
            Panel pnlAdd = (Panel)e.Item.FindControl("pnlAdd");
            CalendarSlot slot = (CalendarSlot)e.Item.DataItem;
            pnlExisting.Visible = slot.CalendarItem != null;


            if (CRM_Venue != null && slot.CalendarItem != null && !slot.CalendarItem.CRM_CalendarVenues.Any(v => v.CRM_VenueID == CRM_Venue.ID))
            {
                pnlExisting.Visible = false;
            }

            Repeater rptVenues = (Repeater)e.Item.FindControl("rptVenues");

            if (slot.CalendarItem != null)
            {
                if (CRM_Venue != null)
                {
                    rptVenues.DataSource = slot.CalendarItem.CRM_CalendarVenues.Where(v => v.CRM_VenueID == CRM_Venue.ID).OrderBy(v => v.DateTimeFrom);
                    rptVenues.DataBind();               
            
                }
                else
                {
                    rptVenues.DataSource = slot.CalendarItem.CRM_CalendarVenues.OrderBy(v => v.DateTimeFrom);
                    rptVenues.DataBind();
                }
            }

            pnlAdd.Visible = slot.Visible;
            

        }
    }
}