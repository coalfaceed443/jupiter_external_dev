using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;

using CRM.Code.Helpers;

namespace CRM.admin.School.Region
{
    public partial class List : AdminPage<CRM_Region>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucList.Type = typeof(CRM_Region);
            BaseSet = CRM_Region.BaseSet(db);
            CheckQuery(ucList);
            ucList.ItemsPerPage = 10;        
        }

    }
}