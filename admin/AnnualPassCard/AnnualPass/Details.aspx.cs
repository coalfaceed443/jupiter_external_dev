using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin.AnnualPass;
using CRM.Code.Managers;
using CRM.Code.Models;
using CRM.Code.Utils.Time;
using CRM.Code.Utils.WebControl;
using CRM.Code.Helpers;
using CRM.Code.Interfaces;
using CRM.Code.Utils.WebControl;
using CRM.Code.Utils.Enumeration;

namespace CRM.admin.AnnualPassCard.AnnualPass
{
    public partial class Details : AnnualPassCardPage<CRM_AnnualPassPerson>
    {
        protected CRM_AnnualPass CRM_AnnualPass;
        protected void Page_Load(object sender, EventArgs e)
        {
            CRM_AnnualPass = Entity.CRM_AnnualPasses.SingleOrDefault(f => f.ID.ToString() == Request.QueryString["pid"]);

            btnSubmit.EventHandler = btnSubmit_Click;
            btnSubmitChanges.EventHandler = btnSubmitChanges_Click;
            btnDelete.EventHandler = btnDelete_Click;

            confirmationDelete.StandardDeleteHidden("pass", btnRealDelete_Click);

            ucNav.Entity = Entity;

            ucACPrimaryContact.EventHandler = lnkSelect_Click;
            ucACPrimaryContact.Config = new AutoCompleteConfig(JSONSet.DataSets.contact);
            ucACNewPerson.EventHandler = lnkSelectNew_Click;
            ucACNewPerson.Config = new AutoCompleteConfig(JSONSet.DataSets.person);

            ucNotes.INotes = CRM_AnnualPass;

            if (CRM_AnnualPass != null)
            {
                lvPersons.Type = typeof(CRM_AnnualPassPerson);
                lvPersons.DataSet = db.CRM_AnnualPassPersons.Where(p => p.CRM_AnnualPassID == CRM_AnnualPass.ID).Select(a => (object)a);
                lvPersons.ItemsPerPage = 10;
                lvPersons.ShowCustomisation = false;
            }
            else
            {
                lvPersons.Type = typeof(CRM_AnnualPassPerson);
                lvPersons.DataSet = Enumerable.Empty<CRM_AnnualPassPerson>().Select(a => (object)a);
                lvPersons.ItemsPerPage = 10;
                lvPersons.ShowCustomisation = false;
            }

            CRMContext = CRM_AnnualPass;

            if (!Page.IsPostBack)
            {
                ddlPassType.DataSource = CRM_AnnualPassType.BaseSet(db);
                ddlPassType.DataBind();

                ddlPaymentType.DataSource = Enumeration.GetAll<CRM.Code.Helpers.PaymentType.Types>();
                ddlPaymentType.DataBind();

                if (CRM_AnnualPass != null)
                {
                    ddlOffer.DataSource = CRM_Offer.SetDropDown(db.CRM_Offers.Cast<IArchivable>(), CRM_AnnualPass.CRM_Offer);
                    ddlOffer.DataBind();
                    PopulateFields();
                }
                else
                {
                    ddlOffer.DataSource = CRM_Offer.SetDropDown(db.CRM_Offers.Cast<IArchivable>(), null);
                    ddlOffer.DataBind();

                    lvPersons.Visible = false;
                    txtStartDate.Value = UKTime.Now;
                    txtExpiryDate.Value = UKTime.Now.AddYears(1);
                }
            }
        }

        protected void lnkSelect_Click(object sender, EventArgs e)
        {
            IContact contact = new ContactManager().Contacts.Single(v => v.Reference.ToString() == ucACPrimaryContact.SelectedID);
            ucACPrimaryContact.Populate(contact);
        }

        protected void lnkSelectNew_Click(object sender, EventArgs e)
        {
            CRM_Person person = db.CRM_Persons.Single(c => c.ID.ToString() == ucACNewPerson.SelectedID);
            AddPersonToPass(person.Reference);
            NoticeManager.SetMessage(person.Fullname + " added to pass " + Entity.MembershipNumber);                    
        }

        protected void AddPersonToPass(string reference)
        {
            CRM_Person person = db.CRM_Persons.Single(p => p.Reference == reference);
            CRM_AnnualPassPerson passperson = new CRM_AnnualPassPerson()
            {
                CRM_AnnualPassID = CRM_AnnualPass.ID,
                CRM_PersonID = person.ID,
                IsArchived = false
            };
        
            db.CRM_AnnualPassPersons.InsertOnSubmit(passperson);
            db.SubmitChanges();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(true);

                NoticeManager.SetMessage("Pass Created, start adding members", "details.aspx?id=" + Entity.ID + "&pid=" + CRM_AnnualPass.ID);
            }
        }

        protected void btnSubmitChanges_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(false);

                NoticeManager.SetMessage("Pass Updated");
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            confirmationDelete.ShowPopup();
        }


        protected void btnRealDelete_Click(object sender, EventArgs e)
        {
            foreach (CRM_AnnualPassPerson person in CRM_AnnualPass.CRM_AnnualPassPersons)
            {
                person.IsArchived = true;
            }

            CRM_AnnualPass.IsArchived = true;
            db.SubmitChanges();

            NoticeManager.SetMessage("Item removed", "list.aspx?id=" + CRM_AnnualPass.ID);
        }


        protected void PopulateFields()
        {
            txtAmountPaid.Text = CRM_AnnualPass.AmountPaid.ToString("N2");
            ddlPassType.SelectedValue = CRM_AnnualPass.CRM_AnnualPassTypeID.ToString();
            txtDiscountApplied.Text = CRM_AnnualPass.DiscountApplied;
            txtExpiryDate.Value = CRM_AnnualPass.ExpiryDate;
            txtStartDate.Value = CRM_AnnualPass.StartDate;
            chkIsPending.Checked = CRM_AnnualPass.IsPending;
            ucACPrimaryContact.Populate(new ContactManager().Contacts.Single(c => c.Reference == CRM_AnnualPass.PrimaryContactReference));
            ddlPaymentType.SelectedValue = CRM_AnnualPass.PaymentMethod.ToString();
        }

        protected void SaveRecord(bool newRecord)
        {
            if (newRecord)
            {
                CRM_AnnualPass = new CRM_AnnualPass();
                CRM_AnnualPass.CRM_AnnualPassCardID = Entity.ID;
                db.CRM_AnnualPasses.InsertOnSubmit(CRM_AnnualPass);               
            }

            CRM_AnnualPass.AmountPaid = Convert.ToDecimal(txtAmountPaid.Text);
            CRM_AnnualPass.CRM_AnnualPassType = db.CRM_AnnualPassTypes.Single(s => s.ID.ToString() == ddlPassType.SelectedValue);
            CRM_AnnualPass.DiscountApplied = txtDiscountApplied.Text;
            CRM_AnnualPass.ExpiryDate = txtExpiryDate.Value;
            CRM_AnnualPass.StartDate = txtStartDate.Value;
            CRM_AnnualPass.IsPending = chkIsPending.Checked;
            CRM_AnnualPass.PrimaryContactReference = ucACPrimaryContact.SelectedID;
            CRM_AnnualPass.PaymentMethod = Convert.ToByte(ddlPaymentType.SelectedValue);
            db.SubmitChanges();

            if (newRecord)
            {
                IContact contact = new ContactManager().Contacts.Single(v => v.Reference.ToString() == ucACPrimaryContact.SelectedID);
                AddPersonToPass(contact.Parent_CRM_Person.Reference);
            }
        }


    }
}