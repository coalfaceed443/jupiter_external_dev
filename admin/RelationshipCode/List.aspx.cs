using CRM.Code.BasePages.Admin;
using CRM.Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM.admin.RelationshipCode
{
    public partial class List : AdminPage<CRM_RelationCode>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucList.Type = typeof(CRM_RelationCode);
            BaseSet = CRM_RelationCode.BaseSet(db);
            CheckQuery(ucList);
            ucList.ItemsPerPage = 10;
        }
    }
}