using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;

namespace CRM.Code.Models
{
    public partial class CRM_CalendarGroupBooking : IHistory, INotes
    {
        public object ShallowCopy()
        {
            return (CRM_CalendarGroupBooking)this.MemberwiseClone();
        }

        public string OutputTableName
        {
            get
            {
                return "Group Booking Record";
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

        [IsListData("Exhibition")]
        public string Exhibition
        {
            get
            {
                return this.CRM_Exhibition == null ? "Not set" : this.CRM_Exhibition.Name;
            }
        }

    }
}