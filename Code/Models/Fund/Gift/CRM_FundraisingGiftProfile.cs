using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;
using System.Web.UI.WebControls;

namespace CRM.Code.Models
{
    public partial class CRM_FundraisingGiftProfile : ICRMContext, IArchivable, IHistory, INotes
    {
        public static IEnumerable<CRM_FundraisingGiftProfile> BaseSet(MainDataContext db)
        {
            return from p in db.CRM_FundraisingGiftProfiles
                   where !p.IsArchived
                   select p;
        }

        public static IEnumerable<CRM_FundraisingGiftProfile> PublicBaseSet(MainDataContext db)
        {
            return from p in db.CRM_FundraisingGiftProfiles
                   where p.IsActive
                   where !p.IsArchived
                   select p;
        }

        public object ShallowCopy()
        {
            return (CRM_FundraisingGiftProfile)this.MemberwiseClone();
        }

        public string OutputTableName
        {
            get
            {
                return "Fundraising Gift Profile Record";
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
        public string DisplayName
        {
            get
            {
                return ArchivedOutput + OutputTableName + " : " + this.ProfileName;
            }
        }

        public string DetailsURL
        {
            get
            {
                return this.CRM_Person.GiftRecordNewURL + "&pid=" + this.ID;
            }
        }

        [IsListData("Details")]
        public string DetailsURLOutput
        {
            get
            {
                return Utils.Text.Text.ConvertUrlsToLinks(DetailsURL, "View");
            }
        }

        [IsListData("Next Payment Date")]
        public string NextPaymentDateOutput
        {
            get
            {
                return this.NextPaymentDate.ToString("ddd dd/MM/yyyy");
            }
        }

        public string ArchivedOutput
        {
            get
            {
                return this.IsArchived ? " [ARCHIVED] " : "";
            }
        }

        public ListItem ListItem
        {
            get
            {
                return new ListItem(this.ProfileName, this.ID.ToString());
            }
        }

        public bool IsDateActive
        {
            get
            {
                return this.StartDate <= this.NextPaymentDate && this.EndDate >= this.NextPaymentDate;
            }
        }
    }
}