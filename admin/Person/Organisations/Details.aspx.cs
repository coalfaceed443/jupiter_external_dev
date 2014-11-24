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
using CRM.Code.Utils.WebControl;
using CRM.Code.Helpers;

namespace CRM.admin.Person.Organisation
{
    public partial class Details : CRM_PersonPage<CRM_Person>
    {
        protected CRM.Code.Models.CRM_PersonOrganisation PersonOrganisation = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);
            PersonOrganisation = db.CRM_PersonOrganisations.SingleOrDefault(c => c.ID.ToString() == Request.QueryString["pid"]);
            
            btnSubmitChanges.Visible = PermissionManager.CanUpdate;
            if (!PermissionManager.CanAdd && Entity == null)
                Response.Redirect("list.aspx");

            ucNavPerson.Entity = Entity;
            ucLogNotes.INotes = PersonOrganisation;
            
            ucACOrganisation.EventHandler = lnkSelect_Click;
            ucACOrganisation.Config = new AutoCompleteConfig(JSONSet.DataSets.organisation);

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

            confirmationDelete.StandardDeleteHidden("personal organisation record", btnRealDelete_Click);

            // process //


            CRMContext = PersonOrganisation;

            if (!IsPostBack)
            {
                ddlRole.DataSource = CRM_Role.BaseSet(db);
                ddlRole.DataBind();

                if (PersonOrganisation != null)
                    PopulateFields();
            }
        }

        protected void lnkSelect_Click(object sender, EventArgs e)
        {
            CRM_Organisation organisation = db.CRM_Organisations.Single(v => v.ID.ToString() == ucACOrganisation.SelectedID);
            ucACOrganisation.Populate(organisation);
        }


        private void PopulateFields()
        {
            ucLogHistory.IHistory = PersonOrganisation;
            ucLogHistory.ParentID = Entity.ID.ToString();
            txtTelephone.Text = PersonOrganisation.Telephone;
            txtEmail.Text = PersonOrganisation.Email;
            ucACOrganisation.Populate(PersonOrganisation.CRM_Organisation);
            ddlRole.SelectedValue = PersonOrganisation.CRM_RoleID.ToString();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(true);

                NoticeManager.SetMessage("Item Added", "details.aspx?id=" + Entity.ID + "&pid=" + PersonOrganisation.ID);
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
            if (newRecord)
            {
                PersonOrganisation = new CRM_PersonOrganisation();
                PersonOrganisation.IsArchived = false;
                PersonOrganisation.CRM_PersonID = Entity.ID;
                db.CRM_PersonOrganisations.InsertOnSubmit(PersonOrganisation);
            }
            else
            {
                oldEntity = PersonOrganisation.ShallowCopy();
            }

            PersonOrganisation.Telephone = txtTelephone.Text;
            PersonOrganisation.Email = txtEmail.Text;
            PersonOrganisation.CRM_Organisation = db.CRM_Organisations.Single(o => o.Reference == ucACOrganisation.SelectedID);
            PersonOrganisation.CRM_Role = db.CRM_Roles.Single(r => r.ID.ToString() == ddlRole.SelectedValue);

            db.SubmitChanges();

            if (oldEntity != null)
            {
                CRM.Code.History.History.RecordLinqUpdate(db, AdminUser, oldEntity, PersonOrganisation);
                db.SubmitChanges();
            }
            else
            {
                CRM.Code.History.History.RecordLinqInsert(AdminUser, PersonOrganisation);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            confirmationDelete.ShowPopup();
        }


        protected void btnRealDelete_Click(object sender, EventArgs e)
        {
            object oldEntity = PersonOrganisation.ShallowCopy();

            PersonOrganisation.IsArchived = true;
            db.SubmitChanges();

            CRM.Code.History.History.RecordLinqUpdate(db, AdminUser, oldEntity, PersonOrganisation);
            db.SubmitChanges();

            NoticeManager.SetMessage("Item removed", "list.aspx");
        }

        protected void btnReinstate_Click(object sender, EventArgs e)
        {
            PersonOrganisation.IsArchived = false;
            db.SubmitChanges();

            NoticeManager.SetMessage("Item reinstated");
        }



    }
}