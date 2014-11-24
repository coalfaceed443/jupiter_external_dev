using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Models;
using CRM.Code.BasePages.Admin;

namespace CRM.admin.Calendar
{
    public partial class List : AdminPage<CRM_Calendar>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucList.Type = typeof(CRM_Calendar);
            BaseSet = CRM_Calendar.BaseSet(db).OrderByDescending(o => o.StartDateTime);
            CheckQuery(ucList);
            ucList.ItemsPerPage = 10;
        }
    }
}