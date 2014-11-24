using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Reflection;

namespace CRM.Code.WebControl
{
    public class UserControlBase : System.Web.UI.UserControl
    {
        public string BaseValue
        {
            get;
            set;
        }
    }
}