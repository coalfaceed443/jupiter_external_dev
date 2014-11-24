using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;

namespace CRM.admin.Communications
{
    public partial class List : AdminPage<CRM_Communication>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucList.Type = typeof(CRM_Communication);
            BaseSet = CRM_Communication.BaseSet(db);
            ucList.ItemsPerPage = 50;
            CheckQuery(ucList);   
        }
    }
}