using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;

namespace CRM.Code.Models
{
    public partial class CRM_LEA : IHistory, ICRMRecord, ICRMContext
    {
        public static IEnumerable<CRM_LEA> BaseSet(MainDataContext db)
        {
            return db.CRM_LEAs.OrderBy(o => o.Name);
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
            return (CRM_LEA)this.MemberwiseClone();
        }

        public string OutputTableName
        {
            get
            {
                return "LEA";
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

        [IsListData("Website")]
        public string ViewWebsite
        {
            get
            {
                return "<a href=\"" + Website + "\" target=\"_blank\">" + Utils.Text.Text.TrimHTML(this.Website, 40) + "...</a>";
            }
        }


        public string DetailsURL
        {
            get
            {
                return "/admin/school/lea/details.aspx?id=" + this.ID;
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