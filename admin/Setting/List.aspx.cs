using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Linq.Dynamic;
using CRM.Code.Managers;
using CRM.Controls.Forms;
using CRM.Code.Models;

namespace CRM.Admin.Setting
{
    public partial class List : CRM.Code.BasePages.Admin.AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSubmit.EventHandler = SubmitChanges;
            if (!Page.IsPostBack)
            {
                LoadList();
            }
        }

        private void LoadList()
        {
            var entities = from p in db.Settings
                           orderby p.Name
                           select p;

            if (entities.Any())
            {
                repItems.DataSource = entities;
                repItems.DataBind();
            }
            else
                litNoSettings.Visible = true;
        }

        protected void SubmitChanges(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                foreach (RepeaterItem item in repItems.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        UserControlTextBox txtValue = (UserControlTextBox)item.FindControl("txtValue");
                        CRM.Code.Models.Setting.SetSetting(txtValue.Attributes["SettingName"], txtValue.Text);
                    }
                }

                NoticeManager.SetMessage("Settings Updated");
            }
        }
    }
}
