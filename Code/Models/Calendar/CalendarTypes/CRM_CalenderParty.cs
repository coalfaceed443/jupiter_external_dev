using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;
using CRM.Code.Managers;
using System.Web.UI.WebControls;

namespace CRM.Code.Models
{
    public partial class CRM_CalendarParty : IHistory, INotes
    {
        public object ShallowCopy()
        {
            return (CRM_CalendarGroupBooking)this.MemberwiseClone();
        }

        public string OutputTableName
        {
            get
            {
                return "Party Booking Record";
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

        public static List<ListItem> GetChildrensAgeList()
        {
            return new List<ListItem>()
            {
                new ListItem("1 year old","1"),
                new ListItem("2 years old", "2"),
                new ListItem("3 years old", "3"),
                new ListItem("4 years old", "4"),
                new ListItem("5 years old", "5"),
                new ListItem("6 years old", "6"),
                new ListItem("7 years old", "7"),
                new ListItem("8 years old", "8"),
                new ListItem("9 years old", "9"),
                new ListItem("10 years old", "10"),
                new ListItem("11 years old", "11"),
                new ListItem("12 years old", "12"),
                new ListItem("13 years old", "13"),
                new ListItem("14 years old", "14")
            };
        }

    }
}