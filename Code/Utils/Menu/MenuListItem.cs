using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace CRM.Code.Utils.Menu
{
    public class MenuListItem
    {
        public string url;
        public string name;
        public View view;
        public List<MenuListItem> children;

        public MenuListItem(string url, string name)
        {
            this.url = url;
            this.name = name;
        }

        public MenuListItem(string url, string name, List<MenuListItem> children)
        {
            this.url = url;
            this.name = name;
            this.children = children;
        }

        private string GetUrlPart(string path)
        {
            var paths = path.Split('/');

            int partsCount = paths.Length - 1;

            if (partsCount > 3)
                partsCount = 3;

            return String.Join("/", paths.Take(partsCount).ToArray());
        }

        public bool HasChildren
        {
            get
            {
                return this.children != null && this.children.Any();
            }
        }

        public bool IsCurrentArea
        {
            get
            {
                return this.GetUrlPart(HttpContext.Current.Request.RawUrl).ToLower() == this.GetUrlPart(url).ToLower() || (this.children != null && this.children.Any(a => a.IsCurrentArea));
            }
        }

        public bool IsCurrent
        {
            get
            {
                return HttpContext.Current.Request.RawUrl.Split('?')[0] == url;
            }
        }

        public bool IsRoot
        {
            get
            {
                string firstpart = String.Join("/", HttpContext.Current.Request.RawUrl.Split('/').Take(2).ToArray());
                if (String.IsNullOrEmpty(firstpart))
                    firstpart = "/";

                if (firstpart.Contains("default.aspx"))
                    firstpart = "/";

                return url == firstpart;
            }
        }
    }
}