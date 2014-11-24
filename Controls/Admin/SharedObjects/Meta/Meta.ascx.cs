using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Models;
using CRM.Code.Managers;
using CRM.Code.BasePages.Admin;

namespace CRM.Controls.Admin.SharedObjects.Meta
{
    public partial class Meta : System.Web.UI.UserControl
    {
        public string Reference { get; set; }
        private CRM.Code.Models.Meta CurrentMeta;
        private MainDataContext db;

        protected void Page_Load(object sender, EventArgs e)
        {

            db = new MainDataContext();
            // buttons //

            btnContinue.EventHandler = Continue;

            // process //
            CurrentMeta = db.Metas.SingleOrDefault(p => p.Reference == Reference);

            PermissionManager PermissionManager = ((AdminPage)Page).PermissionManager;
            btnContinue.Visible = PermissionManager.CanUpdate;
            if (!PermissionManager.CanAdd && CurrentMeta == null)
                Response.Redirect("list.aspx");


            if (!Page.IsPostBack)
            {

                if (CurrentMeta != null)
                    PopulateFields();
            }
        }


        protected void Continue(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(CurrentMeta == null);
            }
        }

        private void SaveRecord(bool newRecord)
        {
            if (newRecord)
            {
                CurrentMeta = new CRM.Code.Models.Meta();
                CurrentMeta.Reference = Reference;
                db.Metas.InsertOnSubmit(CurrentMeta);
            }

            CurrentMeta.Title = txtTitle.Text;
            CurrentMeta.Keywords = txtKeywords.Text;
            CurrentMeta.Description = txtDescription.Text;

            db.SubmitChanges();

            NoticeManager.SetMessage("Meta Saved");
        }

        public void PopulateFields()
        {
            txtTitle.Text = CurrentMeta.Title;
            txtKeywords.Text = CurrentMeta.Keywords;
            txtDescription.Text = CurrentMeta.Description;
        }
    }
}