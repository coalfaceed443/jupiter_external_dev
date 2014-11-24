using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;
using System.Web.UI.WebControls;
using CRM.Code.Abstracts;

namespace CRM.Code.Models
{
    public partial class CRM_PaymentType : absArchivable, IArchivable, IHistory, ICRMRecord, Utils.Ordering.ListOrderItem, ICRMContext
    {
        public static IEnumerable<CRM_PaymentType> BaseSet(MainDataContext db)
        {
            return db.CRM_PaymentTypes.Where(c => !c.IsArchived).OrderBy(c => c.OrderNo);
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
            return (CRM_PaymentType)this.MemberwiseClone();
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
                return "Fund";
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
                return "/admin/fundraising/paymenttype/details.aspx?id=" + this.ID;
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

        [IsListData("Total Submissions")]
        public int TotalSubmissions
        {
            get
            {
                return this.CRM_Fundraisings.Count;
            }
        }


    }
}