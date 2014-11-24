using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Managers;
using CRM.Code.Models;
using CRM.Controls.Admin.SharedObjects;
using CRM.Code.Interfaces;

namespace CRM.admin.School
{
    public partial class Details : AdminPage
    {
        protected CRM.Code.Models.CRM_School Entity = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);

            Entity = db.CRM_Schools.SingleOrDefault(o => o.ID.ToString() == Request.QueryString["id"]);

            // buttons //
            btnDelete.EventHandler = btnDelete_Click;
            btnSubmit.EventHandler = btnSubmit_Click;
            btnSubmitChanges.EventHandler = btnSubmitChanges_Click;
            ucCustomFields._DataTableID = db._DataTables.Single(c => c.TableReference == "CRM_School").ID;
            btnReinstate.EventHandler = btnReinstate_Click;

            ucLogHistory.IHistory = Entity;
            ucLogNotes.INotes = Entity;

            // confirmations //

            confirmationDelete.StandardDeleteHidden("organisation", btnRealDelete_Click);

            CRMContext = Entity;
            // process //

            if (!IsPostBack)
            {
                ddlSchoolType.DataSource = CRM_SchoolType.SetDropDown(db.CRM_SchoolTypes.Select(s => (IArchivable)s), Entity == null ? null : Entity.CRM_SchoolType);
                ddlSchoolType.DataBind();

                ddlLEA.DataSource = CRM_LEA.BaseSet(db);
                ddlLEA.DataBind();

                chkKeyStages.DataSource = CRM_KeyStage.SetCheckboxList(db.CRM_KeyStages.Select(s => (IArchivable)s), Entity == null ? null : Entity.CRM_SchoolKeystages.Select(s => (ICRMRecord)s.CRM_KeyStage));
                chkKeyStages.DataBind();

                if (Entity != null)
                    PopulateFields();
                else
                    ucCustomFields.Populate(String.Empty);
            }

            ucNavSchool.Entity = Entity;

        }



        private void PopulateFields()
        {
            txtOrganisationName.Text = Entity.Name;
            ddlSchoolType.SelectedValue = Entity.CRM_SchoolTypeID.ToString();

            if (Entity.CRM_LEAID != null)
                ddlLEA.SelectedValue = Entity.CRM_LEAID.ToString();
            else
                ddlLEA.SelectedValue = "";

            txtPupils.Text = Entity.ApproxPupils.ToString();
            txtSage.Text = Entity.SageAccountNumber;
            txtSENFrequency.Text = Entity.SENSupportFreq;
            chkIsSEN.Checked = Entity.IsSEN;
            txtPhone.Text = Entity.Phone;
            txtEmail.Text = Entity.Email;

            ucAddress.Populate(Entity.CRM_Address);


            ucCustomFields._DataTableID = db._DataTables.Single(c => c.TableReference == "CRM_School").ID;
            ucCustomFields.Populate(Entity.Reference);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid && ucCustomFields.IsValid())
            {
                SaveRecord(true);

                NoticeManager.SetMessage("Item Added", "details.aspx?id=" + Entity.ID);
            }
        }

        protected void btnSubmitChanges_Click(object sender, EventArgs e)
        {
            if (Page.IsValid && ucCustomFields.IsValid())
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
                Entity = new CRM_School();
                Entity.IsArchived = false;
                Entity.CRM_Address = new CRM_Address();
                Entity.PrimaryContactReference = "";
                db.CRM_Schools.InsertOnSubmit(Entity);
            }
            else
            {
                oldEntity = Entity.ShallowCopy();
                oldAddress = Entity.CRM_Address.ShallowCopy();
            }


            if (ddlLEA.SelectedValue != "")
                Entity.CRM_LEAID = Convert.ToInt32(ddlLEA.SelectedValue);
            else Entity.CRM_LEAID = null;

            Entity.Name = txtOrganisationName.Text;
            Entity.CRM_SchoolTypeID = Convert.ToInt32(ddlSchoolType.SelectedValue);

            Entity.ApproxPupils = Convert.ToInt32(txtPupils.Text);
            Entity.SageAccountNumber = txtSage.Text;
            Entity.SENSupportFreq = txtSENFrequency.Text;
            Entity.IsSEN = chkIsSEN.Checked;
            Entity.Email = txtEmail.Text;
            Entity.Phone = txtPhone.Text;

            ucAddress.Save(Entity.CRM_Address);

            db.CRM_SchoolKeystages.DeleteAllOnSubmit(Entity.CRM_SchoolKeystages);
            
            db.SubmitChanges();

            foreach (ListItem item in chkKeyStages.Items)
            {
                if (item.Selected)
                {
                    CRM_SchoolKeystage sKs = new CRM_SchoolKeystage()
                    {
                        CRM_KeyStageID = Convert.ToInt32(item.Value),
                        CRM_SchoolID = Entity.ID
                    };

                    db.CRM_SchoolKeystages.InsertOnSubmit(sKs);
                    db.SubmitChanges();
                }
            }
            
            if (oldEntity != null)
            {
                CRM.Code.History.History.RecordLinqUpdate(AdminUser, oldEntity, Entity);
                CRM.Code.History.History.RecordLinqUpdate(AdminUser, oldAddress, Entity.CRM_Address);
                db.SubmitChanges();
            }
            else
            {
                CRM.Code.History.History.RecordLinqInsert(AdminUser, Entity);
                CRM.Code.History.History.RecordLinqInsert(AdminUser, Entity.CRM_Address);
            }

            ucCustomFields.Save(Entity.Reference);


        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            confirmationDelete.ShowPopup();
        }


        protected void btnRealDelete_Click(object sender, EventArgs e)
        {
            object oldEntity = Entity.ShallowCopy();

            Entity.IsArchived = true;
            db.SubmitChanges();

            CRM.Code.History.History.RecordLinqUpdate(db, AdminUser, oldEntity, Entity);
            db.SubmitChanges();

            NoticeManager.SetMessage("Item removed", "list.aspx");
        }

        protected void btnReinstate_Click(object sender, EventArgs e)
        {
            Entity.IsArchived = false;
            db.SubmitChanges();

            NoticeManager.SetMessage("Item reinstated");
        }

        protected void chkKeyStages_DataBound(object sender, EventArgs e)
        {
            if (Entity != null)
            {
                foreach (ListItem item in ((CheckBoxList)sender).Items)
                {
                    foreach (CRM_KeyStage stage in Entity.CRM_SchoolKeystages.Select(c => c.CRM_KeyStage))
                    {
                        if (item.Value == stage.ListItem.Value)
                        {
                            item.Selected = true;
                        }
                    }
                }
            }
        }


    }
}