using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;
using CRM.Code.Abstracts;
using System.Web.UI.WebControls;

namespace CRM.Code.Models
{
    public partial class CRM_SchoolType : absArchivable, IArchivable, IHistory, ICRMRecord, ICRMContext, Utils.Ordering.ListOrderItem
    {
        public static IEnumerable<CRM_SchoolType> BaseSet(MainDataContext db)
        {
            return db.CRM_SchoolTypes.OrderBy(o => o.OrderNo);
        }

        public ListItem ListItem
        {
            get
            {
                return new ListItem(this.Name, this.ID.ToString());
            }
        }

        public string DisplayName
        {
            get
            {
                return OutputTableName + " : " + this.Name;
            }
        }

        public object ShallowCopy()
        {
            return (CRM_SchoolType)this.MemberwiseClone();
        }

        public string OutputTableName
        {
            get
            {
                return "School Type";
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
                return "/admin/school/types/details.aspx?id=" + this.ID;
            }
        }

        public string TableName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        public int ParentID
        {
            get
            {
                return 0;
            }
        }
    }
}