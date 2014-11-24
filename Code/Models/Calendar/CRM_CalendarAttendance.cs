using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Code.Models
{
    public partial class CRM_CalendarAttendance
    {
        [IsListData("Scanned In By")]
        public string ScannedIn
        {
            get
            {
                return this.Admin.DisplayName;
            }
        }

        [IsListData("Event")]
        public string EventScannedFor    
        {
            get
            {
                return this.CRM_Calendar.DisplayName;
            }
        }

        [IsListData("Person")]
        public string Person
        {
            get
            {
                return this.CRM_Person.DisplayName;
            }
        }

        [IsListData("Photo")]
        public string Photo
        {
            get
            {
                return this.CRM_Person.PhotoPreviewOutput;
            }
        }

        [IsListData("Time")]
        public string OutputTime
        {
            get
            {
                return this.Timestamp.ToString(Constants.DefaultDateStringFormat);
            }
        }
    

    }
}