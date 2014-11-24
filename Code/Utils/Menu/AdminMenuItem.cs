using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Code.Utils.Menu
{
    public class AdminMenuItem : MenuListItem
    {
        public MenuListItem[] subMenu;
        
        public AdminMenuItem(string url, string name)
            : base(url, name)
        {
        }

        public AdminMenuItem(string url, string name, List<MenuListItem> children)
            : base(url, name, children)
        {
        }

        public AdminMenuItem(string url, string name, MenuListItem[] subMenu, List<MenuListItem> children)
            : base(url, name, children)
        {
            this.subMenu = subMenu;
        }

        public AdminMenuItem(string url, string name, MenuListItem[] subMenu)
            : base(url, name)
        {
            this.subMenu = subMenu;
        }

        public AdminMenuItem(string url, string name, bool defaultMenu)
            : base(url, name)
        {
            string itemName = name;
            if (itemName.EndsWith("ies"))
                itemName = itemName.Substring(0, itemName.IndexOf("ies"));
            else if (itemName.EndsWith("es") && !itemName.EndsWith("les"))
                itemName = itemName.Substring(0, itemName.IndexOf("es"));
            else if (itemName.EndsWith("s"))
                itemName = itemName.TrimEnd('s');

            this.subMenu = new[] {
                new MenuListItem(url, name + " List"),
                new MenuListItem(url.Replace("default.aspx", "details.aspx"), "Add " + itemName),
            };
        }
    }
}