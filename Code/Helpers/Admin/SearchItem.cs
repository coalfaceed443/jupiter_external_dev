using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Code.Helpers.Admin
{
    /// <summary>
    /// Summary description for SearchItem
    /// </summary>
    public class SearchItem
    {
        public string Name;
        public string Url;
        public string ID;

        public SearchItem(string name, string url)
        {
            Name = name;
            Url = url;
        }
        public SearchItem(string name, string url, string id)
        {
            Name = name;
            Url = url;
            ID = id;
        }
    }
}