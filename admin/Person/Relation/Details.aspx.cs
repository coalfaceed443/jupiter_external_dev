using CRM.Code.BasePages.Admin.Persons;
using CRM.Code.Helpers;
using CRM.Code.Managers;
using CRM.Code.Models;
using CRM.Code.Utils.WebControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM.admin.Person.Relation
{
    public partial class Details : CRM_PersonPage<CRM_Person>
    {
        protected CRM.Code.Models.CRM_PersonRelationship PersonRelationship = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);
            PersonRelationship = db.CRM_PersonRelationships.SingleOrDefault(c => c.ID.ToString() == Request.QueryString["pid"]);

            btnSubmitChanges.Visible = PermissionManager.CanUpdate;
            if (!PermissionManager.CanAdd && Entity == null)
                Response.Redirect("list.aspx");

            ucNavPerson.Entity = Entity;
            ucLogNotes.INotes = PersonRelationship;

            ucAcPerson.EventHandler = lnkSelect_Click;
            ucAcPerson.Config = new AutoCompleteConfig(JSONSet.DataSets.person);

            ucAcPersonB.EventHandler = lnkSelectB_Click;
            ucAcPersonB.Config = new AutoCompleteConfig(JSONSet.DataSets.person);

            // buttons //

            btnSubmit.EventHandler = btnSubmit_Click;
            btnSubmitChanges.EventHandler = btnSubmitChanges_Click;
            btnDelete.EventHandler = btnDelete_Click;
            // Security //

            btnSubmitChanges.Visible = PermissionManager.CanUpdate;
            btnDelete.Visible = PermissionManager.CanDelete;
            if (!PermissionManager.CanAdd && Entity == null)
                Response.Redirect("list.aspx");

            // confirmations //

            confirmationDelete.StandardDeleteHidden("personal relationship record", btnRealDelete_Click);

            // process //

            CRMContext = Entity;

            if (!IsPostBack)
            {
                ddlRelationCodeToA.DataSource = CRM_RelationCode.BaseSet(db);
                ddlRelationCodeToA.DataBind();

                ddlRelationCodetoB.DataSource = CRM_RelationCode.BaseSet(db);
                ddlRelationCodetoB.DataBind();

                if (PersonRelationship != null)
                    PopulateFields();
                else
                {
                    ucAcPerson.Populate(Entity);
                    RebindAddresses();
                }
            }
        }

        protected void lnkSelect_Click(object sender, EventArgs e)
        {
            CRM_Person person = db.CRM_Persons.Single(v => v.ID.ToString() == ucAcPerson.SelectedID);
            ucAcPerson.Populate(person);
            RebindAddresses();
        }

        protected void lnkSelectB_Click(object sender, EventArgs e)
        {
            CRM_Person person = db.CRM_Persons.Single(v => v.ID.ToString() == ucAcPersonB.SelectedID);
            ucAcPersonB.Populate(person);
            RebindAddresses();
        }



        private void PopulateFields()
        {
            ucLogHistory.IHistory = PersonRelationship;
            ucLogHistory.ParentID = Entity.ID.ToString();
            ucAcPerson.Populate(PersonRelationship.PersonA);
            ucAcPersonB.Populate(PersonRelationship.PersonB);
            ddlRelationCodeToA.SelectedValue = PersonRelationship.PersonACode.ID.ToString();
            ddlRelationCodetoB.SelectedValue = PersonRelationship.PersonBCode.ID.ToString();
            RebindAddresses();
            rbPersonAddress.SelectedValue = PersonRelationship.CRM_PersonIDAddress.ToString();
            txtSalutation.Text = PersonRelationship.Salutation;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(true);

                NoticeManager.SetMessage("Item Added", "details.aspx?id=" + Entity.ID + "&pid=" + PersonRelationship.ID);
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
                PersonRelationship = new CRM_PersonRelationship();
                db.CRM_PersonRelationships.InsertOnSubmit(PersonRelationship);
            }
            else
            {
                oldEntity = PersonRelationship.ShallowCopy();
            }

            PersonRelationship.PersonA = db.CRM_Persons.Single(s => s.Reference.ToString() == ucAcPerson.SelectedID);
            PersonRelationship.PersonB = db.CRM_Persons.Single(s => s.Reference.ToString() == ucAcPersonB.SelectedID);

            PersonRelationship.PersonACode = db.CRM_RelationCodes.Single(s => s.ID.ToString() == ddlRelationCodeToA.SelectedValue);
            PersonRelationship.PersonBCode = db.CRM_RelationCodes.Single(s => s.ID.ToString() == ddlRelationCodetoB.SelectedValue);

            PersonRelationship.CRM_PersonIDAddress = Convert.ToInt32(rbPersonAddress.SelectedValue);

            PersonRelationship.Salutation = txtSalutation.Text;

            db.SubmitChanges();

            if (oldEntity != null)
            {
                CRM.Code.History.History.RecordLinqUpdate(db, AdminUser, oldEntity, PersonRelationship);
                db.SubmitChanges();
            }
            else
            {
                CRM.Code.History.History.RecordLinqInsert(AdminUser, PersonRelationship);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            confirmationDelete.ShowPopup();
        }


        protected void RebindAddresses()
        {
            List<ListItem> addresses = new List<ListItem>();

            if (PersonRelationship != null)
            {
                addresses.Add(new ListItem(PersonRelationship.PersonA.PrimaryAddress.FormattedAddress, PersonRelationship.PersonA.ID.ToString()));
                addresses.Add(new ListItem(PersonRelationship.PersonB.PrimaryAddress.FormattedAddress, PersonRelationship.PersonB.ID.ToString()));
            }
            else
            {

                CRM_Person personA = db.CRM_Persons.SingleOrDefault(s => s.Reference.ToString() == ucAcPerson.SelectedID);

                if (personA != null)
                    addresses.Add(new ListItem(personA.PrimaryAddress.FormattedAddress, personA.ID.ToString()));

                CRM_Person personB = db.CRM_Persons.SingleOrDefault(s => s.Reference.ToString() == ucAcPersonB.SelectedID);

                if (personB != null)
                    addresses.Add(new ListItem(personB.PrimaryAddress.FormattedAddress, personB.ID.ToString()));

            }

            rbPersonAddress.DataSource = addresses;
            rbPersonAddress.DataBind();
        }

        protected void btnRealDelete_Click(object sender, EventArgs e)
        {
            object oldEntity = PersonRelationship.ShallowCopy();

            PersonRelationship.IsArchived = true;
            db.SubmitChanges();


            CRM.Code.History.History.RecordLinqUpdate(db, AdminUser, oldEntity, PersonRelationship);
            db.SubmitChanges();

            NoticeManager.SetMessage("Item removed", Entity.RelationListURL);
        }


    }
}