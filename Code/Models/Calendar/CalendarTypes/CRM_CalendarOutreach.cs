using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;

namespace CRM.Code.Models
{
    public partial class CRM_CalendarOutreach : IHistory, INotes
    {
        public object ShallowCopy()
        {
            return (CRM_CalendarGroupBooking)this.MemberwiseClone();
        }

        public string OutputTableName
        {
            get
            {
                return "Outreach Booking Record";
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
                return this.CRM_CalendarID;
            }
        }

        public string DisplayName
        {
            get
            {
                return OutputTableName + " : " + this.OrganisationName;
            }
        }

        [IsListData("Offer")]
        public string Offer
        {
            get
            {
                return this.CRM_Offer == null ? "Not set" : this.CRM_Offer.Name;
            }
        }
    }
}