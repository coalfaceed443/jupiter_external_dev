using CRM.Code.BasePages.Admin;
using CRM.Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM.admin.Fundraising
{
    public partial class Split : AdminPage<CRM_FundraisingSplit>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucList.Type = typeof(CRM_FundraisingSplit);

            BaseSet = CRM_FundraisingSplit.BaseSet(db);
            ucList = CheckQuery(ucList);
            ucList.ItemsPerPage = 10;
        }
    }
}