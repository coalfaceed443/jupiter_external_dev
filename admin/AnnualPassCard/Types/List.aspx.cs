using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;

using CRM.Code.Helpers;

namespace CRM.admin.AnnualPassCard.AnnualPass.Types
{
    public partial class List : AdminPage<CRM_AnnualPassType>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucList.Type = typeof(CRM_AnnualPassType);
            BaseSet = CRM_AnnualPassType.BaseSet(db);
            ucList = CheckQuery(ucList);
            ucList.ItemsPerPage = 10;           

        }

    }
}