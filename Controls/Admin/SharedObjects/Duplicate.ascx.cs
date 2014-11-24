using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM.Controls.Admin.SharedObjects
{
    public partial class Duplicate : System.Web.UI.UserControl
    {
        public List<string> ClientIDs { get; set; }
        public object BaseObject { get; set; }
        protected string Namespace { get; set; }
        protected int _Countdown = 10;
        public int Countdown { get; set; }
        public int OriginalID { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            Namespace = BaseObject.GetType().FullName;

            if (Countdown != 0)
                _Countdown = Countdown;

        }
    }
}