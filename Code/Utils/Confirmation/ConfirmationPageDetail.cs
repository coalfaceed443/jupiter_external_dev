using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace CRM.Code.Utils.Confirmation
{
    public class ConfirmationPageDetail
    {
        public string Message { get; set; }

        public List<UserControl> buttons = new List<UserControl>();
    }
}