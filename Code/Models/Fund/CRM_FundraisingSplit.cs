using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;

namespace CRM.Code.Models
{
    public partial class CRM_FundraisingSplit : IHistory, ICRMRecord
    {
        public static IEnumerable<CRM_FundraisingSplit> BaseSet(MainDataContext db)
        {
            return db.CRM_FundraisingSplits.OrderByDescending(c => c.ID);
        }

        public object ShallowCopy()
        {
            return (CRM_FundraisingSplit)this.MemberwiseClone();
        }

        public string DisplayName
        {
            get
            {
                return OutputTableName + " : " + this.ID;
            }
        }

        public string OutputTableName
        {
            get
            {
                return "Donation Split";
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
                return "/admin/fundraising/details.aspx?id=" + this.ID;
            }
        }
    
    }
}