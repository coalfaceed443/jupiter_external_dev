using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Managers;
using CRM.Code.Models;
using CRM.Code.Utils.WebControl;
using CRM.Code.Helpers;

namespace CRM.admin.Families
{
    public partial class Details : AdminPage
    {
        protected CRM.Code.Models.CRM_Family Entity = null;

        protected void Page_Load(object sender, EventArgs e)
        {

            Entity = db.CRM_Families.SingleOrDefault(o => o.ID.ToString() == Request.QueryString["id"]);
            // buttons //
            btnDelete.EventHandler = btnDelete_Click;
            btnSubmit.EventHandler = btnSubmit_Click;
            btnSubmitChanges.EventHandler = btnSubmitChanges_Click;

            ucLogHistory.IHistory = Entity;
            ucLogNotes.INotes = Entity;


            ucACFamily.EventHandler = lnkSelect_Click;
            ucACFamily.Config = new AutoCompleteConfig(JSONSet.DataSets.person);

            ucFamilyPersons.Type = typeof(CRM_FamilyPerson);

            if (Entity != null)
            {
                PopulateFields();
                PopulateFamilyList();
            }
            else
            {
                ucFamilyPersons.Visible = false;
            }

            // confirmations //

            confirmationDelete.StandardDeleteHidden("family record", btnRealDelete_Click);

        }

        private void PopulateFields()
        {
            ucLogHistory.IHistory = Entity;
            txtName.Text = Entity.Name;
        }

        protected void lnkSelect_Click(object sender, EventArgs e)
        {
            CRM_Person person = CRM_Person.BaseSet(db).Single(v => v.ID.ToString() == ucACFamily.SelectedID);
            CRM_FamilyPerson fPerson = new CRM_FamilyPerson()
            {
                CRM_FamilyID = Entity.ID,
                CRM_PersonID = person.ID,
                IsPrimaryContact = false
            };

            db.CRM_FamilyPersons.InsertOnSubmit(fPerson);
            db.SubmitChanges();

            NoticeManager.SetMessage(person.Name + " added to the family");
        }
        private void PopulateFamilyList()
        {
            ucFamilyPersons.DataSet = db.CRM_FamilyPersons.Where(f => f.CRM_Family.Reference.ToString() == Entity.Reference).Select(f => (object)f);
            ucFamilyPersons.Type = typeof(CRM_FamilyPerson);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(true);

                NoticeManager.SetMessage("Item Added", "details.aspx?id=" + Entity.ID);
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
                Entity = new CRM_Family();
                db.CRM_Families.InsertOnSubmit(Entity);
            }
            else
            {
                oldEntity = Entity.ShallowCopy();
            }

            Entity.Name = txtName.Text;

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
            object oldEntity = Entity.ShallowCopy();

            CRM.Code.History.History.RecordLinqDelete(AdminUser, Entity);
            db.SubmitChanges();

            db.CRM_FamilyPersons.DeleteAllOnSubmit(Entity.CRM_FamilyPersons);
            db.SubmitChanges();

            db.CRM_Families.DeleteOnSubmit(Entity);
            db.SubmitChanges();

            NoticeManager.SetMessage("Item removed", "list.aspx");
        }
    }
}