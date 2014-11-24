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

namespace CRM.admin.Person.Families
{
    public partial class Details : CRM_PersonPage<CRM_FamilyPerson>
    {
        protected CRM.Code.Models.CRM_FamilyPerson PersonFamily = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);
            PersonFamily = db.CRM_FamilyPersons.SingleOrDefault(c => c.ID.ToString() == Request.QueryString["pid"]);
            
            btnSubmitChanges.Visible = PermissionManager.CanUpdate;
            if (!PermissionManager.CanAdd && Entity == null)
                Response.Redirect("list.aspx");

            ucNavPerson.Entity = Entity;
            ucLogNotes.INotes = PersonFamily;

            ucACFamily.EventHandler = lnkSelect_Click;
            ucACFamily.Config = new AutoCompleteConfig(JSONSet.DataSets.family);

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

            confirmationDelete.PermanentDeleteHidden("personal family record", btnRealDelete_Click);

            // process //


            CRMContext = PersonFamily;

            if (!IsPostBack)
            {

                if (PersonFamily != null)
                    PopulateFields();
                else if (!String.IsNullOrEmpty(Request.QueryString["familycheck"]))
                {
                    CRM_Family family = db.CRM_Families.Single(v => v.Reference.ToString() == Request.QueryString["familycheck"]);
                    ucACFamily.Populate(family);
                }
            }

            PopulateFamilyList();
        }

        protected void lnkSelect_Click(object sender, EventArgs e)
        {
            CRM_Family family = db.CRM_Families.Single(v => v.ID.ToString() == ucACFamily.SelectedID);
            ucACFamily.Populate(family);
            PopulateFamilyList();
            Response.Redirect("/admin/person/families/details.aspx?id=" + Request.QueryString["id"] + "&familycheck=" + family.Reference);
        }

        private void PopulateFamilyList()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["familycheck"]))
            {
                ucFamilyList.DataSet = db.CRM_FamilyPersons.Where(f => f.CRM_Family.Reference.ToString() == Request.QueryString["familycheck"]).Select(i => (object)i);
                ucFamilyList.Type = typeof(CRM_FamilyPerson);
            }
            else
            {
                ucFamilyList.DataSet = db.CRM_FamilyPersons.Where(f => f.CRM_Family.Reference.ToString() == ucACFamily.SelectedID).Select(i => (object)i);
                ucFamilyList.Type = typeof(CRM_FamilyPerson);
            }
        }

        private void PopulateFields()
        {
            ucLogHistory.IHistory = PersonFamily;
            ucLogHistory.ParentID = Entity.ID.ToString();
            ucACFamily.Populate(PersonFamily.CRM_Family);
            PopulateFamilyList();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(true);

                NoticeManager.SetMessage("Item Added", "details.aspx?id=" + Entity.ID + "&pid=" + PersonFamily.ID);
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
                PersonFamily = new CRM_FamilyPerson();
                PersonFamily.CRM_PersonID = Entity.ID;
                db.CRM_FamilyPersons.InsertOnSubmit(PersonFamily);
            }
            else
            {
                oldEntity = PersonFamily.ShallowCopy();
            }

            if (ucACFamily.SelectedID == "")
            {
                CRM_Family family = new CRM_Family()
                {
                    Name = ucACFamily.Text
                };

                db.CRM_Families.InsertOnSubmit(family);
                PersonFamily.CRM_Family = family;
            }
            else
            {
                PersonFamily.CRM_FamilyID = CRM_Family.BaseSet(db).Single(s => s.Reference == ucACFamily.SelectedID).ID;
            }

            db.SubmitChanges();

            if (oldEntity != null)
            {
                CRM.Code.History.History.RecordLinqUpdate(db, AdminUser, oldEntity, PersonFamily);
                db.SubmitChanges();
            }
            else
            {
                CRM.Code.History.History.RecordLinqInsert(AdminUser, PersonFamily);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            confirmationDelete.ShowPopup();
        }


        protected void btnRealDelete_Click(object sender, EventArgs e)
        {
            db.SubmitChanges();

            NoticeManager.SetMessage("Item removed", "list.aspx");
        }

    }
}