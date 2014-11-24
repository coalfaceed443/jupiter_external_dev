using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;
using CRM.Code.Managers;

namespace CRM.Code.Models
{
    public partial class CRM_Fundraising : IHistory, ICRMRecord, INotes, ICRMContext, IMailable
    {
        public static IEnumerable<CRM_Fundraising> BaseSet(MainDataContext db)
        {
            return db.CRM_Fundraisings.OrderBy(c => c.ID);
        }

        public object ShallowCopy()
        {
            return (CRM_Fundraising)this.MemberwiseClone();
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
                return "Donation";
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

        [IsListData("Total Value [Display]")]
        public string DisplayTotalValue
        {
            get
            {
                return "£" + this.CRM_FundraisingSplits.Sum(s => s.Amount).ToString("N2");
            }
        }

        [IsListData("Total Value [Monetary]")]
        public decimal TotalValue
        {
            get
            {
                return this.CRM_FundraisingSplits.Sum(s => s.Amount);
            }
        }

        [IsListData("Fund Split")]       
        public string FundSplit
        {
            get
            {
                return CRM.Code.Helpers.JSONSet.FlattenList(this.CRM_FundraisingSplits.Select(f => f.CRM_Fund.Name).ToList(),", ");
            }
        }


        public string DetailsURL
        {
            get
            {
                return "/admin/fundraising/details.aspx?id=" + this.ID;
            }
        }

        public IContact PrimaryContact()
        {

                return new ContactManager().GetIContactByReference(this.PrimaryContactReference);
            
        }

        public bool IsMailable
        {
            get
            {
                return PrimaryContact().IsMailable;
            }
        }

        public bool IsEmailable
        {
            get
            {
                return PrimaryContact().IsEmailable;
            }
        }

        [IsListData("Title")] 
        public string Title
        {
            get
            {
                return PrimaryContact().Title;
            }
        }

        [IsListData("Firstname")] 
        public string Firstname
        {
            get
            {
                return PrimaryContact().Firstname;
            }
        }

        [IsListData("Lastname")] 
        public string Lastname
        {
            get
            {
                return PrimaryContact().Lastname;
            }
        }


        [IsListData("Address 1")]
        public string Address1
        {
            get
            {
                return PrimaryContact().PrimaryAddress.AddressLine1;
            }
        }

        [IsListData("Address 2")]
        public string Address2
        {
            get
            {
                return PrimaryContact().PrimaryAddress.AddressLine2;
            }
        }

        [IsListData("Address 3")]
        public string Address3
        {
            get
            {
                return PrimaryContact().PrimaryAddress.AddressLine3;
            }
        }

        [IsListData("Town / City")]
        public string TownCity
        {
            get
            {
                return PrimaryContact().PrimaryAddress.Town;
            }
        }

        [IsListData("Postcode")]
        public string Postcode
        {
            get
            {
                return PrimaryContact().PrimaryAddress.Postcode;
            }
        }
    }
}