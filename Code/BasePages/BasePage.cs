using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;

namespace CRM.Code.BasePages
{
    /// <summary>
    /// Summary description for BasePage
    /// </summary>
    public class BasePage : System.Web.UI.Page
    {
        public MainDataContext db = new MainDataContext();

        public BasePage()
        {

        }
    }
}