using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Models;
using CRM.Code.BasePages.Admin;

namespace CRM.admin.Fundraising.Reasons
{
    public partial class List : AdminPage<CRM_FundraisingReason>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucList.Type = typeof(CRM_FundraisingReason);
            BaseSet = CRM_FundraisingReason.BaseSet(db);
            ucList = CheckQuery(ucList);
            ucList.ItemsPerPage = 10;        
            ucList.CanOrder = true;
        }
    }
}