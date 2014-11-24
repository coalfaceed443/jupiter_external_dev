using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;
using CRM.Code.Helpers;
using CRM.Code.Managers;
using CRM.Code.Utils.Time;
using CRM.Code;

namespace CRM.admin.Notes.Frames
{
    public partial class Details : AdminPage
    {
        protected CRM.Code.Models.CRM_Note Entity = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Action = Request.RawUrl;
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);

            Entity = db.CRM_Notes.SingleOrDefault(c => c.ID.ToString() == Request.QueryString["id"]);
            

            // buttons //
            btnDelete.EventHandler = btnDelete_Click;
            btnSubmit.EventHandler = btnSubmit_Click;
            btnSubmitChanges.EventHandler = btnSubmitChanges_Click;

            // Security //

            if (!PermissionManager.CanAdd && Entity == null)
                Response.Redirect("list.aspx");

            // confirmations //

            confirmationDelete.StandardDeleteHidden("note", btnRealDelete_Click);

            // process //

            if (!IsPostBack)
            {

                if (Entity != null)
                {
                    if (AdminUser.ID != Entity.OwnerAdminID)
                    {
                        btnDelete.Visible = false;
                        btnSubmitChanges.Visible = false;
                    }

                    PopulateFields();
                }
                else
                {
                    lblOwner.Text = AdminUser.DisplayName;
                    lblCreated.Text = UKTime.Now.ToString(Constants.DefaultDateStringFormat);
                }
            }
        }

        private void PopulateFields()
        {
            txtTitle.Text = Entity.Title;
            txtBody.Text = Entity.Body;
            lblOwner.Text = db.Admins.Single(a => a.ID == Entity.OwnerAdminID).DisplayName;
            lblCreated.Text = Entity.DateCreated.ToString(Constants.DefaultDateStringFormat);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(true);

                NoticeManager.SetMessage("Note Added", "details.aspx?id=" + Entity.ID + "&"+ Request.QueryString);
            }
        }

        protected void btnSubmitChanges_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(false);

                NoticeManager.SetMessage("Note Updated");
            }
        }

        protected void SaveRecord(bool newRecord)
        {
            // new record / exiting record //

            object oldEntity = null;

            if (newRecord)
            {
                Entity = new CRM_Note();
                Entity.TargetReference = Request.QueryString["references"];
                Entity.OwnerAdminID = AdminUser.ID;
                Entity.DateCreated = UKTime.Now;
                db.CRM_Notes.InsertOnSubmit(Entity);
            }
            else
            {
                oldEntity = Entity.ShallowCopy();
            }

            Entity.Title = txtTitle.Text;
            Entity.Body = txtBody.Text;

            db.SubmitChanges();

            if (oldEntity != null)
            {
                CRM.Code.History.History.RecordLinqUpdate(AdminUser, oldEntity, Entity);
                db.SubmitChanges();
            }
            else
            {
                CRM.Code.History.History.RecordLinqInsert(AdminUser, Entity);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            confirmationDelete.ShowPopup();
        }


        protected void btnRealDelete_Click(object sender, EventArgs e)
        {
            db.SubmitChanges();

            NoticeManager.SetMessage("Note removed", "list.aspx");
        }


    }
}