using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Managers;
using CRM.Code.Models;
using System.Web.UI.HtmlControls;
using CRM.Code.Utils.Time;

namespace CRM.admin.Calendar.Frame
{
    public partial class WeekView : CRM.Code.BasePages.Admin.AdminPage
    {
        public CRM_Venue CRM_Venue;

        protected DateTime CurrentDate;
        protected void Page_Load(object sender, EventArgs e)
        {

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

            DateTime startDate = time.StartOfWeek(DayOfWeek.Monday);
            CurrentDate = startDate;
            DateTime endDate = startDate.AddDays(7);
            
            List<DateTime> dates = new List<DateTime>();

            while (startDate < endDate)
            {
                dates.Add(startDate);
                startDate = startDate.AddDays(1);
            }

            if (Request.QueryString["venue"] != null)
            {
                CRM_Venue = db.CRM_Venues.SingleOrDefault(v => v.ID.ToString() == Request.QueryString["venue"]);
            }

            rptDays.DataSource = dates;
            rptDays.DataBind();
        }


        private bool Query(string key)
        {
            return Request.QueryString[key] == "true";
        }

        protected void rptDays_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DateTime date = (DateTime)e.Item.DataItem;

            CalendarManager manager = new CalendarManager();


            bool HideExternal = Query("hideexternal");
            bool HideInternal = Query("hideinternal");
            bool HideUntagged = Query("hideNonTagged");

            Repeater rptEvents = (Repeater)e.Item.FindControl("rptEvents");

            int type = Convert.ToInt32(Request.QueryString["type"]);
            int tryAdmin = 0;
            Int32.TryParse(Request.QueryString["privacy"], out tryAdmin);

            int? calendarAdmin = null;

            if (tryAdmin != 0)
            {
                calendarAdmin = tryAdmin;
            }

            rptEvents.DataSource = manager.EventsPerDay(date, HideExternal, HideInternal, HideUntagged, type, null).OrderBy(d => d.StartDateTime);
            rptEvents.DataBind();

        }

        protected void rptEvents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rptVenues = (Repeater)e.Item.FindControl("rptVenues");

            CRM_Calendar calendar = (CRM_Calendar)e.Item.DataItem;

            var venues = calendar.CRM_CalendarVenues.OrderBy(v => v.DateTimeFrom);;

            if (CRM_Venue != null)
            {
                venues = venues.Where(v => v.CRM_VenueID == CRM_Venue.ID).OrderBy(v => v.DateTimeFrom);
                rptVenues.DataSource = venues;
                rptVenues.DataBind();
                
            }
            else
            {
                rptVenues.DataSource = venues;
                rptVenues.DataBind();
            }

            HtmlImage imgVenues = (HtmlImage)e.Item.FindControl("imgVenues");
            imgVenues.Visible = venues.Any();
        }

    }
}