using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;
using CRM.Code.Managers;

namespace CRM.Code.BasePages.Admin.Calendar
{
    public class CRM_CalendarPage<T> : AdminPage<T>
    {
        public CRM_Calendar Entity { get; set; }

        public new void Page_PreInit(object sender, EventArgs e)
        {
            base.Page_PreInit(sender, e);

            Entity = db.CRM_Calendars.SingleOrDefault(a => a.ID.ToString() == Request.QueryString["id"]);

            if (Entity == null && !Request.RawUrl.StartsWith("/admin/calendar/details.aspx"))
                NoticeManager.SetMessage("Calendar entry not found", "/admin/calendar");
        }
    }
}