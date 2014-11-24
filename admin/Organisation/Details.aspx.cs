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
using CRM.Controls.Forms;

namespace CRM.admin.Organisation
{
    public partial class Details : AdminPage
    {
        protected CRM.Code.Models.CRM_Organisation Entity = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);

            Entity = db.CRM_Organisations.SingleOrDefault(o => o.ID.ToString() == Request.QueryString["id"]);

            // buttons //
            btnDelete.EventHandler = btnDelete_Click;
            btnSubmit.EventHandler = btnSubmit_Click;
            btnSubmitChanges.EventHandler = btnSubmitChanges_Click;
            btnReinstate.EventHandler = btnReinstate_Click;

            ucCustomFields._DataTableID = db._DataTables.Single(c => c.TableReference == "CRM_Organisation").ID;
            ucLogHistory.IHistory = Entity;
            ucLogNotes.INotes = Entity;

            // Security //

            if (!PermissionManager.CanAdd && Entity == null)
                Response.Redirect("list.aspx");

            // confirmations //

            confirmationDelete.StandardDeleteHidden("organisation", btnRealDelete_Click);

            // process //


            ((UserControlTextBox)ucAddress.FindControl("txtAddress1")).TextBox.Attributes[CRM_Organisation.DataKey] = ((byte)CRM_Organisation.SearchKeys.Address1).ToString();
            ((UserControlTextBox)ucAddress.FindControl("txtPostcode")).TextBox.Attributes[CRM_Organisation.DataKey] = ((byte)CRM_Organisation.SearchKeys.Postcode).ToString();
            txtOrganisationName.TextBox.Attributes[CRM_Organisation.DataKey] = ((byte)CRM_Organisation.SearchKeys.Name).ToString();

            ucNavOrganisation.Entity = Entity;

            if (!IsPostBack)
            {
                ddlOrgType.DataSource = CRM_OrganisationType.SetDropDown(db.CRM_OrganisationTypes.Cast<IArchivable>(),
                                                                            Entity == null ? null : Entity.CRM_OrganisationType);
                ddlOrgType.DataBind();

                if (Entity != null)
                    PopulateFields();
                else
                    ucCustomFields.Populate(String.Empty);
            }
        }



        private void PopulateFields()
        {
            txtOrganisationName.Text = Entity.Name;
            ddlOrgType.SelectedValue = Entity.CRM_OrganisationTypeID.ToString();
            ucAddress.Populate(Entity.CRM_Address);

            ucCustomFields._DataTableID = db._DataTables.Single(c => c.TableReference == "CRM_Organisation").ID;
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
                Entity = new CRM_Organisation();
                Entity.CRM_Address = new CRM_Address();
                Entity.PrimaryContactReference = "";
                db.CRM_Organisations.InsertOnSubmit(Entity);
            }
            else
            {
                oldEntity = Entity.ShallowCopy();
                oldAddress = Entity.CRM_Address.ShallowCopy();
            }


            Entity.Name = txtOrganisationName.Text;
            Entity.CRM_OrganisationTypeID = Convert.ToInt32(ddlOrgType.SelectedValue);
            ucAddress.Save(Entity.CRM_Address);
           
            db.SubmitChanges();

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



    }
}