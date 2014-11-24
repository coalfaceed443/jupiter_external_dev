using System;
using System.Web.UI;
using System.Linq;
using System.IO;
using System.Web.UI.WebControls;
using System.Threading;
using CodeCarvings.Piczard;
using System.Collections.Generic;
using CRM.Code.BasePages.Admin;
using CRM.Code.Utils.Enumeration;
using CRM.Code.Managers;
using CRM.Code.Models;
using CRM.Code;
using CRM.Code.Auth;

namespace CRM.Admin.AdminUser
{
    public partial class Details : AdminPage
    {
        protected CRM.Code.Models.Admin Entity;

        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.AdminUsers);

            Entity = AdminUser;
            CRMContext = Entity;
            // buttons //

            btnSubmit.EventHandler = btnSubmit_Click;
            btnSubmitChanges.EventHandler = btnSubmitChanges_Click;

            // Security //

            btnSubmitChanges.Visible = PermissionManager.CanUpdate;
            if (Entity == null)
                Response.Redirect("/admin/adminuser/list.aspx");

            // confirmations //

            confirmationDelete.StandardDeleteHidden("item", btnRealDelete_Click);

            // process //

            if (!IsPostBack)
            {
                if (Entity != null)
                {
                    PopulateFields();
                }
            }
            if (Entity == null)
            {
                txtPassword.Required = true;
            }
        }

        private void PopulateFields()
        {
            txtUsername.Text = Entity.Username;
            txtFirstName.Text = Entity.FirstName;
            txtSurname.Text = Entity.Surname;
            txtEmail.Text = Entity.Email;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(true);

                NoticeManager.SetMessage("Admin Added", "details.aspx?id=" + Entity.ID);
            }
        }

        protected void btnSubmitChanges_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(false);

                NoticeManager.SetMessage("Your details have been updated");
            }
        }

        protected void SaveRecord(bool newRecord)
        {
            // new record / exiting record //

            if (newRecord)
            {
                Entity = new CRM.Code.Models.Admin();
                db.Admins.InsertOnSubmit(Entity);
            }

            Entity.Username = txtUsername.Text;
            Entity.Email = txtEmail.Text;
            Entity.FirstName = txtFirstName.Text;
            Entity.Surname = txtSurname.Text;

            if (!String.IsNullOrEmpty(txtPassword.Text))
            {
                Entity.Password = Auth.GetHashedString(txtPassword.Text);
            }

          
            db.SubmitChanges();
        }

        protected void cusValUsername_Validate(object sender, ServerValidateEventArgs e)
        {
            string username = txtUsername.Text.ToLower();
            if (Entity == null)
            {
                e.IsValid = !db.Admins.Any(a => a.Username.ToLower() == username);
            }
            else
            {
                e.IsValid = !db.Admins.Any(a => a.Username.ToLower() == username && a.ID != Entity.ID);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            confirmationDelete.ShowPopup();
        }

        protected void btnRealDelete_Click(object sender, EventArgs e)
        {
            db.Admins.DeleteOnSubmit(Entity);

            db.SubmitChanges();

            NoticeManager.SetMessage("Admin Deleted", "list.aspx");
        }

    
    }
}
