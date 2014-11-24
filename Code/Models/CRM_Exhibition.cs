using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Abstracts;
using CRM.Code.Interfaces;
using System.Web.UI.WebControls;

namespace CRM.Code.Models
{
    public partial class CRM_Exhibition : absArchivable, IArchivable, IHistory, ICRMContext
    {
        public static IEnumerable<CRM_Exhibition> BaseSet(MainDataContext db)
        {
            return db.CRM_Exhibitions.Where(e => e.IsActive && !e.IsArchived).OrderBy(e => e.Name);
        }

        public ListItem ListItem
        {
            get
            {
                return new ListItem(this.Name, this.ID.ToString());
            }
        }

        public object ShallowCopy()
        {
            return (CRM_Note)this.MemberwiseClone();
        }

        public string DisplayName
        {
            get
            {
                return OutputTableName + " : " + this.Name;
            }
        }

        public string OutputTableName
        {
            get
            {
                return "Exhibition";
            }
        }

        public int ParentID
        {
            get
            {
                return 0;
            }
        }

        public string TableName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        [IsListData("View")]
        public string ViewRecord
        {
            get
            {
                return "<a href=\"" + DetailsURL + "\">View</a>";
            }
        }

        public string DetailsURL
        {
            get
            {
                return "/admin/exhibition/details.aspx?id=" + this.ID;
            }
        }


        [IsListData("Group bookings assigned")]
        public int GroupBookingsAssigned
        {
            get
            {
                return this.CRM_CalendarGroupBookings.Count();
            }
        }

        [IsListData("Is Active")]
        public string IsActiveOutput
        {
            get
            {
                return this.IsActive ? "Yes" : "No";
            }
        }
    }
}