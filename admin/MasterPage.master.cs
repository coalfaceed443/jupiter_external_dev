using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using CRM.Code.Models;
using CRM.Code.Utils.Json;
using CRM.Code.BasePages.Admin;
using CRM.Code.Helpers.Admin;
using CRM.Code.Utils.Menu;
using CRM.Code;
using System.IO;
using System.Text.RegularExpressions;

namespace CRM.Admin
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected List<AdminMenuListItem> menuItems = new List<AdminMenuListItem>();
        protected string searchJSON;

        protected List<SearchItem> SearchItems = new List<SearchItem>();

        public class AdminMenuListItem : MenuListItem
        {
            public View view;

            public AdminMenuListItem(string url, string name, View view)
                : base(url, name)
            {
                this.view = view;
            }

            public AdminMenuListItem(string url, string name)
                : base(url, name)
            {

            }
            public AdminMenuListItem(string url, string name, List<MenuListItem> children)
                : base(url, name, children)
            {
            }

            public AdminMenuListItem(string url, string name, MenuListItem[] subMenu, List<MenuListItem> children)
                : base(url, name, children)
            {
            }
        }

        protected string submenuID = "submenu-off";

        protected void Page_Load(object sender, EventArgs e)
        {

            this.Page.Title = ((AdminPage)Page).GetPageTitle();

            // menu //

            menuItems.Add(new AdminMenuListItem("/admin/calendar/default.aspx", "Calendar", new List<MenuListItem>()
            {
                new MenuListItem("/admin/calendar/list.aspx", "Report"),
                new MenuListItem("/admin/calendar/task/list.aspx", "Tasks"),
                new MenuListItem("/admin/calendar/clashes/list.aspx", "Clashes"),
            }));


            menuItems.Add(new AdminMenuListItem("/admin/annualpasscard/list.aspx", "Passes", new List<MenuListItem>()
                {
                    new MenuListItem("/admin/annualpasscard/overview.aspx", "Overview")
                }));


            menuItems.Add(new AdminMenuListItem("/admin/person/list.aspx", "Persons", new List<MenuListItem>()
                {
                    new MenuListItem("/admin/merge/list.aspx", "Merge Tool"),
                    new MenuListItem("/admin/families/list.aspx", "Families")
                }));

            menuItems.Add(new AdminMenuListItem("/admin/organisation/list.aspx", "Organisations", new List<MenuListItem>()
                {
                    new MenuListItem("/admin/organisation/persons/fulllist.aspx", "View People"),       
                }));

            menuItems.Add(new AdminMenuListItem("/admin/school/list.aspx", "Schools", new List<MenuListItem>()
                {
                    new MenuListItem("/admin/school/person/list.aspx", "View People"),             
                }));


            menuItems.Add(new AdminMenuListItem("/admin/fundraising/list.aspx", "Fundraising", new List<MenuListItem>()
                {
                    new MenuListItem("/admin/fundraising/gifts/list.aspx", "Fundraising Records"),      

                }));

            menuItems.Add(new AdminMenuListItem("#", "Dropdowns", new List<MenuListItem>()
                {
                    new MenuListItem("/admin/annualpasscard/types/list.aspx", "Manage Annual Pass Card Types"),
                    new MenuListItem("/admin/exhibition/list.aspx", "Manage Exhibitions"),
                    new MenuListItem("/admin/fundraising/funds/list.aspx", "Manage Fundraising Funds"),
                    new MenuListItem("/admin/fundraising/paymenttype/list.aspx", "Manage Fundraising Payment Types"),
                    new MenuListItem("/admin/fundraising/reasons/list.aspx", "Manage Fundraising Reasons for giving"),
                    new MenuListItem("/admin/school/keystages/list.aspx", "Manage Key Stages"),
                    new AdminMenuListItem("/admin/offer/list.aspx", "Manage Offers"),
                    new MenuListItem("/admin/organisation/types/list.aspx", "Manage Organisation Types"),
                    new AdminMenuListItem("/admin/package/list.aspx", "Manage Packages"),
                    new MenuListItem("/admin/person/titles/list.aspx", "Manage Person Titles"),
                    new MenuListItem("/admin/role/list.aspx", "Manage Roles"),
                    new MenuListItem("/admin/school/lea/list.aspx", "Manage School LAs"),
                    new MenuListItem("/admin/school/region/list.aspx", "Manage School Regions"),
                    new MenuListItem("/admin/school/types/list.aspx", "Manage School Types"),

                }));

            menuItems.Add(new AdminMenuListItem("/admin/customfields/list.aspx", "Custom Fields"));
            menuItems.Add(new AdminMenuListItem("/admin/communications/list.aspx", "Communications", new List<MenuListItem>()
                {
                    new MenuListItem("/admin/communications/emails/list.aspx", "Automated Emails")
                }));

            foreach (var item in menuItems)
            {
                if (item.IsCurrentArea)
                {
                    if (item.view != null)
                    {
                        mvSubmenu.SetActiveView(item.view);
                        submenuID = "submenu";

                        foreach (var control in item.view.Controls)
                        {
                            if (control.GetType().Name == "HyperLink")
                            {
                                HyperLink link = (HyperLink)control;

                                if (link.NavigateUrl == Request.RawUrl)
                                {
                                    link.CssClass = "selected";
                                }
                            }
                        }

                        break;
                    }
                }
            }
            LoadSearchItems();
        }

        protected void LoadSearchItems()
        {
            if (Cache["SearchCache"] == null)
            {
                MainDataContext db = ((AdminPage)Page).DataContext;

                searchJSON = new JSON(
                             from p in SearchItems
                             orderby p.Name
                             select new
                             {
                                 name = p.Name,
                                 url = p.Url
                             }).JSONValue;

                Cache.Add("SearchCache", searchJSON, null, DateTime.UtcNow.AddHours(6),
                    TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Default, null);
            }
            else
            {
                searchJSON = Cache["SearchCache"].ToString();
            }
        }


    }

    public static class Split
    {
        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }
    }
}