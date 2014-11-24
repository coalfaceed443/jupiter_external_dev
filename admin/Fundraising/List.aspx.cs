using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;

namespace CRM.admin.Fundraising
{
    public partial class List : AdminPage<CRM_Fundraising>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucList.Type = typeof(CRM_Fundraising);

            BaseSet = CRM_Fundraising.BaseSet(db);
            ucList = CheckQuery(ucList);
            ucList.ItemsPerPage = 10;      
        }
    }
}