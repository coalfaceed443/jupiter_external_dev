using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;
using CRM.Code.Abstracts;
using System.Web.UI.WebControls;

namespace CRM.Code.Models
{
    public partial class CRM_FundraisingReason : absArchivable, IArchivable, IHistory, ICRMRecord, Utils.Ordering.ListOrderItem, ICRMContext
    {
        public static IEnumerable<CRM_FundraisingReason> BaseSet(MainDataContext db)
        {
            return db.CRM_FundraisingReasons.Where(c => !c.IsArchived).OrderBy(c => c.OrderNo);
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
            return (CRM_FundraisingReason)this.MemberwiseClone();
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
                return this.CRM_Fundraisings.SelectMany(f => f.CRM_FundraisingSplits).Sum(s => s.Amount);
            }
        }


        [IsListData("Last Submission")]
        public string LastSubmission
        {
            get
            {
                CRM_FundraisingSplit split = this.CRM_Fundraisings.SelectMany(f => f.CRM_FundraisingSplits).OrderByDescending(f => f.DateGiven).FirstOrDefault();

                return split == null ? "Never" : split.DateGiven.ToString(Constants.DefaultDateStringFormat);
            }
        }
    }
}