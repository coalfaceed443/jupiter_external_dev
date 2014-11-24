using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.Linq.Expressions;
using System.IO;
using System.Collections.Generic;
using CRM.Code.BasePages.Admin;

namespace CRM.Admin
{
    public partial class NoPermission : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            litFromURL.Text = Request.QueryString["url"];
        }
    }
}