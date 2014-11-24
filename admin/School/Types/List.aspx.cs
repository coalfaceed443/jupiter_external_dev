using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;

using CRM.Code.Helpers;

namespace CRM.admin.School.Types
{
    public partial class List : AdminPage<CRM_SchoolType>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucList.Type = typeof(CRM_SchoolType);
            BaseSet = CRM_SchoolType.BaseSet(db);
            CheckQuery(ucList);
            ucList.ItemsPerPage = 10;
            ucList.CanOrder = true;
        }
    }
}