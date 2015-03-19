using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin.Persons;
using CRM.Code.Models;
using CRM.Code.Managers;
using CRM.Code.Utils.Ordering;

namespace CRM.admin.CustomFields.Answers
{
    public partial class Details : CRM_CustomFieldPage<CRM_FormFieldItem>
    {
        protected CRM_FormFieldItem CRM_FormFieldItem;
        protected void Page_Load(object sender, EventArgs e)
        {      
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);

            CRM_FormFieldItem = Entity.CRM_FormFieldItems.SingleOrDefault(f => f.ID.ToString() == Request.QueryString["fid"]);

            // buttons //

            navForm.Entity = Entity;

            btnSubmit.EventHandler = btnSubmit_Click;
            btnSubmitChanges.EventHandler = btnSubmitChanges_Click;
            btnDelete.EventHandler = btnDelete_Click;

            // Security //

            btnSubmitChanges.Visible = PermissionManager.CanUpdate;
            btnDelete.Visible = PermissionManager.CanDelete;
            if (!PermissionManager.CanAdd && Entity == null)
                Response.Redirect("list.aspx");

            // confirmations //

            confirmationDelete.StandardDeleteHidden("item", btnRealDelete_Click);

            // process //

            if (!IsPostBack)
            {
                if (CRM_FormFieldItem != null)
                {
                    PopulateFields();
                }
            }
        }

        private void PopulateFields()
        {
            txtName.Text = CRM_FormFieldItem.Label;
            chkIsActive.Checked = CRM_FormFieldItem.IsActive;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(true);

                NoticeManager.SetMessage("Answer Added", "details.aspx?id=" + Entity.ID);
            }
        }

        protected void btnSubmitChanges_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(false);

                NoticeManager.SetMessage("Answer Updated");
            }
        }

        protected void SaveRecord(bool newRecord)
        {
            // new record / exiting record //
            if (newRecord)
            {
                CRM_FormFieldItem = new CRM_FormFieldItem();
                CRM_FormFieldItem.CRM_FormFieldID = Entity.ID;
                CRM_FormFieldItem.OrderNo = Ordering.GetNextOrderID(db.CRM_FormFieldItems);
                db.CRM_FormFieldItems.InsertOnSubmit(CRM_FormFieldItem);
            }
            string oldName = CRM_FormFieldItem.Label;

            // common //
            CRM_FormFieldItem.Label = txtName.Text;
            CRM_FormFieldItem.IsActive = chkIsActive.Checked;
            db.SubmitChanges();

            string newName = txtName.Text;

            if (oldName != newName)
            {

                foreach (CRM_FormFieldAnswer answer in CRM_FormFieldItem.CRM_FormField.CRM_FormFieldAnswers)
                {
                    if (oldName != null && newName != null)
                    answer.Answer = answer.Answer.Replace(oldName, newName);
                }
            }

            db.SubmitChanges();

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            confirmationDelete.ShowPopup();
        }

        protected void btnRealDelete_Click(object sender, EventArgs e)
        {
            CRM_FormFieldItem.IsArchived = true;
            db.SubmitChanges();
            NoticeManager.SetMessage("Answer Deleted", "list.aspx?id=" + Entity.ID);
        }

    }
}