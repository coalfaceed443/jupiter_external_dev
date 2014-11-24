using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Code.Utils.Ajax
{

    /// <summary>
    /// Ajax methods callable from AjaxHandler.ashx
    /// </summary>
    public class AjaxMethods
    {
        public string Echo(string data)
        {
            return "Echo: " + data;
        }    
    }
}
