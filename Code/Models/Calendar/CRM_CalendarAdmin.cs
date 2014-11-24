using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Controls.Forms.Handlers;
using CRM.Code.BasePages.Admin;
using CRM.Code.Utils.Enumeration;
using System.Web.UI.WebControls;

namespace CRM.Code.Models
{
    public partial class CRM_CalendarAdmin
    {
        [IsListData("Admin Name")]
        public string AdminName
        {
            get
            {
                return this.Admin.DisplayName;
            }
        }

        public enum StatusTypes
        {
            [StringValue("Not Responded")]
            NotResponded,
            [StringValue("Attending")]
            Attending,
            [StringValue("Not Attending")]
            NotAttending,

        }

        public string StatusOutput
        {
            get
            {
                return Enumeration.GetStringValue<StatusTypes>(this.Status);
            }
        }

        public string EventName
        {
            get
            {
                return this.CRM_Calendar.DisplayName;
            }
        }

        public string EventDate
        {
            get
            {
                return this.CRM_Calendar.OutputDate;
            }
        }

        [IsListData("Date added to Calendar item")]
        public string DateAdded
        {
            get
            {
                return this.Timestamp.ToString("dd/MM/yyyy HH:mm");
            }
        }

        public bool CanRemove
        {
            get
            {
                return ((AdminPage)HttpContext.Current.Handler).AdminUser.IsMasterAdmin || this.AdminID == ((AdminPage)HttpContext.Current.Handler).AdminUser.ID;
            }
        }

        public ListItem ListItem
        {
            get
            {
                return new ListItem(this.AdminName, this.ID.ToString());
            }
        }

        [IsListData("Remove Admin From Item")]
        public string RemoveFromCalendarItemURL
        {
            get
            {
                if (!CanRemove)
                {
                    return Utils.Text.Text.ConvertUrlsToLinks(RemoveFromCalendarItem, "---");
                }
                else
                    return Utils.Text.Text.ConvertUrlsToLinks(RemoveFromCalendarItem, "Remove");
            }
        }

        private string RemoveFromCalendarItem
        {
            get
            {
                if (!CanRemove)
                    return "#";
                else
                    return ActionLink.FormURL(ActionLink.Route.RemoveAdminFromCalendarItem, this.ID, HttpContext.Current.Request.RawUrl);
            }
        }
    }
}