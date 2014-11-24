using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;
using CRM.Code.Calendar;
using System.Web.UI.HtmlControls;
using CRM.Code.Managers;
using CRM.Code.Utils.Time;

namespace CRM.admin.Calendar
{
    public partial class Calendar : AdminPage
    {
        protected DateTime CurrentDate;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                // set date to start but will be pulling from cookie after initial users load.
                hdnCalendarCurrentDate.Value = UKTime.Now.ToString("dd-MM-yyyy");


                rptKey.DataSource = from p in db.CRM_CalendarTypes
                                    orderby p.OrderNo
                                    select p;
                rptKey.DataBind();

                ddlVenueFilter.DataSource = from p in CRM_Venue.BaseSet(db)
                                            orderby p.IsInternal descending, p.Name
                                            select p.ListItem;
                ddlVenueFilter.DataBind();

                ddlPersonCalendar.DataSource = from p in db.Admins.ToArray()
                                               orderby p.FirstName, p.Surname
                                               select p;
                ddlPersonCalendar.DataBind();
            }

        }

        

    }



}