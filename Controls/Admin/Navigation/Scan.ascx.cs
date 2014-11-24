using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CRM.Code.Models;
namespace CRM.Controls.Admin.Navigation
{
    public partial class Scan : System.Web.UI.UserControl
    {
        public string Section { get; set; }
        public CRM_Person Entity { get; set; }
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
            else this.Visible = false;
        }
    }
}