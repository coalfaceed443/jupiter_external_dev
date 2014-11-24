using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Managers;
using CRM.Code.Models;
using CRM.Code.BasePages.Admin.Calendar;

using CRM.Code.Helpers;
using CRM.Code.Utils.Time;
using CRM.Code.Interfaces;
using CRM.Code.Utils.WebControl;

namespace CRM.admin.Calendar.Finance
{
    public partial class Details : CRM_CalendarPage<CRM_Calendar>
    {
        ContactManager ContactManager;
        protected void Page_Load(object sender, EventArgs e)
        {
            ContactManager = new Code.Managers.ContactManager();
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);
       
            btnSubmitChanges.Visible = PermissionManager.CanUpdate;
            if (!PermissionManager.CanAdd && Entity == null)
                Response.Redirect("list.aspx");

            ucNavCal.Entity = Entity;
            ucLogNotes.INotes = Entity;

            CRMContext = Entity;
            // buttons //

            btnSubmitChanges.EventHandler = btnSubmitChanges_Click;

            // Security //

            btnSubmitChanges.Visible = PermissionManager.CanUpdate;
            if (!PermissionManager.CanAdd && Entity == null)
                Response.Redirect("list.aspx");


            // process //

            ucAddCustomer.Config = new AutoCompleteConfig(JSONSet.DataSets.contact);
            ucAddCustomer.EventHandler = lnkSelectCustomer_Click;

            if (!IsPostBack)
            {

                ddlTitles.DataSource = CRM_Title.SetDropDownWithString(db.CRM_Titles.Cast<IArchivable>(), String.Empty);
                ddlTitles.DataBind();
                PopulateFields();



                ReloadCalPerHead();
            }

        }

        protected void ReloadCalPerHead()
        {

            List<CRM_CalendarPerHead> headList = new List<CRM_CalendarPerHead>();
            headList.AddRange(Entity.CRM_CalendarPerHeads);

            rptCustomersPerHead.DataSource = headList;
            rptCustomersPerHead.DataBind();

            ucAddCustomer.SwitchToInput();
        }

        protected void lnkSelectCustomer_Click(object sender, EventArgs e)
        {
            decimal defaultPrice = 0M;
            
            Decimal.TryParse(txtPriceAgreed.Text, out defaultPrice);

            CRM_CalendarPerHead calHead = new CRM_CalendarPerHead()
            {
                ChildrenPrice = 0,
                CRM_CalendarID = Entity.ID,
                PlusChildren = 0,
                Price = defaultPrice,
                TargetReference = ucAddCustomer.SelectedID
            };

            db.CRM_CalendarPerHeads.InsertOnSubmit(calHead);
            db.SubmitChanges();

            ReloadCalPerHead();
        }

        private void PopulateFields()
        {

            ddlTitles.DataSource = CRM_Title.SetDropDownWithString(db.CRM_Titles.Cast<IArchivable>(), Entity.InvoiceTitle);
            ddlTitles.DataBind();

            txtPriceAgreed.Text = Entity.PriceAgreed.ToString("N2");
            ddlPriceType.SelectedValue = Entity.PriceType.ToString();
            txtFirstname.Text = Entity.InvoiceFirstname;
            txtLastname.Text = Entity.InvoiceLastname;
            ddlTitles.SelectedValue = Entity.InvoiceTitle;
            ucLogNotes.INotes = Entity;
            ucLogHistory.IHistory = Entity;
            ucLogHistory.ParentID = Entity.ID.ToString();
            txtPONumber.Text = Entity.PONumber;


            if (Entity.DatePaid != null)
            {
                txtDatePaid.Value = (DateTime)Entity.DatePaid;
            }

            if (Entity.InvoiceAddressID == null && Entity.PrimaryContactReference != "")
            {
                IContact Contact = ContactManager.GetIContactByReference(Entity.PrimaryContactReference);

                ucAddress.Populate(Contact.PrimaryAddress);

                ddlTitles.DataSource = CRM_Title.SetDropDownWithString(db.CRM_Titles.Cast<IArchivable>(), Contact.Title);
                ddlTitles.DataBind();

                ddlTitles.SelectedValue = Contact.Title;
                txtFirstname.Text = Contact.Firstname;
                txtLastname.Text = Contact.Lastname;
            }
            else
            {

                
                CRM_Address invoiceAddress = db.CRM_Addresses.SingleOrDefault(c => c.ID == Entity.InvoiceAddressID);

                if (invoiceAddress != null)
                {
                    ucAddress.Populate(invoiceAddress);
                }
                else
                {
                    IContact contact = new Code.Managers.ContactManager().Contacts.SingleOrDefault(c => c.Reference == Entity.PrimaryContactReference);
                    
                    
                    if (contact != null && contact.PrimaryAddress != null)
                        ucAddress.Populate(contact.PrimaryAddress);
                }
                
                ddlTitles.SelectedValue = Entity.InvoiceTitle;
                txtFirstname.Text = Entity.InvoiceFirstname;
                txtLastname.Text = Entity.InvoiceLastname;
            }



        }

        protected void btnSubmitChanges_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(false);

                NoticeManager.SetMessage("Invoice Updated");
            }
        }

        protected void SaveRecord(bool newRecord)
        {
            // new record / exiting record //

            object oldEntity = Entity.ShallowCopy();

            Entity.PriceAgreed = Convert.ToDecimal(txtPriceAgreed.Text);
            Entity.PriceType = Convert.ToByte(ddlPriceType.SelectedValue);
            Entity.InvoiceFirstname = txtFirstname.Text;
            Entity.InvoiceLastname = txtLastname.Text;
            Entity.InvoiceTitle = ddlTitles.SelectedValue;
            Entity.PONumber = txtPONumber.Text;
            if (txtDatePaid.Text.Length != 0)
            {
                Entity.DatePaid = txtDatePaid.Value;
            }

            db.SubmitChanges();

            if (oldEntity != null)
            {
                CRM.Code.History.History.RecordLinqUpdate(db, AdminUser, oldEntity, Entity);
                db.SubmitChanges();
            }

            if (Entity.InvoiceAddressID == null)
            {
                Entity.CRM_Address = (CRM_Address)ucAddress.Save(new CRM_Address());
            }
            else
            {
                Entity.CRM_Address = (CRM_Address)ucAddress.Save((IAddress)db.CRM_Addresses.Single(c => c.ID == Entity.InvoiceAddressID));
            }
            
            
            db.SubmitChanges();

            foreach (RepeaterItem item in rptCustomersPerHead.Items)
            {
                TextBox txtPrice = (TextBox)item.FindControl("txtPrice");
                TextBox txtPlusChildren = (TextBox)item.FindControl("txtPlusChildren");
                TextBox txtChildrenPrice = (TextBox)item.FindControl("txtChildrenPrice");
                LinkButton lnkRemove = (LinkButton)item.FindControl("lnkRemove");
                string id = lnkRemove.CommandArgument;

                CRM_CalendarPerHead calPerHead = db.CRM_CalendarPerHeads.Single(c => c.ID.ToString() == id);

                decimal price = calPerHead.Price;
                int plusChildren = calPerHead.PlusChildren;
                decimal childrenPrice = calPerHead.ChildrenPrice;

                bool okPrice = Decimal.TryParse(txtPrice.Text, out price);
                bool okChildren = Int32.TryParse(txtPlusChildren.Text, out plusChildren);
                bool okChildrenPrice = Decimal.TryParse(txtChildrenPrice.Text, out childrenPrice);

                calPerHead.Price = price;
                calPerHead.PlusChildren = plusChildren;
                calPerHead.ChildrenPrice = childrenPrice;
                db.SubmitChanges();
            }            
        }

        protected void rptCustomersPerHead_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            CRM_CalendarPerHead calPerHead = (CRM_CalendarPerHead)e.Item.DataItem;

            Label lblName = (Label)e.Item.FindControl("lblName");
            TextBox txtPrice = (TextBox)e.Item.FindControl("txtPrice");
            TextBox txtPlusChildren = (TextBox)e.Item.FindControl("txtPlusChildren");
            TextBox txtChildrenPrice = (TextBox)e.Item.FindControl("txtChildrenPrice");
            LinkButton lnkRemove = (LinkButton)e.Item.FindControl("lnkRemove");

            lblName.Text = ContactManager.GetIContactByReference(calPerHead.TargetReference).Fullname;
            txtPrice.Text = calPerHead.Price.ToString("N2");
            txtPlusChildren.Text = calPerHead.PlusChildren.ToString();
            txtChildrenPrice.Text = calPerHead.ChildrenPrice.ToString("N2");
            lnkRemove.CommandArgument = calPerHead.ID.ToString();
        }

        protected void lnkRemove_Click(object sender, EventArgs e)
        {
            string id = ((LinkButton)sender).CommandArgument;
            CRM_CalendarPerHead calPerHead = db.CRM_CalendarPerHeads.Single(s => s.ID.ToString() == id);
            db.CRM_CalendarPerHeads.DeleteOnSubmit(calPerHead);
            db.SubmitChanges();

            ReloadCalPerHead();
        }


    }
}