using CRM.Code.BasePages.Admin;
using CRM.Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM.admin.Attendance.AttendanceEvent
{
    public partial class List : AdminPage<CRM_AttendanceEvent>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);

            ucList.Type = typeof(CRM_AttendanceEvent);
            BaseSet = db.CRM_AttendanceEvents;
            CheckQuery(ucList);
            ucList.ItemsPerPage = 10;
            ucList.CanOrder = false;
        }
    }
}