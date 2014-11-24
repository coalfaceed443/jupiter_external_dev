using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;
using CRM.Code.Managers;
using CRM.Code.Utils.Time;



namespace CRM.admin.Emails
{
    static class LinqExtensions
    {
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> list, int parts)
        {
            int i = 0;
            var splits = from item in list
                         group item by i++ % parts into part
                         select part.AsEnumerable();
            return splits;
        }
    }

    public partial class Details : AdminPage
    {
        protected TemplateEmail Entity;
        protected void Page_Load(object sender, EventArgs e)
        {
            // buttons //

            Entity = db.TemplateEmails.SingleOrDefault(s => s.ID.ToString() == Request.QueryString["id"]);
            btnSubmitChanges.EventHandler = btnSubmitChanges_Click;

            // Security //

            //If the page is the layout page then load the layout from there rather than pass it in.

            // confirmations //

            // process //
            
            if (!IsPostBack)
            {
                if (Entity != null)
                {
                    PopulateFields();
                }
            }
        }



        private void PopulateFields()
        {
            txtTitle.Text = Entity.Name;
            txtSubject.Text = Entity.Subject;
            txtFrom.Text = Entity.FromEmail;
            txtToEmail.ShowText = !Entity.IsToEnabled;
            txtToEmail.Text = Entity.ToEmail;
            txtCCEmail.Text = Entity.CCEmail;
            txtBCCEmail.Text = Entity.BCCEmail;
            txtBody.Text = Entity.Body;
            chkIsEnabled.Checked = !Entity.IsDisabled;

            var placeholders = Entity.TemplateEmailPlaceholders.Split<TemplateEmailPlaceholder>(2);

            rptLeft.DataSource = placeholders.Take(1).First().OrderBy(c => c.Description);
            rptLeft.DataBind();

            rptRight.DataSource = placeholders.Skip(1).FirstOrDefault().OrderBy(c => c.Description);
            rptRight.DataBind();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(true);

                NoticeManager.SetMessage("Item Added", "/admin/emails/details.aspx?id=" + Entity.ID);
            }
        }

        protected void btnSubmitChanges_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(false);

                if (!String.IsNullOrEmpty(txtTestEmail.Text))
                {
                    EmailManager manager = new EmailManager();
                    
                    if (Entity.FixedRef == TemplateEmail.TemplateEmails.RenewalEmail)
                    {
                        manager.SendRenewalEmail(db.CRM_AnnualPasses.First(c => c.ExpiryDate >= UKTime.Now), db, txtTestEmail.Text);                     
                    }

                    NoticeManager.SetMessage("Item Updated and test sent to " + txtTestEmail.Text);
                }
                else
                {

                    NoticeManager.SetMessage("Item Updated");
                }

            }
        }

        protected void SaveRecord(bool newRecord)
        {
            Entity.Name = txtTitle.Text;
            Entity.Subject = txtSubject.Text;
            Entity.FromEmail = txtFrom.Text;
            Entity.ToEmail = txtToEmail.Text;
            Entity.CCEmail = txtCCEmail.Text;
            Entity.BCCEmail = txtBCCEmail.Text;
            Entity.Body = txtBody.Text;
            Entity.IsDisabled = !chkIsEnabled.Checked;
            db.SubmitChanges();

        }

    }
}