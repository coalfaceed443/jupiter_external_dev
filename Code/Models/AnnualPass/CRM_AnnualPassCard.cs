using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Utils.Time;
using CRM.Code.Helpers;
using CRM.Code.Interfaces;

namespace CRM.Code.Models
{
    public partial class CRM_AnnualPassCard : ICRMContext, IHistory
    {
        public string TableName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        public object ShallowCopy()
        {
            return (CRM_AnnualPassCard)this.MemberwiseClone();
        }

        private int _parentID = 0;
        public int ParentID
        {
            get
            {
                return _parentID;
            }
            set
            {
                _parentID = value;
            }
        }

        public string DisplayName
        {
            get
            {
                return this.MembershipNumber.ToString();
            }
        }

        [IsListData("Years Active")]
        public int YearsActive
        {
            get
            {
                return Convert.ToInt32(Math.Round((NewestMembership - OldestMembership).TotalDays / 365, 0));
            }
        }

        public Int64 NextExpiryDate_T
        {
            get
            {
                return NextExpiryDate.Ticks;
            }
        }

        [IsListData("Next Expiry Date")]
        public DateTime NextExpiryDate
        {
            get
            {
                if (this.CRM_AnnualPasses.Any(c => c.ExpiryDate >= DateTime.Now))
                {
                    return this.CRM_AnnualPasses.OrderBy(d => d.ExpiryDate).First(c => c.ExpiryDate >= DateTime.Now).ExpiryDate;
                }
                else
                    return UKTime.Now;
            }
        }

        [IsListData("Oldest Membership")]
        public DateTime OldestMembership
        {
            get
            {
                if (this.CRM_AnnualPasses.Any())
                {
                    return this.CRM_AnnualPasses.OrderBy(d => d.StartDate).First().StartDate;
                }
                else
                    return DateTime.MinValue;
            }
        }

        [IsListData("Newest Membership")]
        public DateTime NewestMembership
        {
            get
            {
                if (this.CRM_AnnualPasses.Any())
                {
                    return this.CRM_AnnualPasses.OrderByDescending(d => d.StartDate).First().StartDate;
                }
                else
                    return UKTime.Now;
            }
        }

        [IsListData("Number of People on Card")]
        public int PersonsActiveOnAccount
        {
            get
            {
                return this.CRM_AnnualPasses.Where(c => c.IsCurrent).Sum(s => s.PeopleOnPass);
            }
        }

        [IsListData("Types of Pass")]
        public string PassTypes
        {
            get
            {
                return JSONSet.FlattenList(this.CRM_AnnualPasses.Select(h => h.CRM_AnnualPassType.Name).ToList());
            }
        }

        [IsListData("View")]
        public string DetailsURLOutput
        {
            get
            {
                return Utils.Text.Text.ConvertUrlsToLinks(DetailsURL, "View");
            }
        }

        public string DetailsURL
        {
            get
            {
                return "/admin/annualpasscard/annualpass/list.aspx?id=" + this.ID;
            }
        }

    }
}