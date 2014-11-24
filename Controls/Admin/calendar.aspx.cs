using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Utils;
using CRM.Code.Models;
using CRM.Code.Utils.Time;

namespace CRM.Controls.Admin
{
    public partial class Calendar : System.Web.UI.Page
    {
        protected class DataContext
        {
            public List<DateTime> days { get; set; }
            public DateTime month { get; set; }
            public Dictionary<int, bool> hasEvents = new Dictionary<int, bool>();
        }

        protected DataContext dataContext = new DataContext();

        protected void Page_Load(object sender, EventArgs e)
        {

            using (MainDataContext db = new MainDataContext())
            {


                DateTime month = DateTime.Today;


                if (Session["calmonth"] == null)
                {
                    Session["calmonth"] = month;
                }
                else
                {
                    month = (DateTime)Session["calmonth"];
                }

                if (Request.QueryString["action"] != null)
                {
                    if (Request.QueryString["action"] == "next")
                    {
                        if (month < UKTime.Now.AddYears(2))
                        {
                            month = month.AddMonths(1);
                        }

                    }
                    else
                    {
                        month = month.AddMonths(-1);                        
                    }
                    Session["calmonth"] = month;
                }


                CRM.Code.Calendar.Calendar calendar = new CRM.Code.Calendar.Calendar(db);

                dataContext.days = calendar.GetDaysInMonth(month.Month, month.Year);
                dataContext.month = month;


                int daysinmonth = DateTime.DaysInMonth(
                   dataContext.month.Year,
                   dataContext.month.Month);


                for (int i = 1; i <= daysinmonth; i++)
                {
                    DateTime day = new DateTime(
                        dataContext.month.Year,
                        dataContext.month.Month,
                        i);

                    if (String.IsNullOrEmpty(Request.QueryString["venue"]))
                    {
                        dataContext.hasEvents[i] =
                            db.CRM_Calendars.Any(c => c.StartDateTime.Date == day.Date);
                    }
                    else
                    {

                        dataContext.hasEvents[i] =
                            db.CRM_Calendars.Any(c => c.StartDateTime.Date == day.Date && c.CRM_CalendarVenues.Any(v => v.CRM_VenueID.ToString() == Request.QueryString["venue"]));
                    }
                }


                


                db.Dispose();
            }
        }

        protected string randomQuery()
        {
            string random = "";

            Random ran = new Random();
            random = ran.Next(1000).ToString();
            return random;
        }
    }
}