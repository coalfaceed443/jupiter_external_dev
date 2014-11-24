using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;
using CRM.Code.Interfaces;
using System.Web.Script.Serialization;
using CRM.Code.Managers;
using CRM.Code.Utils.Time;

namespace CRM.admin.Merge
{
    public partial class Details : AdminPage
    {
        protected HoldingPen Entity;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            Entity = db.HoldingPens.Single(c => c.ID.ToString() == Request.QueryString["id"]);


            Dictionary<byte, string> comparibles = new Dictionary<byte, string>();

            comparibles.Add((byte)CRM_Person.SearchKeys.Firstname, Entity.Firstname);
            comparibles.Add((byte)CRM_Person.SearchKeys.Lastname, Entity.Lastname);
            comparibles.Add((byte)CRM_Person.SearchKeys.PrimaryEmail, Entity.Email);
            comparibles.Add((byte)CRM_Person.SearchKeys.Postcode, Entity.Postcode);
            comparibles.Add((byte)CRM_Person.SearchKeys.Telephone, Entity.Telephone);


            if (!Page.IsPostBack)
            {

                lnkNextRecord.Text += " (" + (HoldingPen.BaseSet(db).Count() - 1 - HoldingPen.BaseSet(db).ToList().IndexOf(Entity)) + " remaining)";


                var dupes = from p in CRM_Person.BaseSet(db)
                            where p.SearchDictionary.Any(c => comparibles.ContainsKey(c.Key) && comparibles[c.Key].Trim() != "" && c.Value.ToLower().Trim() == (comparibles[c.Key]))
                            orderby p.SearchDictionary.Where(c => comparibles.ContainsKey(c.Key) && comparibles[c.Key].Trim() != "" && c.Value.ToLower().Trim() == (comparibles[c.Key])).Count() descending
                            select (object)p;

                lvItems.DataSource = dupes.Take(3);
                lvItems.DataBind();

                ddlTitle.DataSource = from p in db.CRM_Titles
                                      select p;
                ddlTitle.DataBind();

                PopulateFields();
            }

        }

        protected void PopulateFields()
        {
            txtFirstname.Text = Entity.Firstname;
            txtLastname.Text = Entity.Lastname;
            txtEmail.Text = Entity.Email;
            txtAddress1.Text = Entity.Address1;
            txtAddress2.Text = Entity.Address2;
            txtAddress3.Text = Entity.Address3;
            txtCity.Text = Entity.City;
            txtCounty.Text = Entity.County;
            txtPostcode.Text = Entity.Postcode;
            txtTelephone.Text = Entity.Telephone;

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            IEnumerable<ListItem> interests = db.CRM_FormFieldItems.Where(f => f.CRM_FormFieldID == 24 && f.IsActive && !f.IsArchived).Select(s => new ListItem(s.Label, s.ID.ToString()));


            var items = from p in serializer.Deserialize<IEnumerable<CRM.WebService.CRMSync.InterestAnswer>>(Entity.InterestObjects)
                        select p;

            rptInterests.DataSource = interests;
            rptInterests.DataBind();

            
            for (int i = 0; i < rptInterests.Items.Count; i++)
            {
                
                foreach (CRM.WebService.CRMSync.InterestAnswer displayItem in items)
                {

                    if (rptInterests.Items[i].Value == displayItem.FormFieldItemID.ToString() && displayItem.OptIn == true)
                        rptInterests.Items[i].Selected = true;
                }

            }

        }

        protected void lnkUpdate_Click(object sender, EventArgs e)
        {
            CRM.Code.Models.CRM_Person person = db.CRM_Persons.Single(s => s.ID.ToString() == ((LinkButton)sender).CommandArgument);

            litBefore.Text = person.MergeCard.Replace(Environment.NewLine, "<br/>");
            litAfter.Text = TransformPerson(person).MergeCard.Replace(Environment.NewLine, "<br/>");

            pnlMerge.Visible = true;

            hdnRecord.Value = person.ID.ToString();
        }

        protected CRM_Person TransformPerson (CRM_Person person)
        {

            if (chkTitle.Checked)
                person.Title = ddlTitle.SelectedItem.Text;

            if (chkFirstname.Checked)
                person.Firstname = txtFirstname.Text;

            if (chkLastname.Checked)
                person.Lastname = txtLastname.Text;

            if (chkEmail.Checked)
                person.PrimaryEmail = txtEmail.Text;

            if (chkAddress1.Checked)
                person.PrimaryAddress.AddressLine1 = txtAddress1.Text;

            if (chkAddress2.Checked)
                person.PrimaryAddress.AddressLine2 = txtAddress2.Text;

            if (chkAddress3.Checked)
                person.PrimaryAddress.AddressLine3 = txtAddress3.Text;

            if (chkCity.Checked)
                person.PrimaryAddress.Town = txtCity.Text;

            if (chkCounty.Checked)
                person.PrimaryAddress.County = txtCounty.Text;

            if (chkPostcode.Checked)
                person.PrimaryAddress.Postcode = txtPostcode.Text;

            if (chkTel.Checked)
                person.PrimaryTelephone = txtTelephone.Text;

            return person;

        }

        protected void btnSubmitChanges_Click(object sender, EventArgs e)
        {
            if (hdnRecord.Value != "")
            {

                object oldEntity = null;
                object oldAddress = null;

                CRM.Code.Models.CRM_Person person = db.CRM_Persons.Single(s => s.ID.ToString() == hdnRecord.Value);


                oldEntity = person.ShallowCopy();
                oldAddress = person.CRM_Address.ShallowCopy();

                TransformPerson(person);
                person.WebsiteAccountID = Entity.OriginID;
                if (Entity.DoNotMail != null)
                    person.IsDoNotMail = (bool)Entity.DoNotMail;

                if (Entity.DoNotEmail != null)
                    person.IsDoNotEmail = (bool)Entity.DoNotEmail;

                db.SubmitChanges();

                CRM.Code.History.History.RecordLinqUpdate(AdminUser, oldEntity, person);
                CRM.Code.History.History.RecordLinqUpdate(AdminUser, oldAddress, person.PrimaryAddress);
                db.SubmitChanges();

                
                CRM_FormFieldAnswer answer = db.CRM_FormFieldAnswers.FirstOrDefault(f => f.CRM_FormFieldID == 24 && f.TargetReference == person.Reference);

                if (answer == null)
                {
                    answer = new CRM_FormFieldAnswer();
                    answer.CRM_FormFieldID = 24;
                    answer.TargetReference = person.Reference;
                    db.CRM_FormFieldAnswers.InsertOnSubmit(answer);
                }

                answer.Answer = "";
                foreach (ListItem chkBox in rptInterests.Items)
                {
                    if (chkBox.Selected)
                    {
                        string id = chkBox.Value;
                        CRM_FormFieldItem item = db.CRM_FormFieldItems.Single(s => s.ID.ToString() == id);
                        answer.Answer += item.Label + "<br/>";
                    }
                }

                db.SubmitChanges();


                NoticeManager.SetMessage("Record merged");

            }
        }

        protected void lnkCreateNew_Click(object sender, EventArgs e)
        {
            CRM_Address address = new CRM_Address()
            {
                AddressLine1 = txtAddress1.Text,
                AddressLine2 = txtAddress2.Text,
                AddressLine3 = txtAddress3.Text,
                CountryID = db.Countries.First().ID,
                County = txtCounty.Text,
                Postcode = txtPostcode.Text,
                Town = txtCity.Text
            };

            db.CRM_Addresses.InsertOnSubmit(address);
            db.SubmitChanges();

            CRM_Person person = new CRM_Person()
            {
                DateAdded = UKTime.Now,
                CRM_AddressID = address.ID,
                DateModified = UKTime.Now,
                DateOfBirth = null,
                Firstname = txtFirstname.Text,
                IsArchived = false,
                IsChild = false,
                IsConcession = false,
                IsContactEmail = (bool)Entity.DoNotEmail,
                IsContactPost = (bool)Entity.DoNotMail,
                IsGiftAid = false,
                Lastname = txtLastname.Text,
                PrimaryEmail = txtEmail.Text,
                PrimaryTelephone = txtTelephone.Text,
                Title = ddlTitle.SelectedItem.Text,
                WebsiteAccountID = Entity.ID,
                IsCarerMinder = false,
                PreviousNames = ""
            };

            db.CRM_Persons.InsertOnSubmit(person);
            db.SubmitChanges();

            NoticeManager.SetMessage("Record added (" + person.Reference + ")");
        }

        protected void lnkNextRecord_Click(object sender, EventArgs e)
        {
            var baseSet = HoldingPen.BaseSet(db).ToList();

            int indexOf = baseSet.IndexOf(Entity);

            db.HoldingPens.DeleteOnSubmit(Entity);
            db.SubmitChanges();

            if (indexOf != baseSet.Count)
            {
                NoticeManager.SetMessage("Deleted temporary merge record and moved to next record", baseSet[indexOf + 1].DetailsURL);
            }
            else
            {
                NoticeManager.SetMessage("There are no further merge records", "list.aspx");
            }

        }
    }
}