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
using CRM.Code;

namespace CRM.admin.Calendar.CPD
{
    public partial class Details : CRM_CalendarPage<CRM_Calendar>
    {
        protected CRM_CalendarCPD CRM_CalendarCPD;
        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);

            CRM_CalendarCPD = Entity.CRM_CalendarCPDs.FirstOrDefault();

            ucLogHistory.IHistory = CRM_CalendarCPD;
            ucLogNotes.INotes = Entity;
            ucNavCal.Entity = Entity;


            CRMContext = Entity;

            // buttons //

            btnSubmit.EventHandler = btnSubmit_Click;
            btnSubmitChanges.EventHandler = btnSubmitChanges_Click;

            // Security //

            btnSubmitChanges.Visible = PermissionManager.CanUpdate;

            ucACSchoolOrganisation.EventHandler = lnkSelectOrganisation_Click;
            ucACSchoolOrganisation.Config = new AutoCompleteConfig(JSONSet.DataSets.schoolorgs);

            if (!PermissionManager.CanAdd && CRM_CalendarCPD == null)
                Response.Redirect(Entity.DetailsURL);

            if (!Page.IsPostBack)
            {
                ddlLength.DataSource = CRM_CalendarCPD.GetDurationLengths();
                ddlLength.DataBind();

                if (CRM_CalendarCPD != null)
                {
                    ddlPackage.DataSource = from p in db.CRM_Packages
                                            where p.IsActive || CRM_CalendarCPD.CRM_PackageID == p.ID
                                            orderby p.Name
                                            select p;

                    ddlPackage.DataBind();

                    PopulateFields();
                }
                else
                {

                    ddlPackage.DataSource = from p in db.CRM_Packages
                                            where p.IsActive
                                            orderby p.Name
                                            select p;

                    ddlPackage.DataBind();
                }
            }
        }

        protected void lnkSelectOrganisation_Click(object sender, EventArgs e)
        {
            AutoCompleteManager amManager = new AutoCompleteManager();           
            IAutocomplete org = amManager.GetIAutocompleteByReference(ucACSchoolOrganisation.SelectedID);
            ucACSchoolOrganisation.Populate(org);
        }

        private void PopulateFields()
        {
            AutoCompleteManager amManager = new AutoCompleteManager();
            IAutocomplete org = amManager.GetIAutocompleteByReference(CRM_CalendarCPD.SchoolOrganisationReference);
            ucACSchoolOrganisation.Populate(org);
            txtAttendees.Text = CRM_CalendarCPD.Attendees.ToString();

            if (CRM_CalendarCPD.ConfirmationSent != null)
            {
                txtConfirmationSent.Value = (DateTime)CRM_CalendarCPD.ConfirmationSent;
            }

            ddlLength.SelectedValue = CRM_CalendarCPD.Length.ToString();
            ddlPackage.SelectedValue = CRM_CalendarCPD.CRM_PackageID.ToString();
            txtInitials.Text = CRM_CalendarCPD.ConfirmationInitials;
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
                CRM_CalendarCPD = new CRM_CalendarCPD();
                CRM_CalendarCPD.CRM_CalendarID = Entity.ID;
                db.CRM_CalendarCPDs.InsertOnSubmit(CRM_CalendarCPD);
            }
            else
            {
                oldEntity = CRM_CalendarCPD.ShallowCopy();
            }


            CRM_CalendarCPD.Attendees = Convert.ToInt32(txtAttendees.Text);

            if (txtConfirmationSent.Text.Length != 0)
                CRM_CalendarCPD.ConfirmationSent = txtConfirmationSent.Value;
            else
                CRM_CalendarCPD.ConfirmationSent = null;

            CRM_CalendarCPD.SchoolOrganisationReference = ucACSchoolOrganisation.SelectedID;
            CRM_CalendarCPD.Length = Convert.ToByte(ddlLength.SelectedValue);
            CRM_CalendarCPD.CRM_PackageID = Convert.ToInt32(ddlPackage.SelectedValue);
            CRM_CalendarCPD.ConfirmationInitials = txtInitials.Text;
            db.SubmitChanges();

            if (oldEntity != null)
            {
                CRM.Code.History.History.RecordLinqUpdate(AdminUser, oldEntity, CRM_CalendarCPD);
                db.SubmitChanges();
            }
            else
            {
                CRM.Code.History.History.RecordLinqInsert(AdminUser, CRM_CalendarCPD);
            }
        }


    }
}