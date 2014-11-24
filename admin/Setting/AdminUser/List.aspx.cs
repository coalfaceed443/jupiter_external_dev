using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Linq.Dynamic;
using CRM.Code.BasePages.Admin;

namespace CRM.Admin.AdminUser
{
    public partial class List : AdminPage
    {
        private string PAGE = "adminPage";

        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.AdminUsers);

            if (!Page.IsPostBack)
            {
                LoadList();
            }
        }

        private void LoadList()
        {
            if (Session[PAGE] != null)
            {
                dpMain.SetPageProperties((int)Session[PAGE], dpMain.MaximumRows, false);
            }
            var entities = from p in db.Admins
                           orderby p.Username
                           select p;

            lvItems.DataSource = entities;
            lvItems.DataBind();

            dpMain.Visible = entities.Any();
        }

        protected void lvItems_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            dpMain.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);

            Session[PAGE] = e.StartRowIndex;

            LoadList();
        }
    }
}