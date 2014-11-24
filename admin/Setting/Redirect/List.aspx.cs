using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Linq.Dynamic;
using CRM.Code.Models;
using CRM.Code.Managers;
using CRM.Controls.Forms;

namespace CRM.Admin.Setting.Redirect
{
    public partial class List : CRM.Code.BasePages.Admin.AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadList();
            }
        }

        private void LoadList()
        {
            var entities = from p in db.Redirects
                           orderby p.ID descending
                           select p;
           
            repItems.DataSource = entities;
            repItems.DataBind();

            if (!entities.Any())
                litNoRedirects.Visible = true;
        }

        protected void lnkNew_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtCurrent_New.Text) || String.IsNullOrEmpty(txtRedirect_New.Text))
            {
                if (String.IsNullOrEmpty(txtCurrent_New.Text))
                    txtCurrent_New.addStyle("border:1px solid red;");

                if (String.IsNullOrEmpty(txtRedirect_New.Text))
                    txtRedirect_New.addStyle("border:1px solid red;");
            }
            else
            {
                CRM.Code.Models.Redirect redirect = new CRM.Code.Models.Redirect();
                db.Redirects.InsertOnSubmit(redirect);

                redirect.CurrentUrl = (txtCurrent_New.Text.StartsWith("/") || txtCurrent_New.Text.StartsWith("http")) ? txtCurrent_New.Text : "/" + txtCurrent_New.Text;
                redirect.RedirectUrl = (txtRedirect_New.Text.StartsWith("/") || txtRedirect_New.Text.StartsWith("http")) ? txtRedirect_New.Text : "/" + txtRedirect_New.Text;
                db.SubmitChanges();
                NoticeManager.SetMessage("404 Redirect Added");
            }
        }

        protected void lnkUpdate_Click(object sender, EventArgs e)
        {
            LinkButton lnk = (LinkButton)sender;
            CRM.Code.Models.Redirect redirect = db.Redirects.FirstOrDefault(p => p.ID.ToString() == lnk.CommandArgument);

            foreach (RepeaterItem item in repItems.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    LinkButton repLnk = (LinkButton)item.FindControl("lnkUpdate");

                    if (repLnk.CommandArgument == lnk.CommandArgument)
                    {
                        UserControlTextBox txtCurrent = (UserControlTextBox)item.FindControl("txtCurrent");
                        UserControlTextBox txtRedirect = (UserControlTextBox)item.FindControl("txtRedirect");

                        if (String.IsNullOrEmpty(txtCurrent.Text) || String.IsNullOrEmpty(txtRedirect.Text))
                        {
                            if (String.IsNullOrEmpty(txtCurrent.Text))
                                txtCurrent.addStyle("border:1px solid red;");

                            if (String.IsNullOrEmpty(txtRedirect.Text))
                                txtRedirect.addStyle("border:1px solid red;");

                            break;
                        }
                        else
                        {
                            redirect.CurrentUrl = (txtCurrent.Text.StartsWith("/") || txtCurrent.Text.StartsWith("http")) ? txtCurrent.Text : "/" + txtCurrent.Text;
                            redirect.RedirectUrl = (txtRedirect.Text.StartsWith("/") || txtRedirect.Text.StartsWith("http")) ? txtRedirect.Text : "/" + txtRedirect.Text;
                            db.SubmitChanges();
                            NoticeManager.SetMessage("404 Redirect Updated");
                        }
                    }
                }
            }
        }

        protected void lnkRemove_Click(object sender, EventArgs e)
        {
            LinkButton lnk = (LinkButton)sender;
            CRM.Code.Models.Redirect redirect = db.Redirects.FirstOrDefault(p => p.ID.ToString() == lnk.CommandArgument);
            db.Redirects.DeleteOnSubmit(redirect);
            db.SubmitChanges();
            NoticeManager.SetMessage("404 Redirect Removed");
        }
    }
}
