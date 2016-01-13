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
            return db.CRM_FundraisingSplits.Where(r => !r.CRM_Fundraising.IsArchived).OrderByDescending(c => c.ID);
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

        [IsListData("Address 1")]
        public string Address1
        {
            get
            {
                return this.CRM_Fundraising.Address1;
            }
        }

        [IsListData("Address 2")]
        public string Address2
        {
            get
            {
                return this.CRM_Fundraising.Address2;
            }
        }

        [IsListData("Address 3")]
        public string Address3
        {
            get
            {
                return this.CRM_Fundraising.Address3;
            }
        }

        [IsListData("Town / City")]
        public string TownCity
        {
            get
            {
                return this.CRM_Fundraising.TownCity;
            }
        }

        [IsListData("Postcode")]
        public string Postcode
        {
            get
            {
                return this.CRM_Fundraising.Postcode;
            }
        }


        [IsListData("Title")]
        public string Title
        {
            get
            {
                return this.CRM_Fundraising.Title;
            }
        }

        [IsListData("Firstname")]
        public string Firstname
        {
            get
            {
                return this.CRM_Fundraising.Firstname;
            }
        }

        [IsListData("Lastname")]
        public string Lastname
        {
            get
            {
                return this.CRM_Fundraising.Lastname;
            }
        }

        [IsListData("Gift aid Claimed")]
        public string GiftAidStatus
        {
            get
            {
                return this.CRM_Fundraising.GiftAidClaimedOutput;
            }
        }

        [IsListData("Gift aid Firstname")]
        public string GiftaidFirstname
        {
            get
            {
                return this.CRM_Fundraising.GiftAidFirstname;
            }
        }

        [IsListData("Gift aid Lastname")]
        public string GiftaidLastname
        {
            get
            {
                return this.CRM_Fundraising.GiftAidLastname;
            }
        }

        [IsListData("Gift aid Country")]
        public string Country
        {
            get
            {
                return this.CRM_Fundraising.CRM_Address.Country.Name;
            }
        }

        [IsListData("Is Gift aid")]
        public string IsGiftAid
        {
            get
            {
                return this.CRM_Fundraising.IsGiftAid ? "Yes" : "No";
            }
        }

        [IsListData("Donation ID")]
        public int DoantionID
        {
            get
            {
                return this.CRM_Fundraising.ID;
            }
        }

        [IsListData("Date Given")]
        public string DateGivenOutput
        {
            get
            {
                return this.DateGiven.ToString("dd/MM/yyyy");
            }
        }

        public long DateGivenOutput_T
        {
            get
            {
                return this.DateGiven.Ticks;
            }
        }

        [IsListData("Gift aid pence per pound")]
        public string PencePerPound
        {
            get
            {
                return this.GiftAidRate.ToString("N0");
            }
        }

        [IsListData("Total Gift Aid")]
        public string TotalGiftAidOutput
        {
            get
            {
                return ((this.GiftAidRate / 100) * this.Amount).ToString("N2");
            }
        }


        public decimal TotalGiftAid
        {
            get
            {
                return ((this.GiftAidRate / 100) * this.Amount);
            }
        }

        [IsListData("Fund")]
        public string Fund
        {
            get
            {
                return this.CRM_Fund.Name;
            }
        }

        [IsListData("Payment Type")]
        public string PaymentType
        {
            get
            {
                return this.CRM_Fundraising.CRM_PaymentType.Name;
            }
        }


        [IsListData("Recurring")]
        public string Recurring
        {
            get
            {
                return this.CRM_Fundraising.IsRecurring ? "Yes" : "No";
            }
        }

        [IsListData("Recurring weeks")]
        public int RecurringWeeks
        {
            get
            {
                return this.CRM_Fundraising.RecurringEveryWeeks;
            }
        }

        [IsListData("Duration in weeks")]
        public int DurationWeeks
        {
            get
            {
                return this.CRM_Fundraising.Duration;
            }
        }


        [IsListData("In Kind")]
        public string InKind
        {
            get
            {
                return this.CRM_Fundraising.IsInKind ? "Yes" : "No";
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
                return "/admin/fundraising/details.aspx?id=" + this.CRM_FundRaisingID;
            }
        }
    }
}