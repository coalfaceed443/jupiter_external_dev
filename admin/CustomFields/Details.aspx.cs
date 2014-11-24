using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Managers;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;
using CRM.Code.Utils.Enumeration;
using CRM.Code.Utils.Ordering;
namespace CRM.admin.CustomFields
{
    public partial class Details : AdminPage
    {
        protected CRM_FormField FormField;
        protected void Page_Load(object sender, EventArgs e)
        {

            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);
            FormField = db.CRM_FormFields.SingleOrDefault(f => f.ID.ToString() == Request.QueryString["id"]);
            navCustomField.Entity = FormField;
            // buttons //

            btnSubmit.EventHandler = btnSubmit_Click;
            btnSubmitChanges.EventHandler = btnSubmitChanges_Click;
            btnDelete.EventHandler = btnDelete_Click;

            // Security //

            btnSubmitChanges.Visible = PermissionManager.CanUpdate;
            btnDelete.Visible = PermissionManager.CanDelete;
            if (!PermissionManager.CanAdd && FormField == null)
                Response.Redirect("list.aspx");

            // confirmations //

            confirmationDelete.StandardDeleteHidden("item", btnRealDelete_Click);


            // process //

            if (!IsPostBack)
            {

                ddlTable.DataSource = CRM_FormField.DataTableBaseSet(db);
                ddlTable.DataBind();

                ddlType.DataSource = Enumeration.GetAll<CRM_FormField.Types>();
                ddlType.DataBind();

                if (FormField != null)
                {
                    PopulateFields();
                    lnkBack.HRef = "list.aspx?id=" + ddlTable.SelectedValue;
                }
                else
                {
                    ddlTable.SelectedValue = Request.QueryString["sid"];
                }
            }
        }

        private void PopulateFields()
        {
            ddlTable.SelectedValue = FormField._DataTableID.ToString();
            txtName.Text = FormField.Name;
            ddlType.SelectedValue = FormField.Type.ToString();
            chkIsRequired.Checked = FormField.IsRequired;
            chkIsActive.Checked = FormField.IsActive;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(true);

                NoticeManager.SetMessage("Field Added", "details.aspx?id=" + FormField.ID);
            }
        }

        protected void btnSubmitChanges_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(false);

                NoticeManager.SetMessage("Question Updated");
            }
        }

        protected void SaveRecord(bool newRecord)
        {
            // new record / exiting record //
            if (newRecord)
            {
                FormField = new CRM_FormField();
                FormField.OrderNo = Ordering.GetNextOrderID(db.CRM_FormFields);
                db.CRM_FormFields.InsertOnSubmit(FormField);
            }

            // common //
            FormField.IsRequired = chkIsRequired.Checked;
            FormField.Name = txtName.Text;
            FormField.Type = byte.Parse(ddlType.SelectedValue);
            FormField.IsActive = chkIsActive.Checked;
            FormField._DataTableID = Convert.ToInt32(ddlTable.SelectedValue);
            db.SubmitChanges();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            confirmationDelete.ShowPopup();
        }

        protected void btnRealDelete_Click(object sender, EventArgs e)
        {
            FormField.IsArchived = true;
            db.SubmitChanges();

            NoticeManager.SetMessage("Form Field Deleted", "list.aspx");
        }
    }

}