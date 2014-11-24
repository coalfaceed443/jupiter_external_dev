using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;
using CRM.Code.Managers;
using System.Web.UI.WebControls;

namespace CRM.Code.Models
{
    public partial class  CRM_CalendarCPD : IHistory, INotes
    {
        public object ShallowCopy()
        {
            return (CRM_CalendarCPD)this.MemberwiseClone();
        }

        public string OutputTableName
        {
            get
            {
                return "CPD Booking Record";
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
                return OutputTableName + " : " + new ContactManager().Contacts.First(c => c.Reference == this.CRM_Calendar.PrimaryContactReference).Fullname;
            }
        }

        public static List<ListItem> GetDurationLengths()
        {
            return new List<ListItem>()
            {
                new ListItem("Half day", "0"),
                new ListItem("Full day", "1")                
            };
        }

        public string OutputDateConfirmed
        {
            get
            {
                return this.ConfirmationSent == null ? "Not sent" : ((DateTime)this.ConfirmationSent).ToString(Constants.DefaultDateStringFormat);
            }
        }

    }
}