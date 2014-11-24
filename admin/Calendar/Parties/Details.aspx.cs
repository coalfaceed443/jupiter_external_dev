using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin.Calendar;

using CRM.Code.Helpers;
using CRM.Code.Models;
using CRM.Code.Interfaces;
using CRM.Code.Managers;

namespace CRM.admin.Calendar.Parties
{
    public partial class Details : CRM_CalendarPage<CRM_Calendar>
    {
        protected CRM_CalendarParty CRM_CalendarParty;
        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);

            CRM_CalendarParty = Entity.CRM_CalendarParties.FirstOrDefault();

            ucLogHistory.IHistory = CRM_CalendarParty;
            ucLogNotes.INotes = Entity;
            ucNavCal.Entity = Entity;

            CRMContext = Entity;

            // buttons //

            btnSubmit.EventHandler = btnSubmit_Click;
            btnSubmitChanges.EventHandler = btnSubmitChanges_Click;

            // Security //

            btnSubmitChanges.Visible = PermissionManager.CanUpdate;

            if (!PermissionManager.CanAdd && CRM_CalendarParty == null)
                Response.Redirect(Entity.DetailsURL);

            if (!Page.IsPostBack)
            {
                ddlAgeOnBirthday.DataSource = CRM_CalendarParty.GetChildrensAgeList();
                ddlAgeOnBirthday.DataBind();

                if (CRM_CalendarParty != null)
                {
                    PopulateFields();
                }
            }
        }


        private void PopulateFields()
        {
            ddlAgeOnBirthday.SelectedValue = CRM_CalendarParty.AgeOnBirthday.ToString();
            txtTheme.Text = CRM_CalendarParty.Theme;
            txtChildren.Text = CRM_CalendarParty.NumberOfChildren.ToString();
            txtAdults.Text = CRM_CalendarParty.AdditionalAdults.ToString();
            txtCateringPrice.Text = CRM_CalendarParty.CateringPrice.ToString("N2");
            txtCateringRequirements.Text = CRM_CalendarParty.CateringRequirements;
            chkAgreedToTerms.Checked = CRM_CalendarParty.AgreedToTerms;
            txtAdditionalEmail.Text = CRM_CalendarParty.AdditionalEmail;
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
                CRM_CalendarParty = new CRM_CalendarParty();
                CRM_CalendarParty.CRM_CalendarID = Entity.ID;
                db.CRM_CalendarParties.InsertOnSubmit(CRM_CalendarParty);
            }
            else
            {
                oldEntity = CRM_CalendarParty.ShallowCopy();
            }

            CRM_CalendarParty.AgeOnBirthday = Convert.ToInt32(ddlAgeOnBirthday.SelectedValue);
            CRM_CalendarParty.Theme = txtTheme.Text;
            CRM_CalendarParty.NumberOfChildren = Convert.ToInt32(txtChildren.Text);
            CRM_CalendarParty.AdditionalAdults = Convert.ToInt32(txtAdults.Text);
            CRM_CalendarParty.CateringPrice = Convert.ToDecimal(txtCateringPrice.Text);
            CRM_CalendarParty.CateringRequirements = txtCateringRequirements.Text;
            CRM_CalendarParty.AgreedToTerms = chkAgreedToTerms.Checked;
            CRM_CalendarParty.AdditionalEmail = txtAdditionalEmail.Text;

            db.SubmitChanges();

            if (oldEntity != null)
            {
                CRM.Code.History.History.RecordLinqUpdate(AdminUser, oldEntity, CRM_CalendarParty);
                db.SubmitChanges();
            }
            else
            {
                CRM.Code.History.History.RecordLinqInsert(AdminUser, CRM_CalendarParty);
            }
        }


    }
}