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
using CRM.Code.History;
using CRM.Code.Interfaces;
using System.IO;
using CRM.Code.Utils.Time;
using CRM.Controls.Forms;

namespace CRM.admin.Person
{
    public partial class Details : AdminPage
    {
        protected CRM.Code.Models.CRM_Person Entity = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);

            Entity = db.CRM_Persons.SingleOrDefault(c => c.ID.ToString() == Request.QueryString["id"]);
            ucLogHistory.IHistory = Entity;
            ucLogNotes.INotes = Entity;
            ucCustomFields._DataTableID = db._DataTables.Single(c => c.TableReference == "CRM_Person").ID;
            // buttons //
            btnDelete.EventHandler = btnDelete_Click;
            btnSubmit.EventHandler = btnSubmit_Click;
            btnSubmitChanges.EventHandler = btnSubmitChanges_Click;

            ucNavPerson.Entity = Entity;

            // Security //

            if (!PermissionManager.CanAdd && Entity == null)
                Response.Redirect("list.aspx");

            // confirmations //

            confirmationDelete.StandardDeleteHidden("person", btnRealDelete_Click);

            // process //


            ucDuplicate.BaseObject = new CRM_Person();
            txtFirstname.TextBox.Attributes[CRM_Person.DataKey] = ((byte)CRM_Person.SearchKeys.Firstname).ToString();
            txtLastname.TextBox.Attributes[CRM_Person.DataKey] = ((byte)CRM_Person.SearchKeys.Lastname).ToString();
            txtPrimaryEmail.TextBox.Attributes[CRM_Person.DataKey] = ((byte)CRM_Person.SearchKeys.PrimaryEmail).ToString();
            txtTelephone.TextBox.Attributes[CRM_Person.DataKey] = ((byte)CRM_Person.SearchKeys.Telephone).ToString();
            txtDateOfBirth.TextBox.Attributes[CRM_Person.DataKey] = ((byte)CRM_Person.SearchKeys.DoB).ToString();

            ((UserControlTextBox)ucAddress.FindControl("txtPostcode")).TextBox.Attributes[CRM_Person.DataKey] = ((byte)CRM_Person.SearchKeys.Postcode).ToString();

            ucDuplicate.OriginalID = Entity == null ? 0 : Entity.ID;
            CRMContext = Entity;
            ucAddress.TownRequired = false;
            if (!IsPostBack)
            {
                ddlTitles.DataSource = CRM_Title.SetDropDownWithString(db.CRM_Titles.Cast<IArchivable>(), Entity == null ? "" : Entity.Title);
                ddlTitles.DataBind();

                if (Entity != null)
                {
                    PopulateFields();
                }
                else
                {
                    ucCustomFields.Populate(String.Empty);
                }
            }

            pnlAdultInfo.Visible = !chkIsChild.Checked;
            pnlAdultLeft.Visible = !chkIsChild.Checked;
        }



        private void PopulateFields()
        {
            ucLogComms.IContact = Entity;
            ddlTitles.SelectedValue = Entity.Title;
            txtFirstname.Text = Entity.Firstname;
            txtLastname.Text = Entity.Lastname;
            txtPrevious.Text = Entity.PreviousNames;
            txtPrimaryEmail.Text = Entity.PrimaryEmail;
            txtTelephone.Text = Entity.PrimaryTelephone;
            chkIsContactEmail.Checked = Entity.IsContactEmail;
            chkIsChild.Checked = Entity.IsChild;
            chkIsConcession.Checked = Entity.IsConcession;
            chkIsCarerMinder.Checked = Entity.IsCarerMinder;
            chkIsDeceased.Checked = Entity.IsDeceased;
            chkIsGiftAid.Checked = Entity.IsGiftAid;
            chkIsDonor.Checked = Entity.IsDonor;
            chkIsDoNotEmail.Checked = Entity.IsDoNotEmail;
            chkIsDoNotMail.Checked = Entity.IsDoNotMail;
            txtTelephone2.Text = Entity.Telephone2;

            if (Entity.DateOfBirth != null)
                txtDateOfBirth.Value = (DateTime)Entity.DateOfBirth;

            ((Address)ucAddress).Populate(Entity.CRM_Address);

            if (File.Exists(MapPath(Entity.PersonImageURL)))
                hdnBaseImage.Value = Entity.PersonImageURL;
            else
                hdnBaseImage.Value = CRM_Person.PlaceholderPhoto;

            ucCustomFields._DataTableID = db._DataTables.Single(c => c.TableReference == "CRM_Person").ID;
            ucCustomFields.Populate(Entity.Reference);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid && ucCustomFields.IsValid())
            {
                SaveRecord(true);

                NoticeManager.SetMessage("Person Added", "details.aspx?id=" + Entity.ID);
            }
        }

        protected void btnSubmitChanges_Click(object sender, EventArgs e)
        {
            if (Page.IsValid && ucCustomFields.IsValid())
            {
                SaveRecord(false);
                NoticeManager.SetMessage("Person Updated");
            }
        }

        protected void SaveRecord(bool newRecord)
        {
            // new record / exiting record //

            object oldEntity = null;
            object oldAddress = null;

            if (newRecord)
            {
                Entity = new CRM_Person();
                Entity.IsArchived = false;
                Entity.DateAdded = UKTime.Now;
                db.CRM_Persons.InsertOnSubmit(Entity);
            }
            else
            {
                oldEntity = Entity.ShallowCopy();
                oldAddress = Entity.CRM_Address.ShallowCopy();
            }

            Entity.DateModified = UKTime.Now;
            Entity.Title = ddlTitles.SelectedValue;
            Entity.Firstname = txtFirstname.Text;
            Entity.Lastname = txtLastname.Text;
            Entity.PreviousNames = txtPrevious.Text;
            Entity.PrimaryEmail = txtPrimaryEmail.Text;
            Entity.PrimaryTelephone = txtTelephone.Text;
            Entity.IsContactEmail = chkIsContactEmail.Checked;
            Entity.IsChild = chkIsChild.Checked;
            Entity.IsConcession = chkIsConcession.Checked;
            Entity.IsCarerMinder = chkIsCarerMinder.Checked;
            Entity.IsDeceased = chkIsDeceased.Checked;
            Entity.IsGiftAid = chkIsGiftAid.Checked;            
            Entity.IsDoNotMail = chkIsDoNotMail.Checked;
            Entity.IsDoNotEmail = chkIsDoNotEmail.Checked;
            Entity.AddressType = (byte)CRM_Address.Types.Home;
            Entity.Telephone2 = txtTelephone2.Text;
            if (!String.IsNullOrEmpty(txtDateOfBirth.Text))
                Entity.DateOfBirth = txtDateOfBirth.Value;

            if (newRecord)
            {
                CRM_Address address = new CRM_Address();
                Entity.CRM_Address = (CRM_Address)((Address)ucAddress).Save(address);
                
            }
            else
            {
                Entity.CRM_Address = (CRM_Address)((Address)ucAddress).Save(Entity.CRM_Address);
            }
            db.SubmitChanges();

            Entity.CRM_Address.ParentID = Entity.ID;
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

            if (hdnCaptured.Value == "1")
            {
                File.Copy(MapPath(AdminUser.TempPhotoFile), MapPath(Entity.PersonImageURL), true);
            }

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