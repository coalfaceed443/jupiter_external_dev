using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;

namespace CRM.Code.Calendar
{
    public class CalendarSlot
    {


        public CRM_Calendar CalendarItem { get; set; }
        public string NewDetailsURL { get; set; }
        public bool Visible { get; set; }
        public bool InsideFilter { get; set; }
        public string SlotHeight
        {
            get
            {
                if (this.CalendarItem != null)
                {
                    return this.CalendarItem.SlotHeight;
                }
                else
                    return String.Empty;        
            }
        }

        public CalendarSlot(string time)
        {
            NewDetailsURL = "/admin/calendar/details.aspx?slot=" + time;
            Visible = true;

        }

        public CalendarSlot(string time, bool visible)
        {
            NewDetailsURL = "/admin/calendar/details.aspx?slot=" + time;
            Visible = visible;
        }

        public CalendarSlot(string time, bool visible, bool insideFilter)
        {
            NewDetailsURL = "/admin/calendar/details.aspx?slot=" + time;
            Visible = visible;
            InsideFilter = insideFilter;
        }
    }
}