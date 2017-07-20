using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CRM.Code.Models;
using CRM.Code.Auth;
using CRM.Code.Utils.Time;
using CRM.Code;
using CRM.Code.Utils.Encryption;
using CRM.Code.Managers;

namespace CRM.Admin
{
    public partial class Login : System.Web.UI.Page
    {
        protected MainDataContext db = new MainDataContext();

        protected void Page_Load(object sender, EventArgs e)
        {


            if (!Request.IsSecureConnection)
            {
                string absoluteUri = Request.Url.AbsoluteUri;
                Response.Redirect(absoluteUri.Replace("http://", "https://"));
            }

            if (Request.QueryString["action"] == "logout")
            {
                AuthAdmin authAdmin = new AuthAdmin(db);
                authAdmin.Logout();
            }

            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["verify"]))
                {
                    CRM.Code.Models.Admin admin = db.Admins.FirstOrDefault(a => a.ResetLink.Contains(Request.RawUrl) && a.ResetLink != String.Empty);

                    if (admin != null)
                    {
                        if (((DateTime)admin.LastReset).AddMinutes(5) < UKTime.Now)
                        {
                            mvLogin.SetActiveView(viewLogin);
                        }

                        mvLogin.SetActiveView(viewReset);
                    }
                }
                lnkForgotten.Visible = true;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            AuthAdmin authAdmin = new AuthAdmin(db);

            if (authAdmin.Login(txtUsername.Text, txtPassword.Text))
            {
                if (Request.QueryString["redirect"] != null)
                {
                    Response.Redirect(Request.QueryString["redirect"]);
                }

                Response.Redirect("default.aspx");
            }
            else
            {
                lblMessage.Text = "Invalid Username or Password. Please try again.";
                lblMessage.Visible = true;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            CRM.Code.Models.Admin admin = db.Admins.First(a => a.ResetLink.Contains(Request.RawUrl));
            admin.Password = AuthAdmin.GetHashedString(txtNewPassword.Text);
            db.SubmitChanges();

            mvLogin.SetActiveView(viewDone);
        }

        protected void lnkForgotten_Click(object sender, EventArgs e)
        {
            mvLogin.SetActiveView(viewForgotten);
        }

        protected void btnForgotten_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                CRM.Code.Models.Admin admin = db.Admins.First(a => a.Username.ToLower().Trim() == txtEmailUsername.Text.ToLower().Trim() || a.Email.ToLower().Trim() == txtEmailUsername.Text.ToLower().Trim());
                admin.LastReset = UKTime.Now;
                litEmail.Text = admin.Email;
                admin.ResetLink = Constants.DomainName + "admin/login.aspx?verify=" + Guid.NewGuid();

                db.SubmitChanges();

                EmailManager manager = new EmailManager();
                manager.AddTo(admin.Email);
                manager.SendResetLink(admin);
                
                mvLogin.SetActiveView(viewSent);
            }
        }

        protected void cusNoLogin_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = db.Admins.Any(a => a.Username.ToLower().Trim() == txtEmailUsername.Text.ToLower().Trim() || a.Email.ToLower().Trim() == txtEmailUsername.Text.ToLower().Trim());
        }
    }
}
