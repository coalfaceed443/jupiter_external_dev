using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;

namespace CRM.admin.Fundraising.Gifts
{
    public partial class List : AdminPage<CRM_FundraisingGiftProfileLog>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucList.Type = typeof(CRM_FundraisingGiftProfileLog);

            BaseSet = from p in db.CRM_FundraisingGiftProfileLogs
                      orderby p.TimestampCreated descending
                      select p;

            ucList = CheckQuery(ucList);
            ucList.ItemsPerPage = 10;      
        }
    }
}