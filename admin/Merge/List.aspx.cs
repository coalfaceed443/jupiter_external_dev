using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;

namespace CRM.admin.Merge
{
    public partial class List : AdminPage<HoldingPen>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            ucList.Type = typeof(HoldingPen);
            BaseSet = HoldingPen.BaseSet(db);
            ucList = CheckQuery(ucList);
            ucList.ItemsPerPage = 10;
            ucList.CanOrder = false;

        }
    }
}