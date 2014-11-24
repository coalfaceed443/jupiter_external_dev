using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;
using CRM.Controls.Forms.Handlers;

namespace CRM.Code.Models
{
    public partial class CRM_Note: IHistory
    {

        public object ShallowCopy()
        {
            return (CRM_Note)this.MemberwiseClone();
        }

        [IsListData("View")]
        public string ViewRecord
        {
            get
            {
                return "<a href=\"" + DetailsURL + "\">View</a>";
            }
        }


        [IsListData("Toggle Read Status")]
        public string ToggleReadStatus
        {
            get
            {
                return Utils.Text.Text.ConvertUrlsToLinks(ToggleReadStatusURL, "Loading read status") + "<input type=\"hidden\" class=\"ReadStatus\" value=\"" + this.ID + "\">";
            }
        }

        public string ToggleReadStatusURL
        {
            get
            {
                return ActionLink.FormURL(ActionLink.Route.ToggleReadStatus, this.ID, this.ListURL);
            }
        }

        public string ListURL
        {
            get
            {
                return "/admin/notes/frames/list.aspx?references=" + HttpContext.Current.Request.QueryString["references"];
            }
        }


        public string DetailsURL
        {
            get
            {
                return "/admin/notes/frames/details.aspx?id=" + this.ID + "&references=" + HttpContext.Current.Request.QueryString["references"];
            }
        }


        public string DisplayName
        {
            get
            {
                return OutputTableName + " : " + this.Title;
            }
        }

        public string OutputTableName
        {
            get
            {
                return "Note Record";
            }
        }

        public string TableName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        private int _parentID = 0;
        public int ParentID
        {
            get
            {
                return _parentID;
            }
            set
            {
                _parentID = value;
            }
        }


        public static IEnumerable<CRM_Note> BaseSet(MainDataContext db, List<string> References)
        {
            return from p in db.CRM_Notes
                   where References.Contains(p.TargetReference)
                   orderby p.DateCreated descending
                   select p;
        }

        [IsListData("Created")]
        public string DisplayTimestamp
        {
            get
            {
                return this.DateCreated.ToString("dd/MM/yyyy HH:mm");
            }

        }
    }
}