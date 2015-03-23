using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Managers;
using CRM.Code.Models;
using CRM.Code.Abstracts;
using CRM.Code.Interfaces;
using CRM.Code.Utils.WebControl;
using CRM.Code.Helpers;
using CRM.Controls.Admin.SharedObjects;
using CRM.Controls.Forms;
using CRM.Code.Utils.Time;

namespace CRM.admin.Fundraising
{
    public partial class Details : AdminPage
    {
        protected CRM.Code.Models.CRM_Fundraising Entity = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            pnlImportPrompt.Visible = false;
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);
            Entity = db.CRM_Fundraisings.SingleOrDefault(c => c.ID.ToString() == Request.QueryString["id"]);

            btnSubmitChanges.Visible = PermissionManager.CanUpdate;
            if (!PermissionManager.CanAdd && Entity == null)
                Response.Redirect("list.aspx");

            ucCustomFields._DataTableID = db._DataTables.Single(c => c.TableReference == "CRM_Fundraising").ID;
            // buttons //

            btnSubmit.EventHandler = btnSubmit_Click;
            btnSubmitChanges.EventHandler = btnSubmitChanges_Click;
            btnDelete.EventHandler = btnDelete_Click;

            ucACPrimaryContact.EventHandler = lnkSelect_Click;
            ucACPrimaryContact.Config = new AutoCompleteConfig(JSONSet.DataSets.contact);

            // Security //

            btnSubmitChanges.Visible = PermissionManager.CanUpdate;
            btnDelete.Visible = PermissionManager.CanDelete;
            if (!PermissionManager.CanAdd && Entity == null)
                Response.Redirect("list.aspx");

            // confirmations //

            confirmationDelete.StandardDeleteHidden("donation", btnRealDelete_Click);

            CRMContext = Entity; 
            ucLogNotes.INotes = Entity;

            // process //

            if (!IsPostBack)
            {
                Session.Remove(SplitListConst);
                ddlPaymentType.DataSource = CRM_PaymentType.SetDropDown(db.CRM_PaymentTypes.OrderBy(o => o.OrderNo).Cast<IArchivable>(),
                                                                            Entity == null ? null : Entity.CRM_PaymentType);
                ddlPaymentType.DataBind();

                ddlFundReason.DataSource = CRM_FundraisingReason.SetDropDown(db.CRM_FundraisingReasons.OrderBy(o => o.OrderNo).Cast<IArchivable>(),
                                                                                Entity == null ? null : Entity.CRM_FundraisingReason);
                ddlFundReason.DataBind();

                BindFundDDL(ddlFund);

                if (Entity != null)
                {
                    PopulateFields();
                    SplitList = Entity.CRM_FundraisingSplits.ToList();
                    
                    RebindSplitList();

                }

            }
        }

        protected void BindFundDDL(DropDownList ddl)
        {
            ddl.DataSource = from d in CRM_Fund.BaseSet(db)
                             orderby d.OrderNo
                             select d;
            ddl.DataBind();
        }

        protected void lnkSelect_Click(object sender, EventArgs e)
        {
            IContact contact = new ContactManager().Contacts.Single(v => v.Reference.ToString() == ucACPrimaryContact.SelectedID);
            ucACPrimaryContact.Populate(contact);

            if (contact.Parent_CRM_Person.IsGiftAid)
                pnlImportIsGift.Visible = true;
            else
                pnlImportPrompt.Visible = true;

        }

        protected void lnkPopulateContact_Click(object sender, EventArgs e)
        {
            IContact contact = new ContactManager().Contacts.Single(v => v.Reference.ToString() == ucACPrimaryContact.SelectedID);
            txtGiftaidFirstname.Text = contact.Firstname;
            txtGiftaidLastname.Text = contact.Lastname;
            ucGiftAidAddress.Populate(contact.PrimaryAddress);
            chkIsGiftAid.Checked = true;

            pnlImportIsGift.Visible = false;
            pnlImportPrompt.Visible = false;
        }

        private void PopulateFields()
        {
            chkIsInKind.Checked = Entity.IsInKind;
            txtAmount.Text = Entity.PledgedAmount.ToString("N2");
            chkIsGiftAid.Checked = Entity.IsGiftAid;
            txtGiftaidFirstname.Text = Entity.GiftAidFirstname;
            txtGiftaidLastname.Text = Entity.GiftAidLastname;
            ucGiftAidAddress.Populate(Entity.CRM_Address);
            chkIsRecurring.Checked = Entity.IsRecurring;
            txtRecurringWeeks.Text = Entity.RecurringEveryWeeks.ToString();
            txtDuration.Text = Entity.Duration.ToString();
            ddlPaymentType.SelectedValue = Entity.CRM_PaymentTypeID.ToString();
            ddlFundReason.SelectedValue = Entity.CRM_FundraisingReasonID.ToString();

            ucACPrimaryContact.Populate(new ContactManager().Contacts.Single(c => c.Reference == Entity.PrimaryContactReference));


            ucCustomFields._DataTableID = db._DataTables.Single(c => c.TableReference == "CRM_Fundraising").ID;
            ucCustomFields.Populate(Entity.Reference);

            mvSplit.SetActiveView(viewSplit);
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
            object oldAddress = null;


            if (newRecord)
            {
                Entity = new CRM_Fundraising();
                db.CRM_Fundraisings.InsertOnSubmit(Entity);
            }
            else
            {
                oldEntity = Entity.ShallowCopy();
                oldAddress = Entity.CRM_Address.ShallowCopy();
            }


            int recurring = 0;

            Int32.TryParse(txtRecurringWeeks.Text, out recurring);

            Entity.IsInKind = chkIsInKind.Checked;
            Entity.PledgedAmount = Convert.ToDecimal(txtAmount.Text);
            Entity.IsGiftAid = chkIsGiftAid.Checked;
            Entity.GiftAidFirstname = txtGiftaidFirstname.Text;
            Entity.GiftAidLastname = txtGiftaidLastname.Text;
            Entity.IsRecurring = chkIsRecurring.Checked;
            Entity.RecurringEveryWeeks = recurring;
            Entity.Duration = Convert.ToInt32(txtDuration.Text);
            Entity.CRM_PaymentTypeID = Convert.ToInt32(ddlPaymentType.SelectedValue);
            Entity.CRM_FundraisingReasonID = Convert.ToInt32(ddlFundReason.SelectedValue);
            Entity.PrimaryContactReference = ucACPrimaryContact.SelectedID;

            using (ContactManager manager = new ContactManager())
            {
                IContact selectedContact = new ContactManager().Contacts.Single(c => c.Reference == ucACPrimaryContact.SelectedID);

                Entity.ContactName = selectedContact.Fullname;
                Entity.ContactType = selectedContact.DisplayName;
                manager.Dispose();
            }

            if (newRecord)
            {
                CRM_Address address = new CRM_Address();
                Entity.CRM_Address = (CRM_Address)((Address)ucGiftAidAddress).Save(address);
            }
            else
            {
                Entity.CRM_Address = (CRM_Address)((Address)ucGiftAidAddress).Save(Entity.CRM_Address);
            }

            db.SubmitChanges();

            db.CRM_FundraisingSplits.DeleteAllOnSubmit(Entity.CRM_FundraisingSplits);
            db.SubmitChanges();

            foreach (CRM_FundraisingSplit serialSplit in GetSplitsFromRepeater())
            {
                CRM_FundraisingSplit split = new CRM_FundraisingSplit()
                {
                    Amount = serialSplit.Amount,
                    CRM_FundID = serialSplit.CRM_FundID,
                    DateGiven = serialSplit.DateGiven,
                    GiftAidRate = serialSplit.GiftAidRate,
                    CRM_FundRaisingID = Entity.ID
                };

                db.CRM_FundraisingSplits.InsertOnSubmit(split);
                db.SubmitChanges();
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

            Session.Remove(SplitListConst);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            confirmationDelete.ShowPopup();
        }


        protected void btnRealDelete_Click(object sender, EventArgs e)
        {
            Entity.IsArchived = true;
            db.SubmitChanges();

            NoticeManager.SetMessage("Item removed", "list.aspx");
        }

        private string DataIndex = "data-index";
        protected void rptSplits_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            CRM_FundraisingSplit split = (CRM_FundraisingSplit)e.Item.DataItem;

            DropDownList ddlFund = (DropDownList)e.Item.FindControl("ddlFund");
            TextBox txtPrice = (TextBox)e.Item.FindControl("txtPrice");
            TextBox txtGiftAidRate = (TextBox)e.Item.FindControl("txtGiftAidRate");
            UserControlDateCalendar txtDateGiven = (UserControlDateCalendar)e.Item.FindControl("txtDateGiven");
            LinkButton lnkRemove = (LinkButton)e.Item.FindControl("lnkRemove");

            BindFundDDL(ddlFund);

            ddlFund.SelectedValue = split.CRM_FundID.ToString();
            lnkRemove.Attributes[DataIndex] = e.Item.ItemIndex.ToString();
            txtPrice.Text = split.Amount.ToString("N2");
            txtDateGiven.Value = split.DateGiven;
            txtGiftAidRate.Text = split.GiftAidRate.ToString("N0");
        }

        protected void lnkRemove_Click(object sender, EventArgs e)
        {
            SplitList = GetSplitsFromRepeater();

            int myindex = Convert.ToInt32(((LinkButton)sender).Attributes[DataIndex]);
            SplitList.RemoveAt(myindex);
            SplitList = SplitList;
            RebindSplitList();
        }

        private string SplitListConst
        {
            get
            {
                return this.Page.UniqueID;
            }
        }
        private List<CRM_FundraisingSplit> SplitList
        {
            get
            {
                if (Session[SplitListConst] == null)
                {
                    Session[SplitListConst] = new List<CRM_FundraisingSplit>()
                        {
                            GetDefaultSplit(Convert.ToInt32(db.CRM_Funds.OrderBy(f => f.OrderNo).First().ID))
                        };
                }

                return (List<CRM_FundraisingSplit>)Session[SplitListConst];
            }
            set
            {
                Session[SplitListConst] = value;
            }
        }

        protected void ddlFund_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlFund.SelectedValue != "")
            {
                SplitList = GetSplitsFromRepeater();
                
                SplitList.Add(GetDefaultSplit(Convert.ToInt32(ddlFund.SelectedValue)));
                SplitList = SplitList;

                RebindSplitList();

                mvSplit.SetActiveView(viewSplit);
                ddlFund.SelectedValue = "";
            }
        }

        protected List<CRM_FundraisingSplit> GetSplitsFromRepeater()
        {
            List<CRM_FundraisingSplit> splitList = new List<CRM_FundraisingSplit>();
            foreach (RepeaterItem item in rptSplits.Items)
            {
                DropDownList ddlFund = (DropDownList)item.FindControl("ddlFund");
                TextBox txtPrice = (TextBox)item.FindControl("txtPrice");
                TextBox txtGiftAidRate = (TextBox)item.FindControl("txtGiftAidRate");
                UserControlDateCalendar txtDateGiven = (UserControlDateCalendar)item.FindControl("txtDateGiven");

                CRM_FundraisingSplit split = new CRM_FundraisingSplit()
                {
                    Amount = Convert.ToDecimal(txtPrice.Text),
                    CRM_FundID = Convert.ToInt32(ddlFund.SelectedValue),
                    DateGiven = txtDateGiven.Value,
                    GiftAidRate = Convert.ToDecimal(txtGiftAidRate.Text)
                };

                splitList.Add(split);
            }

            return splitList;
        }


        private void RebindSplitList()
        {
            rptSplits.DataSource = SplitList;
            rptSplits.DataBind();
        }

        protected CRM_FundraisingSplit GetDefaultSplit(int DefaultFundID)
        {
            return new CRM_FundraisingSplit()
                    {
                        CRM_Fund = db.CRM_Funds.Single(f => f.ID == DefaultFundID),
                        CRM_FundID = DefaultFundID,
                        Amount = 0,
                        DateGiven = UKTime.Now,
                        GiftAidRate = 0.20M
                    };
        }

    }
}