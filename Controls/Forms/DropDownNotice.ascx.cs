using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Managers;

namespace CRM.Controls.Forms
{
    public partial class DropDownNotice : System.Web.UI.UserControl
    {
        public string Message { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Message = NoticeManager.GetMessage();
        }
    }
}