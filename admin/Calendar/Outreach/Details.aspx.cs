using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Models;
using CRM.Code.BasePages.Admin;
using CRM.Code.BasePages.Admin.Calendar;
using CRM.Code.Interfaces;
using CRM.Code.Utils.WebControl;
using CRM.Code.Helpers;
using CRM.Code.Managers;

namespace CRM.admin.Calendar.Outreach
{
    public partial class Details : CRM_CalendarPage<CRM_Calendar>
    {
        protected CRM_CalendarOutreach CRM_CalendarOutreach;
        protected void Page_Load(object sender, EventArgs e)
        {
            CRM_CalendarOutreach = Entity.CRM_CalendarOutreaches.FirstOrDefault();

            ucLogHistory.IHistory = CRM_CalendarOutreach;
            ucLogNotes.INotes = Entity;
            ucNavCal.Entity = Entity;

            CRMContext = Entity;

            // buttons //

            btnSubmit.EventHandler = btnSubmit_Click;
            btnSubmitChanges.EventHandler = btnSubmitChanges_Click;

            // Security //

            FormAutoCompletes();

            if (!Page.IsPostBack)
            {
                if (CRM_CalendarOutreach == null)
                {
                    ddlOffer.DataSource = CRM_Offer.SetDropDown(db.CRM_Offers.OrderBy(o => o.Name).Cast<IArchivable>(), null);
                    ddlOffer.DataBind();
                }
                else
                {
                    ddlOffer.DataSource = CRM_Offer.SetDropDown(db.CRM_Offers.OrderBy(o => o.Name).Cast<IArchivable>(), CRM_CalendarOutreach.CRM_Offer);
                    ddlOffer.DataBind();
                    PopulateFields();
                }
            }
        }
        protected void FormAutoCompletes()
        {
            acSchoolOrg.EventHandler = lnkSelectSchool_Click;
            acSchoolOrg.Config = new AutoCompleteConfig(JSONSet.DataSets.schoolorgs);
            acFacilitator.EventHandler = lnkSelectFacilitator_Click;
            acFacilitator.Config = new AutoCompleteConfig(JSONSet.DataSets.contact);

        }
        protected void lnkSelectSchool_Click(object sender, EventArgs e)
        {
            IAutocomplete org = null;


            AutoCompleteManager acManager = new AutoCompleteManager();
            org = acManager.GetIAutocompleteByReference(acSchoolOrg.SelectedID);

            if (org != null)
                acSchoolOrg.Populate(org);
        }

        protected void lnkSelectFacilitator_Click(object sender, EventArgs e)
        {
            IContact facil = new ContactManager().Contacts.Single(v => v.Reference == acFacilitator.SelectedID);
            acFacilitator.Populate(facil);
        }


        private void PopulateFields()
        {

            AutoCompleteManager acManager = new AutoCompleteManager();
            acSchoolOrg.Populate(acManager.GetIAutocompleteByReference(CRM_CalendarOutreach.SchoolOrgReference));
            //acSchoolPerson.Populate(CRM_CalendarGroupBooking.CRM_PersonSchool);

            if (!String.IsNullOrEmpty(CRM_CalendarOutreach.Facilitator))
            {
                acFacilitator.Populate(new ContactManager().Contacts.Single(c => c.Reference == CRM_CalendarOutreach.Facilitator));
            }


            chkConfirmationSent.Checked = CRM_CalendarOutreach.ConfirmationSent;
            txtSpecialNeeds.Text = CRM_CalendarOutreach.SENNoChildren.ToString();
            txtSpecialNeedsDetails.Text = CRM_CalendarOutreach.SENDetails;

            txtPupils.Text = CRM_CalendarOutreach.NoPupils.ToString();
            txtAdults.Text = CRM_CalendarOutreach.NoAdults.ToString();
            txtYearGroup.Text = CRM_CalendarOutreach.Yeargroup;

            txtAttendedAdults.Text = CRM_CalendarOutreach.ActualAdults.ToString();
            txtAttendedChildren.Text = CRM_CalendarOutreach.ActualChildren.ToString();
            txtAttendedTeachers.Text = CRM_CalendarOutreach.ActualTeachers.ToString();

            txtInitials.Text = CRM_CalendarOutreach.ConfirmationInitials;

            txtWorkshopTimes.Text = CRM_CalendarOutreach.WorkshopTimes;

            if (CRM_CalendarOutreach.ConfirmationSentDate != null)
            {
                txtConfirmationSent.Value = (DateTime)CRM_CalendarOutreach.ConfirmationSentDate;
            }

            if (CRM_CalendarOutreach.CRM_OfferID != null)
                ddlOffer.SelectedValue = CRM_CalendarOutreach.CRM_OfferID.ToString();

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(true);

                NoticeManager.SetMessage("Item Added", "../finance/details.aspx?id=" + Entity.ID);
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
                CRM_CalendarOutreach = new CRM_CalendarOutreach();
                CRM_CalendarOutreach.CRM_CalendarID = Entity.ID;
                db.CRM_CalendarOutreaches.InsertOnSubmit(CRM_CalendarOutreach);
            }
            else
            {
                oldEntity = CRM_CalendarOutreach.ShallowCopy();
            }

          

            IAutocomplete org = new AutoCompleteManager().GetIAutocompleteByReference(acSchoolOrg.SelectedID);

            CRM_CalendarOutreach.SchoolOrgReference = org.Reference;
            CRM_CalendarOutreach.OrganisationName = org.Name;

            CRM_CalendarOutreach.ActualAdults = Convert.ToInt32(txtAttendedAdults.Text);
            CRM_CalendarOutreach.ActualChildren = Convert.ToInt32(txtAttendedChildren.Text);
            CRM_CalendarOutreach.ActualTeachers = Convert.ToInt32(txtAttendedTeachers.Text);

            CRM_CalendarOutreach.SENNoChildren = Convert.ToInt32(txtSpecialNeeds.Text);
            CRM_CalendarOutreach.SENDetails = txtSpecialNeedsDetails.Text;

            CRM_CalendarOutreach.WorkshopTimes = txtWorkshopTimes.Text;

            if (acFacilitator.SelectedID != "")
                CRM_CalendarOutreach.Facilitator = acFacilitator.SelectedID;
            else
                CRM_CalendarOutreach.Facilitator = String.Empty;


            if (ddlOffer.SelectedValue != "")
                CRM_CalendarOutreach.CRM_OfferID = Convert.ToInt32(ddlOffer.SelectedValue);
            else
                CRM_CalendarOutreach.CRM_OfferID = null;

            if (txtConfirmationSent.Text.Length != 0)
                CRM_CalendarOutreach.ConfirmationSentDate = txtConfirmationSent.Value;
            else
                CRM_CalendarOutreach.ConfirmationSentDate = null;

            CRM_CalendarOutreach.ConfirmationInitials = txtInitials.Text;

            db.SubmitChanges();

            if (oldEntity != null)
            {
                CRM.Code.History.History.RecordLinqUpdate(AdminUser, oldEntity, CRM_CalendarOutreach);
                db.SubmitChanges();
            }
            else
            {
                CRM.Code.History.History.RecordLinqInsert(AdminUser, CRM_CalendarOutreach);
            }
        }



    }


}