using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;

namespace CRM.Code.Models
{
    public partial class CRM_Package : IHistory, ICRMRecord, ICRMContext
    {
        public static IEnumerable<CRM_Package> BaseSet(MainDataContext db)
        {
            return db.CRM_Packages.OrderBy(c => c.Name);
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
                return "Role";
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
                return "/admin/package/details.aspx?id=" + this.ID;
            }
        }

        [IsListData("Events assigned")]
        public int EventsAssigned
        {
            get
            {
                return this.CRM_CalendarCPDs.Count();
            }
        }


        [IsListData("Active Status")]
        public string OutputActive
        {
            get
            {
                return this.IsActive ? "Yes" : "No";
            }
        }
    }
}