using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Managers;
using CRM.Code.Models;
using CRM.Code.Helpers;

namespace CRM.admin.Calendar.Clashes
{
    public partial class List : AdminPage<CRM_CalendarVenue>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            var venues = from v in db.CRM_Venues
                         select v;

            List<CRM_CalendarVenue> clashes = new List<CRM_CalendarVenue>(); 

            foreach (CRM_Venue venue in venues)
            {
                var overlaps = CalendarManager.GetOverlappedTimes<CRM_CalendarVenue>(venue.CRM_CalendarVenues,
                 ((CRM_CalendarVenue v) => v.DateTimeFrom >= DateTime.Now),
                 ((CRM_CalendarVenue vIn) => vIn.DateTimeFrom),
                 ((CRM_CalendarVenue vOut) => vOut.DateTimeTo));

                clashes.AddRange(overlaps.Select(o => (CRM_CalendarVenue)o.ItemOne));
                clashes.AddRange(overlaps.Select(o => (CRM_CalendarVenue)o.ItemTwo));

            }


            ucClash.Type = typeof(CRM_CalendarVenue);
            ucClash.DataSet = clashes.Distinct().OrderBy(d => d.ClashColourID).ThenBy(d => d.DateTimeFrom).Select(p => (object)p); ;
            ucClash.ItemsPerPage = 10;


        }
    }
}