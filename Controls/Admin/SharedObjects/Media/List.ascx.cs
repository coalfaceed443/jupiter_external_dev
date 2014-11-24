using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Models;
using CRM.Code.Utils.Ordering;
using CRM.Code.Managers;

namespace CRM.Controls.Admin.SharedObjects.Media
{
    public partial class List : System.Web.UI.UserControl
    {
        public string Reference { get; set; }
        public bool ShowCategories { get; set; }
        public const string PAGE = "MediaPage";
        protected MainDataContext db;

        protected void Page_Load(object sender, EventArgs e)
        {
            db = MainDataContext.CurrentContext;

            if (Reference == null)
                NoticeManager.SetMessage("The media list control has not been set", "/admin");

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

            var items = LoadData();

            lvItems.DataSource = items;
            lvItems.DataBind();

            dpMain.Visible = items.Any();
        }

        protected void lvItems_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            dpMain.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);

            Session[PAGE] = e.StartRowIndex;

            LoadList();
        }

        private IEnumerable<CRM.Code.Models.Media> LoadData()
        {
            var items = db.Medias.Where(m => m.Reference == Reference);

            /*
            if (ShowCategories)
            {
                items = items.Where(i => i.MediaCategoryID.ToString() == ddlCategories.SelectedValue);
            }
            */

            return items.OrderBy(m => m.OrderNo);

        }

        protected void btnMoveUp_Click(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            reorderItem(button, false);
        }

        protected void btnMoveDown_Click(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            reorderItem(button, true);
        }

        private void reorderItem(ImageButton button, bool increase)
        {
            var entities = LoadData();

            CRM.Code.Models.Media media = db.Medias.SingleOrDefault(a => a.ID == Int32.Parse(button.CommandArgument));
            Ordering.ChangeOrder(entities, media, increase);
            db.SubmitChanges();

            LoadList();
        }
        protected void ddlCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadList();
        }
    }
}