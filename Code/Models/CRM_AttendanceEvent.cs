using CRM.Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Code.Models
{
    public partial class CRM_AttendanceEvent : ICRMRecord
    {

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
                return "/admin/attendance/attendanceevent/details.aspx?id=" + this.ID;
            }
        }

        public string OutputTableName
        {
            get { return this.Name; }
        }
    }
}