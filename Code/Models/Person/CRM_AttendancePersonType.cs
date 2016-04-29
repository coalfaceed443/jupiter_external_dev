using CRM.Code.Interfaces;
using CRM.Code.Utils.Ordering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Code.Models
{
    public partial class CRM_AttendancePersonType : ICRMRecord, ListOrderItem
    {

        public string OutputTableName
        {
            get { return this.Name; }
        }

        [IsListData("View")]
        public string ViewRecord
        {
            get
            {
                return "<a href=\"" + DetailsURL + "\">View</a>";
            }
        }


        [IsListData("IsArchived")]
        public string ViewIsArchived
        {
            get
            {
                return this.IsArchived ? "Yes" : "No";
            }
        }


        [IsListData("IsActive")]
        public string ViewIsActive
        {
            get
            {
                return this.IsActive ? "Yes" : "No";
            }
        }

        public string DetailsURL
        {
            get
            {
                return "/admin/attendance/attendancetypes/details.aspx?id=" + this.ID;
            }
        }
    }
}