using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin.Calendar;
using CRM.Code.Utils.WebControl;
using CRM.Code.Helpers;
using CRM.Code.Models;
using CRM.Code.Interfaces;
using CRM.Code.Managers;

namespace CRM.admin.Calendar.GroupBookings
{
    public partial class Details : CRM_CalendarPage<CRM_Calendar>
    {
        protected CRM_CalendarGroupBooking CRM_CalendarGroupBooking;
        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);

            CRM_CalendarGroupBooking = Entity.CRM_CalendarGroupBookings.FirstOrDefault();

            ucLogHistory.IHistory = CRM_CalendarGroupBooking;
            ucLogNotes.INotes = Entity;
            ucNavCal.Entity = Entity;

            CRMContext = Entity;

            // buttons //

            btnSubmit.EventHandler = btnSubmit_Click;
            btnSubmitChanges.EventHandler = btnSubmitChanges_Click;

            // Security //

            btnSubmitChanges.Visible = PermissionManager.CanUpdate;

            if (!PermissionManager.CanAdd && CRM_CalendarGroupBooking == null)
                Response.Redirect(Entity.DetailsURL);

            FormAutoCompletes();

            if (!Page.IsPostBack)
            {
                ddlEvent.DataSource = from ev in db.Events
                                      where ev.IsActive
                                      where !ev.IsArchived
                                      orderby ev.Name
                                      select ev;
                ddlEvent.DataBind();

                if (CRM_CalendarGroupBooking == null)
                {
                    ddlOffer.DataSource = CRM_Offer.SetDropDown(db.CRM_Offers.OrderBy(o => o.Name).Cast<IArchivable>(), null);
                    ddlOffer.DataBind();
                    ddlExhibition.DataSource = CRM_Exhibition.SetDropDown(db.CRM_Exhibitions.OrderBy(o => o.Name).Cast<IArchivable>(), null);
                    ddlExhibition.DataBind();
                }
                else
                {
                    ddlOffer.DataSource = CRM_Offer.SetDropDown(db.CRM_Offers.OrderBy(o => o.Name).Cast<IArchivable>(), CRM_CalendarGroupBooking.CRM_Offer);
                    ddlOffer.DataBind();
                    ddlExhibition.DataSource = CRM_Exhibition.SetDropDown(db.CRM_Exhibitions.OrderBy(o => o.Name).Cast<IArchivable>(), CRM_CalendarGroupBooking.CRM_Exhibition);
                    ddlExhibition.DataBind();

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
            acFacilitator2.EventHandler = lnkSelectFacilitatorTwo_Click;
            acFacilitator2.Config = new AutoCompleteConfig(JSONSet.DataSets.contact);
            acFacilitator3.EventHandler = lnkSelectFacilitatorThree_Click;
            acFacilitator3.Config = new AutoCompleteConfig(JSONSet.DataSets.contact);
            acSchoolPerson.EventHandler = lnkSelectSchoolPerson_Click;
            acSchoolPerson.Config = new AutoCompleteConfig(JSONSet.DataSets.schoolperson);
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
        protected void lnkSelectFacilitatorTwo_Click(object sender, EventArgs e)
        {
            IContact facil = new ContactManager().Contacts.Single(v => v.Reference == acFacilitator2.SelectedID);
            acFacilitator2.Populate(facil);
        }
        protected void lnkSelectFacilitatorThree_Click(object sender, EventArgs e)
        {
            IContact facil = new ContactManager().Contacts.Single(v => v.Reference == acFacilitator3.SelectedID);
            acFacilitator3.Populate(facil);
        }

        protected void lnkSelectSchoolPerson_Click(object sender, EventArgs e)
        {
            CRM_PersonSchool schoolp = db.CRM_PersonSchools.Single(v => v.ID.ToString() == acSchoolPerson.SelectedID);
            acSchoolPerson.Populate(schoolp);
        }

        private void PopulateFields()
        {

            AutoCompleteManager acManager = new AutoCompleteManager();
            acSchoolOrg.Populate(acManager.GetIAutocompleteByReference(CRM_CalendarGroupBooking.SchoolOrgReference));
            //acSchoolPerson.Populate(CRM_CalendarGroupBooking.CRM_PersonSchool);

            if (!String.IsNullOrEmpty(CRM_CalendarGroupBooking.Facilitator))
            {
                acFacilitator.Populate(new ContactManager().Contacts.Single(c => c.Reference == CRM_CalendarGroupBooking.Facilitator));
            }

            if (!String.IsNullOrEmpty(CRM_CalendarGroupBooking.FacilitatorTwo))
            {
                acFacilitator2.Populate(new ContactManager().Contacts.Single(c => c.Reference == CRM_CalendarGroupBooking.FacilitatorTwo));
            }

            if (!String.IsNullOrEmpty(CRM_CalendarGroupBooking.FacilitatorThree))
            {
                acFacilitator3.Populate(new ContactManager().Contacts.Single(c => c.Reference == CRM_CalendarGroupBooking.FacilitatorThree));
            }

            chkIsBookshopRequired.Checked = CRM_CalendarGroupBooking.IsBookshopRequired;
            chkIsWithin2Hours.Checked = CRM_CalendarGroupBooking.Within2HourSlot;
            chkRequiresLunch.Checked = CRM_CalendarGroupBooking.IsPackedLunchSpace;
            chkConfirmationSent.Checked = CRM_CalendarGroupBooking.ConfirmationSent;
            txtSpecialNeeds.Text = CRM_CalendarGroupBooking.SpecialNeeds.ToString();

            txtPhone.Text = CRM_CalendarGroupBooking.Phone;
            txtEmail.Text = CRM_CalendarGroupBooking.Email;
            txtPupils.Text = CRM_CalendarGroupBooking.Pupils.ToString();
            txtAdults.Text = CRM_CalendarGroupBooking.Adults.ToString();
            txtYearGroup.Text = CRM_CalendarGroupBooking.Yeargroup;

            txtAttendedAdults.Text = CRM_CalendarGroupBooking.ActualAdults.ToString();
            txtAttendedChildren.Text = CRM_CalendarGroupBooking.ActualChildren.ToString();
            txtAttendedTeachers.Text = CRM_CalendarGroupBooking.ActualTeachers.ToString();

            txtFurtherDetails.Text = CRM_CalendarGroupBooking.Details;

            txtInitials.Text = CRM_CalendarGroupBooking.ConfirmationInitials;

            if (CRM_CalendarGroupBooking.ConfirmationDateSent != null)
            {
                txtConfirmationSent.Value = (DateTime)CRM_CalendarGroupBooking.ConfirmationDateSent;
            }
            
            if (CRM_CalendarGroupBooking.EventID != null)
                ddlEvent.SelectedValue = CRM_CalendarGroupBooking.EventID.ToString();

            if (CRM_CalendarGroupBooking.OfferID != null)
                ddlOffer.SelectedValue = CRM_CalendarGroupBooking.OfferID.ToString();

            if (CRM_CalendarGroupBooking.CRM_ExhibitionID != null)
                ddlOffer.SelectedValue = CRM_CalendarGroupBooking.CRM_ExhibitionID.ToString();
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
                CRM_CalendarGroupBooking = new CRM_CalendarGroupBooking();
                CRM_CalendarGroupBooking.CRM_PersonSchoolID = null; // obsolete
                CRM_CalendarGroupBooking.CRM_CalendarID = Entity.ID;
                CRM_CalendarGroupBooking.IsOutreach = false; // superseded by outreach calendar type
                db.CRM_CalendarGroupBookings.InsertOnSubmit(CRM_CalendarGroupBooking);
            }
            else
            {
                oldEntity = CRM_CalendarGroupBooking.ShallowCopy();
            }


            CRM_CalendarGroupBooking.IsBookshopRequired = chkIsBookshopRequired.Checked;
            CRM_CalendarGroupBooking.Within2HourSlot = chkIsWithin2Hours.Checked;
            CRM_CalendarGroupBooking.IsPackedLunchSpace = chkRequiresLunch.Checked;
            CRM_CalendarGroupBooking.ConfirmationSent = chkConfirmationSent.Checked;
            CRM_CalendarGroupBooking.SpecialNeeds = Convert.ToInt32(txtSpecialNeeds.Text);
            CRM_CalendarGroupBooking.Phone = txtPhone.Text;
            CRM_CalendarGroupBooking.Email = txtEmail.Text;
            CRM_CalendarGroupBooking.Pupils = Convert.ToInt32(txtPupils.Text);
            CRM_CalendarGroupBooking.Adults = Convert.ToInt32(txtAdults.Text);
            CRM_CalendarGroupBooking.Yeargroup = txtYearGroup.Text;

            IAutocomplete org = new AutoCompleteManager().GetIAutocompleteByReference(acSchoolOrg.SelectedID);

            CRM_CalendarGroupBooking.SchoolOrgReference = org.Reference;
            CRM_CalendarGroupBooking.OrganisationName = org.Name;
            CRM_CalendarGroupBooking.CRM_SchoolID = null;

            CRM_CalendarGroupBooking.ActualAdults = Convert.ToInt32(txtAttendedAdults.Text);
            CRM_CalendarGroupBooking.ActualChildren = Convert.ToInt32(txtAttendedChildren.Text);
            CRM_CalendarGroupBooking.ActualTeachers = Convert.ToInt32(txtAttendedTeachers.Text);

            if (acFacilitator.SelectedID != "")
                CRM_CalendarGroupBooking.Facilitator = acFacilitator.SelectedID;
            else
                CRM_CalendarGroupBooking.Facilitator = String.Empty;

            if (acFacilitator2.SelectedID != "")
                CRM_CalendarGroupBooking.FacilitatorTwo = acFacilitator2.SelectedID;
            else
                CRM_CalendarGroupBooking.FacilitatorTwo = String.Empty;

            if (acFacilitator3.SelectedID != "")
                CRM_CalendarGroupBooking.FacilitatorThree = acFacilitator3.SelectedID;
            else
                CRM_CalendarGroupBooking.FacilitatorThree = String.Empty;

            if (ddlEvent.SelectedValue != "")
                CRM_CalendarGroupBooking.EventID = Convert.ToInt32(ddlEvent.SelectedValue);
            else
                CRM_CalendarGroupBooking.EventID = null;

            if (ddlOffer.SelectedValue != "")
                CRM_CalendarGroupBooking.OfferID = Convert.ToInt32(ddlOffer.SelectedValue);
            else
                CRM_CalendarGroupBooking.OfferID = null;

            if (ddlExhibition.SelectedValue != "")
                CRM_CalendarGroupBooking.CRM_ExhibitionID = Convert.ToInt32(ddlExhibition.SelectedValue);
            else
                CRM_CalendarGroupBooking.CRM_ExhibitionID = null;

            if (txtConfirmationSent.Text.Length != 0)
                CRM_CalendarGroupBooking.ConfirmationDateSent = txtConfirmationSent.Value;
            else
                CRM_CalendarGroupBooking.ConfirmationDateSent = null;

            CRM_CalendarGroupBooking.ConfirmationInitials = txtInitials.Text;

            CRM_CalendarGroupBooking.Details = txtFurtherDetails.Text;

            db.SubmitChanges();

            if (oldEntity != null)
            {
                CRM.Code.History.History.RecordLinqUpdate(AdminUser, oldEntity, CRM_CalendarGroupBooking);
                db.SubmitChanges();
            }
            else
            {
                CRM.Code.History.History.RecordLinqInsert(AdminUser, CRM_CalendarGroupBooking);
            }
        }


    }
}