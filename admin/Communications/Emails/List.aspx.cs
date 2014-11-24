using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;

namespace CRM.admin.Emails
{
    public partial class List : AdminPage<TemplateEmail>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            ucList.Type = typeof(TemplateEmail);

            BaseSet = TemplateEmail.BaseSet(db);
            CheckQuery(ucList);
            ucList.CanOrder = false;
        }
    }
}