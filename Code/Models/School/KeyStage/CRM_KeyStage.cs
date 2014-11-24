using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;
using System.Web.UI.WebControls;
using CRM.Code.Abstracts;

namespace CRM.Code.Models
{
    public partial class CRM_KeyStage : absArchivable, IArchivable, IHistory, ICRMRecord, Utils.Ordering.ListOrderItem, ICRMContext
    {
        public static IEnumerable<CRM_KeyStage> BaseSet(MainDataContext db)
        {
            return db.CRM_KeyStages.Where(c => !c.IsArchived).OrderBy(o => o.OrderNo);
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
            return (CRM_KeyStage)this.MemberwiseClone();
        }

        public string OutputTableName
        {
            get
            {
                return "Key Stage";
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

        public ListItem ListItem
        {
            get
            {
                return new ListItem(this.Name, this.ID.ToString());
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


        public string DetailsURL
        {
            get
            {
                return "/admin/school/keystages/details.aspx?id=" + this.ID;
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