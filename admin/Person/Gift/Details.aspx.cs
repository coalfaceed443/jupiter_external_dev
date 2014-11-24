using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin.Persons;
using CRM.Code.Models;
using CRM.Code.Managers;
using CRM.Code.Utils.Enumeration;
using CRM.Code.Utils.Time;

namespace CRM.admin.Person.Gift
{
    public partial class Details : CRM_PersonPage<CRM_FundraisingGiftProfile>
    {
        protected CRM.Code.Models.CRM_FundraisingGiftProfile CRM_FundraisingGiftProfile = null;

        protected void Page_Load(object sender, EventArgs e)
        {

            CRM_FundraisingGiftProfile = Entity.CRM_FundraisingGiftProfiles.SingleOrDefault(c => c.ID.ToString() == Request.QueryString["pid"]);

            btnSubmit.EventHandler = btnSubmit_Click;
            btnSubmitChanges.EventHandler = btnSubmitChanges_Click;
            btnDelete.EventHandler = btnDelete_Click;
            btnReinstate.EventHandler = btnReinstate_Click;

            // confirmations //

            confirmationDelete.StandardDeleteHidden("gift profile", btnRealDelete_Click);

            // process //


            CRMContext = CRM_FundraisingGiftProfile;
            ucLogNotes.INotes = CRM_FundraisingGiftProfile;
            ucNavPerson.Entity = Entity;

            if (!IsPostBack)
            {
                FormDropdowns();

                if (CRM_FundraisingGiftProfile != null)
                    PopulateFields();
                else
                {
                    pnlGiftAidLogs.Visible = false;
                    txtStartDate.Value = UKTime.Now;
                    txtEndDate.Value = UKTime.Now.AddYears(1);
                    pnlNextDate.Visible = false;
                }
            }
        }

        private void FormDropdowns()
        {
            int daysInMonth = 28;

            List<ListItem> days = new List<ListItem>();
            for (int i = 1; i <= daysInMonth; i++)
                days.Add(new ListItem(i.ToString()));

            int intervalLimit = 24;

            List<ListItem> monthlyIntervals = new List<ListItem>();
            for (int i = 1; i <= intervalLimit; i++)
                monthlyIntervals.Add(new ListItem(i.ToString()));

            ddlDayOfMonth.DataSource = days;
            ddlDayOfMonth.DataBind();
            ddlMonthlyIntervals.DataSource = monthlyIntervals;
            ddlMonthlyIntervals.DataBind();

        }

        private void PopulateFields()
        {
            var logs = CRM_FundraisingGiftProfile.CRM_FundraisingGiftProfileLogs.OrderByDescending(c => c.DateCreatedOutput);
            rptGiftLog.DataSource = logs;
            rptGiftLog.DataBind();

            pnlNoEntries.Visible = !logs.Any();

            ucLogHistory.IHistory = CRM_FundraisingGiftProfile;
            ucLogHistory.ParentID = Entity.ID.ToString();

            txtName.Text = CRM_FundraisingGiftProfile.ProfileName;
            chkIsActive.Checked = CRM_FundraisingGiftProfile.IsActive;
            txtAmountToCharge.Text = CRM_FundraisingGiftProfile.AmountToCharge.ToString("N2");
            txtStartDate.Value = CRM_FundraisingGiftProfile.StartDate;
            txtEndDate.Value = CRM_FundraisingGiftProfile.EndDate;
            ddlDayOfMonth.SelectedValue = CRM_FundraisingGiftProfile.DayOfMonth.ToString();
            ddlMonthlyIntervals.SelectedValue = CRM_FundraisingGiftProfile.EveryXMonth.ToString();
            txtNextDate.Value = CRM_FundraisingGiftProfile.NextPaymentDate;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(true);

                NoticeManager.SetMessage("Item Added", "details.aspx?id=" + Entity.ID + "&pid=" + CRM_FundraisingGiftProfile.ID);
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
                CRM_FundraisingGiftProfile = new CRM_FundraisingGiftProfile();
                CRM_FundraisingGiftProfile.IsArchived = false;
                CRM_FundraisingGiftProfile.CRM_PersonID = Entity.ID;
                db.CRM_FundraisingGiftProfiles.InsertOnSubmit(CRM_FundraisingGiftProfile);
            }
            else
            {
                oldEntity = CRM_FundraisingGiftProfile.ShallowCopy();
            }

            CRM_FundraisingGiftProfile.ProfileName = txtName.Text;
            CRM_FundraisingGiftProfile.IsActive = chkIsActive.Checked;
            CRM_FundraisingGiftProfile.AmountToCharge = Convert.ToDecimal(txtAmountToCharge.Text);
            CRM_FundraisingGiftProfile.StartDate = txtStartDate.Value;
            CRM_FundraisingGiftProfile.EndDate = txtEndDate.Value;
            CRM_FundraisingGiftProfile.DayOfMonth = Convert.ToInt32(ddlDayOfMonth.SelectedValue);
            CRM_FundraisingGiftProfile.EveryXMonth = Convert.ToInt32(ddlMonthlyIntervals.SelectedValue);
            CRM_FundraisingGiftProfile.PaymentReference = txtPaymentReference.Text;

            if (newRecord)
                CRM_FundraisingGiftProfile.NextPaymentDate = txtStartDate.Value;
            else
                CRM_FundraisingGiftProfile.NextPaymentDate = txtNextDate.Value;
            
            db.SubmitChanges();

            if (oldEntity != null)
            {
                CRM.Code.History.History.RecordLinqUpdate(db, AdminUser, oldEntity, CRM_FundraisingGiftProfile);
                db.SubmitChanges();
            }
            else
            {
                CRM.Code.History.History.RecordLinqInsert(AdminUser, CRM_FundraisingGiftProfile);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            confirmationDelete.ShowPopup();
        }


        protected void btnRealDelete_Click(object sender, EventArgs e)
        {
            object oldEntity = CRM_FundraisingGiftProfile.ShallowCopy();

            CRM_FundraisingGiftProfile.IsArchived = true;
            db.SubmitChanges();

            CRM.Code.History.History.RecordLinqUpdate(db, AdminUser, oldEntity, CRM_FundraisingGiftProfile);
            db.SubmitChanges();

            NoticeManager.SetMessage("Item removed", "list.aspx");
        }

        protected void btnReinstate_Click(object sender, EventArgs e)
        {
            CRM_FundraisingGiftProfile.IsArchived = false;
            db.SubmitChanges();

            NoticeManager.SetMessage("Item reinstated");
        }

        protected void cusNextDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (CRM_FundraisingGiftProfile != null)
                args.IsValid = txtNextDate.Value >= txtStartDate.Value && txtNextDate.Value <= txtEndDate.Value;
            else
                args.IsValid = true;
        }
    }
}