using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;

namespace CRM.Code.Models
{
    public partial class CRM_Fund : IHistory, ICRMRecord, Utils.Ordering.ListOrderItem, INotes, ICRMContext
    {
        public static IEnumerable<CRM_Fund> BaseSet(MainDataContext db)
        {
            return db.CRM_Funds.Where(c => !c.IsArchived).OrderBy(c => c.OrderNo);
        }

        public object ShallowCopy()
        {
            return (CRM_Fund)this.MemberwiseClone();
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
                return "/admin/fundraising/funds/details.aspx?id=" + this.ID;
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

        [IsListData("Total Value")]       
        public string TotalValueOutput
        {
            get
            {
                return "£" + this.TotalValue.ToString("N2");
            }
        }

        public decimal TotalValue
        {
            get
            {
                return this.CRM_FundraisingSplits.Sum(s => s.Amount);
            }
        }


        [IsListData("Last Submission")]
        public string LastSubmission
        {
            get
            {
                CRM_FundraisingSplit split = this.CRM_FundraisingSplits.OrderByDescending(f => f.DateGiven).FirstOrDefault();

                return split == null ? "Never" : split.DateGiven.ToString(Constants.DefaultDateStringFormat);
            }
        }
    }
}