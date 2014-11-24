using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Managers;
using CRM.Code.Utils;
using CRM.Code.Helpers;
using CRM.Code.Models;
namespace CRM.admin.Calendar.Frame
{
    public partial class Rota : System.Web.UI.Page
    {
        protected RotaManager RotaManager;
        protected void Page_Load(object sender, EventArgs e)
        {
            RotaManager = new RotaManager(Convert.ToDateTime(Request.QueryString["date"]));
            rptUsersHeading.DataSource = RotaManager.Admins.OrderByDescending(r => r.CRM_CalendarAdmins.Count());
            rptUsersHeading.DataBind();
            rptCalendar.DataSource = RotaManager.RotaContainers;
            rptCalendar.DataBind();
        }

        protected void rptCalendar_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RotaContainer container = (RotaContainer)e.Item.DataItem;

            Repeater rptUsers = (Repeater)e.Item.FindControl("rptUsers");
            rptUsers.DataSource = container.AdminLists;
            rptUsers.DataBind();
        }

        protected void rptUsers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RotaContainer.AdminList list = (RotaContainer.AdminList)e.Item.DataItem;
            Repeater rptEntry = (Repeater)e.Item.FindControl("rptEntry");
            rptEntry.DataSource = list.CRM_CalendarAdmins.OrderByDescending(r => r.Admin.CRM_CalendarAdmins.Count());
            rptEntry.DataBind();
        }

        protected void rptEntry_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            CRM_CalendarAdmin admin = (CRM_CalendarAdmin)e.Item.DataItem;
            Repeater rptVenues = (Repeater)e.Item.FindControl("rptVenues");
            rptVenues.DataSource = admin.CRM_Calendar.CRM_CalendarVenues.OrderBy(o => o.DateTimeFrom);
            rptVenues.DataBind();

        }

    }
}