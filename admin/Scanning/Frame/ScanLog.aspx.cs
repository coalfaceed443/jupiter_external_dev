using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;
using CRM.Code.Helpers;

namespace CRM.admin.Scanning.Frame
{
    public partial class ScanLog : AdminPage<CRM_CalendarAttendance>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lvLog.Type = typeof(CRM_CalendarInvite);
            lvLog.DataSet = db.CRM_CalendarInvites.OrderByDescending(c => c.ID).ToArray().Select(p => (object)p);
            lvLog.ItemsPerPage = 10;
            lvLog.ShowCustomisation = false;
            lvLog.ShowExport = false;
        }
    }
}