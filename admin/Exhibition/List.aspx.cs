using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;

using CRM.Code.Helpers;

namespace CRM.admin.Exhibition
{
    public partial class List : AdminPage<CRM_Exhibition>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucList.Type = typeof(CRM_Exhibition);
            BaseSet = CRM_Exhibition.BaseSet(db);
            ucList.ItemsPerPage = 50;     
            CheckQuery(ucList);   
        }

    }
}