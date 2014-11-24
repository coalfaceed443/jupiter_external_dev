using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Interfaces;

namespace CRM.Controls.Admin.SharedObjects.Panel
{
    public partial class CommsLog : System.Web.UI.UserControl
    {
        public IContact IContact { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {


            this.Visible = IContact != null;
        }
    }
}