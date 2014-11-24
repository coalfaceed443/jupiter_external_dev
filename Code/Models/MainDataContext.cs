using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;
using System.Web.UI;
using CRM.Code.BasePages;

namespace CRM.Code.Models
{
    /// <summary>
    /// Summary description for MainDataContext
    /// </summary>
    public partial class MainDataContext
    {
        [Function(Name = "NEWID", IsComposable = true)]
        public Guid Random()
        { // to prove not used by our C# code... 
            throw new NotImplementedException();
        }

        private static MainDataContext _DataContext;
        public static MainDataContext CurrentContext
        {
            get
            {
                Page page = HttpContext.Current.Handler as Page;

                if (page != null)
                {
                    if (page is BasePage)
                    {
                        _DataContext = ((BasePage)page).db;
                    }
                }
                if (_DataContext == null)
                    _DataContext = new MainDataContext();

                return _DataContext;
            }
        }
    }
}