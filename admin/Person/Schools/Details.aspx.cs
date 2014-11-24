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

namespace CRM.admin.Person.School
{
    public partial class Details : CRM_PersonPage<CRM_Person>
    {
        protected CRM.Code.Models.CRM_PersonSchool PersonSchool = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);
            PersonSchool = db.CRM_PersonSchools.SingleOrDefault(c => c.ID.ToString() == Request.QueryString["pid"]);
            
            btnSubmitChanges.Visible = PermissionManager.CanUpdate;
            if (!PermissionManager.CanAdd && Entity == null)
                Response.Redirect("list.aspx");

            ucNavPerson.Entity = Entity;
            ucLogNotes.INotes = PersonSchool;

            ucACSchool.EventHandler = lnkSelect_Click;
            ucACSchool.Config = new AutoCompleteConfig(JSONSet.DataSets.school);

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

            confirmationDelete.StandardDeleteHidden("personal school record", btnRealDelete_Click);

            // process //

            CRMContext = Entity;

            if (!IsPostBack)
            {
                ddlRole.DataSource = CRM_Role.BaseSet(db);
                ddlRole.DataBind();

                if (PersonSchool != null)
                    PopulateFields();
            }
        }

        protected void lnkSelect_Click(object sender, EventArgs e)
        {
            CRM_School school = db.CRM_Schools.Single(v => v.ID.ToString() == ucACSchool.SelectedID);
            ucACSchool.Populate(school);
        }


        private void PopulateFields()
        {
            ucLogHistory.IHistory = PersonSchool;
            ucLogHistory.ParentID = Entity.ID.ToString();
            txtTelephone.Text = PersonSchool.Telephone;
            txtEmail.Text = PersonSchool.Email;
            ucACSchool.Populate(PersonSchool.CRM_School);
            ddlRole.SelectedValue = PersonSchool.CRM_RoleID.ToString();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(true);

                NoticeManager.SetMessage("Item Added", "details.aspx?id=" + Entity.ID + "&pid=" + PersonSchool.ID);
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
                PersonSchool = new CRM_PersonSchool();
                PersonSchool.IsArchived = false;
                PersonSchool.CRM_PersonID = Entity.ID;
                db.CRM_PersonSchools.InsertOnSubmit(PersonSchool);
            }
            else
            {
                oldEntity = PersonSchool.ShallowCopy();
            }

            PersonSchool.Telephone = txtTelephone.Text;
            PersonSchool.Email = txtEmail.Text;
            PersonSchool.CRM_School = db.CRM_Schools.Single(s => s.Reference.ToString() == ucACSchool.SelectedID);
            PersonSchool.CRM_Role = db.CRM_Roles.Single(r => r.ID.ToString() == ddlRole.SelectedValue);

            db.SubmitChanges();

            if (oldEntity != null)
            {
                CRM.Code.History.History.RecordLinqUpdate(db, AdminUser, oldEntity, PersonSchool);
                db.SubmitChanges();
            }
            else
            {
                CRM.Code.History.History.RecordLinqInsert(AdminUser, PersonSchool);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            confirmationDelete.ShowPopup();
        }


        protected void btnRealDelete_Click(object sender, EventArgs e)
        {
            object oldEntity = PersonSchool.ShallowCopy();

            PersonSchool.IsArchived = true;
            db.SubmitChanges();

            CRM.Code.History.History.RecordLinqUpdate(db, AdminUser, oldEntity, PersonSchool);
            db.SubmitChanges();

            NoticeManager.SetMessage("Item removed", "list.aspx");
        }

        protected void btnReinstate_Click(object sender, EventArgs e)
        {
            PersonSchool.IsArchived = false;
            db.SubmitChanges();

            NoticeManager.SetMessage("Item reinstated");
        }



    }
}