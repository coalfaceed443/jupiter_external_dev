using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Models;
using System.Web.UI.HtmlControls;
using CRM.Code.BasePages.Admin;

namespace CRM.Controls.Admin.Navigation
{
    public partial class CustomField : System.Web.UI.UserControl
    {
        public CRM.Code.Models.CRM_FormField Entity { get; set; }

        public string Section { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Entity != null)
            {
                System.Web.UI.Control control = divHolder.FindControl(Section);
                if (control != null)
                {
                    ((HtmlGenericControl)control).Attributes["class"] += " current";
                }
            }
            else
                this.Visible = false;
        }
    }
}