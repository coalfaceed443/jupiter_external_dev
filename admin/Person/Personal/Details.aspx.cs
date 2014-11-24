using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Managers;
using CRM.Code.Models;
using CRM.Code.BasePages.Admin.Persons;

namespace CRM.admin.Person.Personal
{
    public partial class Details : CRM_PersonPage<CRM_Person>
    {
        protected CRM.Code.Models.CRM_PersonPersonal PersonPersonal = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);
            PersonPersonal = db.CRM_PersonPersonals.SingleOrDefault(c => c.ID.ToString() == Request.QueryString["pid"]);
            
            btnSubmitChanges.Visible = PermissionManager.CanUpdate;
            if (!PermissionManager.CanAdd && Entity == null)
                Response.Redirect("list.aspx");

            ucNavPerson.Entity = Entity;
            ucLogNotes.INotes = PersonPersonal;
            // buttons //

            btnSubmit.EventHandler = btnSubmit_Click;
            btnSubmitChanges.EventHandler = btnSubmitChanges_Click;
            btnDelete.EventHandler = btnDelete_Click;
            btnReinstate.EventHandler = btnReinstate_Click;
            // Security //

            btnSubmitChanges.Visible = PermissionManager.CanUpdate;
            btnDelete.Visible = PermissionManager.CanDelete;
            if (!PermissionManager.CanAdd && Entity == null)
                Response.Redirect("list.aspx");

            // confirmations //

            confirmationDelete.StandardDeleteHidden("personal record", btnRealDelete_Click);

            // process //
            CRMContext = PersonPersonal;
            if (!IsPostBack)
            {
                if (PersonPersonal != null)
                    PopulateFields();
            }
        }



        private void PopulateFields()
        {
            ucLogHistory.IHistory = PersonPersonal;
            ucLogHistory.ParentID = Entity.ID.ToString();
            txtDescription.Text = PersonPersonal.Description;
            txtTelephone.Text = PersonPersonal.Telephone;
            txtEmail.Text = PersonPersonal.Email;
            chkMakePrimary.Checked = PersonPersonal.IsPrimary;
            ucAddress.Populate(PersonPersonal.CRM_Address);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(true);

                NoticeManager.SetMessage("Item Added", "details.aspx?id=" + Entity.ID + "&pid=" + PersonPersonal.ID);
            }
        }

        protected void btnSubmitChanges_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(false);

                NoticeManager.SetMessage("Item Updated");
            }
        }

        protected void SaveRecord(bool newRecord)
        {
            // new record / exiting record //

            object oldEntity = null;
            object oldAddress = null;
            if (newRecord)
            {
                PersonPersonal = new CRM_PersonPersonal();
                PersonPersonal.CRM_Address = new CRM_Address();
                PersonPersonal.IsArchived = false;
                PersonPersonal.CRM_PersonID = Entity.ID;
                db.CRM_PersonPersonals.InsertOnSubmit(PersonPersonal);
            }
            else
            {
                oldEntity = PersonPersonal.ShallowCopy();
                oldAddress = PersonPersonal.CRM_Address.ShallowCopy();
            }

            PersonPersonal.Description = txtDescription.Text;
            PersonPersonal.Telephone = txtTelephone.Text;
            PersonPersonal.Email = txtEmail.Text;
            PersonPersonal.IsPrimary = chkMakePrimary.Checked;

            ucAddress.Save(PersonPersonal.CRM_Address);
            db.SubmitChanges();

            PersonPersonal.CRM_Address.ParentID = PersonPersonal.ID;
            if (oldEntity != null)
            {
                CRM.Code.History.History.RecordLinqUpdate(db, AdminUser, oldEntity, PersonPersonal);
                CRM.Code.History.History.RecordLinqUpdate(db, AdminUser, oldAddress, PersonPersonal.CRM_Address);
                db.SubmitChanges();
            }
            else
            {
                CRM.Code.History.History.RecordLinqInsert(AdminUser, PersonPersonal);
                CRM.Code.History.History.RecordLinqInsert(AdminUser, PersonPersonal.CRM_Address);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            confirmationDelete.ShowPopup();
        }


        protected void btnRealDelete_Click(object sender, EventArgs e)
        {
            object oldEntity = PersonPersonal.ShallowCopy();

            PersonPersonal.IsArchived = true;
            db.SubmitChanges();

            CRM.Code.History.History.RecordLinqUpdate(db, AdminUser, oldEntity, PersonPersonal);
            db.SubmitChanges();

            NoticeManager.SetMessage("Item removed", "list.aspx");
        }

        protected void btnReinstate_Click(object sender, EventArgs e)
        {
            PersonPersonal.IsArchived = false;
            db.SubmitChanges();

            NoticeManager.SetMessage("Item reinstated");
        }


    }
}