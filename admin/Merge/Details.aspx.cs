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
using System.Web.Security;
using CRM.Code.Managers;
using CRM.Code.Utils.Time;
using System.Web.UI.HtmlControls;

namespace CRM.admin.Merge
{
    public partial class Details : AdminPage
    {
        protected HoldingPen Entity;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            Entity = db.HoldingPens.Single(c => c.ID.ToString() == Request.QueryString["id"]);


            Dictionary<byte, string> comparibles = new Dictionary<byte, string>();
            
            comparibles.Add((byte)CRM_Person.SearchKeys.DoB, "");
            comparibles.Add((byte)CRM_Person.SearchKeys.PrimaryEmail, Entity.Email);
            comparibles.Add((byte)CRM_Person.SearchKeys.Postcode, Entity.Postcode);
            comparibles.Add((byte)CRM_Person.SearchKeys.Telephone, Entity.Telephone);
            comparibles.Add((byte)CRM_Person.SearchKeys.Fullname, Entity.Firstname + " " + Entity.Lastname);


            if (!Page.IsPostBack)
            {

                lnkNextRecord.Text += " (" + (HoldingPen.BaseSet(db).Count() - 1 - HoldingPen.BaseSet(db).ToList().IndexOf(Entity)) + " remaining)";


                var dupes = from p in CRM_Person.BaseSet(db)
                            where p.SearchDictionary.Any(c => comparibles.ContainsKey(c.Key) && comparibles[c.Key].Trim() != "" && c.Value.ToLower().Trim() == (comparibles[c.Key].ToLower().Trim()))
                            orderby p.SearchDictionary.Where(c => comparibles.ContainsKey(c.Key) && comparibles[c.Key].Trim() != "" && c.Value.ToLower().Trim() == (comparibles[c.Key])).Count() descending
                            select (object)p;

                lvItems.DataSource = dupes.Take(20);
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


            rptConstituent.DataSource = db.CRM_FormFieldItems.Where(f => f.CRM_FormFieldID == 1
                && f.IsActive && !f.IsArchived
                );
            rptConstituent.DataBind();

            if (Entity.JointBtoAID != null)
            {
                CRM_RelationCode code = db.CRM_RelationCodes.SingleOrDefault(f => f.ID == Entity.JointBtoAID);
                if (code != null)
                {
                    litOtherRecordRel.Text = code.Name;
                }
            }


            if (Entity.JointAtoBID != null)
            {
                CRM_RelationCode code = db.CRM_RelationCodes.SingleOrDefault(f => f.ID == Entity.JointAtoBID);
                if (code != null)
                {
                    litFromRel.Text = code.Name;
                }
            }

            try
            {
                if (Entity.InterestObjects != null)
                {
                    var items = from p in serializer.Deserialize<IEnumerable<CRM.WebService.CRMSync.InterestAnswer>>(Entity.InterestObjects)
                                select p;

                    rptInterests.DataSource = interests;
                    rptInterests.DataBind();
                }
            }
            catch(ArgumentNullException ex) { }
            /*
            for (int i = 0; i < rptInterests.Items.Count; i++)
            {
                
                foreach (CRM.WebService.CRMSync.InterestAnswer displayItem in items)
                {

                    if (rptInterests.Items[i].Value == displayItem.FormFieldItemID.ToString() && displayItem.OptIn == true)
                        rptInterests.Items[i].Selected = true;
                }

            }
             */

        }

        protected void lnkUpdate_Click(object sender, EventArgs e)
        {
            CRM.Code.Models.CRM_Person person = db.CRM_Persons.Single(s => s.ID.ToString() == ((LinkButton)sender).CommandArgument);

            litBefore.Text = person.MergeCard.Replace(Environment.NewLine, "<br/>");
           
            lblConstituentTypesBefore.Text = CRM.Code.Helpers.JSONSet.FlattenList(person.ConstituentTypes(db).ToList(), "<br/>");

            litAfter.Text = TransformPerson(person).MergeCard.Replace(Environment.NewLine, "<br/>");
            lblConstituentTypesAfter.Text = lblConstituentTypesBefore.Text + "<br/>";

            foreach (RepeaterItem item in rptConstituent.Items)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkOption");
                HtmlGenericControl lbl = (HtmlGenericControl)item.FindControl("lblOption");

                if (chk.Checked && !person.ConstituentTypes(db).Contains(lbl.InnerText))
                {
                    lblConstituentTypesAfter.Text += lbl.InnerText + "<br/>";
                }
            }


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

                person.WebsiteAccountID = Entity.OriginAccountID;
                person.Password = Entity.Password;

                if (Entity.DoNotMail != null)
                    person.IsDoNotMail = (bool)Entity.DoNotMail;

                if (Entity.DoNotEmail != null)
                    person.IsDoNotEmail = (bool)Entity.DoNotEmail;

                /*
                if (Entity.AlwaysSendPassInfo != null)
                    person.AlwaysSendPassInfo = (bool)Entity.AlwaysSendPassInfo;
                */

                db.SubmitChanges();

                CRM.Code.History.History.RecordLinqUpdate(AdminUser, oldEntity, person);
                CRM.Code.History.History.RecordLinqUpdate(AdminUser, oldAddress, person.PrimaryAddress);
                db.SubmitChanges();

                ApplyCustomField(person, 2, 22);
                db.SubmitChanges();

                ApplyConstituentsToSelected(person);


                /*
                
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

                 */

                AddRelationship(person);
 
                db.SubmitChanges();

                
                NoticeManager.SetMessage("Record merged");

            }
        }

        protected void AddRelationship(CRM_Person person)
        {
            if (Entity.JointHoldingPenID != null)
            {

                CRM_Person otherRecord = db.CRM_Persons.FirstOrDefault(f => f.WebsiteAccountID == Entity.OriginAccountID &&
                    Entity.OriginType == (byte)HoldingPen.Origins.websitemembership
                    && person.ID != f.ID);

                if (otherRecord != null)
                {
                    CRM_PersonRelationship relationship = new CRM_PersonRelationship();
                    relationship.CRM_PersonIDAddress = person.ID;
                    relationship.PersonA = otherRecord;
                    relationship.PersonB = person;
                    relationship.CRM_RelationCodeID = (int)Entity.JointAtoBID;
                    relationship.CRM_RelationCodeID2 = (int)Entity.JointBtoAID;
                    relationship.Salutation = Entity.JointSalutation;

                    db.CRM_PersonRelationships.InsertOnSubmit(relationship);
                    db.SubmitChanges();
                }

            }

        }

        protected void ApplyConstituentsToSelected(CRM_Person person)
        {

            foreach (RepeaterItem item in rptConstituent.Items)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkOption");
                HtmlGenericControl lbl = (HtmlGenericControl)item.FindControl("lblOption");

                if (chk.Checked)
                {
                    ApplyCustomField(person, 1, Convert.ToInt32(chk.Attributes["data-id"]));
                }
            }

        }

        protected void ApplyCustomField(CRM_Person Person, int formFieldID, int formFieldItemID)
        {
            IEnumerable<CRM_FormFieldResponse> answers = 
                db.CRM_FormFieldResponses.Where(f => f.CRM_FormFieldID == formFieldID  && f.TargetReference == Person.Reference && f.CRM_FormFieldItemID == formFieldItemID);
            
            if (!answers.Any())
            {
                CRM_FormFieldResponse answer = new CRM_FormFieldResponse()
                {
                    CRM_FormFieldItemID = formFieldItemID,
                    CRM_FormFieldID = formFieldID,
                    Answer = "",
                    TargetReference = Person.Reference
                };

                db.CRM_FormFieldResponses.InsertOnSubmit(answer);
            }

            db.SubmitChanges();
        }

        protected void lnkCreateNew_Click(object sender, EventArgs e)
        {
            CRM_Address address = new CRM_Address()
            {
                AddressLine1 = txtAddress1.Text,
                AddressLine2 = txtAddress2.Text,
                AddressLine3 = txtAddress3.Text,
                AddressLine4 = "",
                AddressLine5 = "",
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
                WebsiteAccountID = Entity.OriginAccountID,
                IsCarerMinder = false,
                PreviousNames = "",
                LegacyID = null,
                IsMale = null,
                AddressType = 0,
                Telephone2 = "",
                PrimaryAddressID = address.ID,
                Password = "",
                TempCode = ""
            };

            db.CRM_Persons.InsertOnSubmit(person);
            db.SubmitChanges();

            ApplyCustomField(person, 2, 22);
            ApplyConstituentsToSelected(person);

            AddRelationship(person);

            NoticeManager.SetMessage("Record added (" + person.Reference + ")");
        }

        protected void lnkNextRecord_Click(object sender, EventArgs e)
        {
            var baseSet = HoldingPen.BaseSet(db).ToList();

            int indexOf = baseSet.IndexOf(Entity);

            db.HoldingPens.DeleteOnSubmit(Entity);
            db.SubmitChanges();

            if (indexOf != baseSet.Count - 1)
            {
                NoticeManager.SetMessage("Deleted temporary merge record and moved to next record", baseSet[indexOf + 1].DetailsURL);
            }
            else
            {
                NoticeManager.SetMessage("There are no further merge records", "list.aspx");
            }

        }

        protected void rptConstituent_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            CRM_FormFieldItem item = (CRM_FormFieldItem)e.Item.DataItem;

            CheckBox chk = (CheckBox)e.Item.FindControl("chkOption");
            chk.Attributes["data-id"] = item.ID.ToString();

            HtmlGenericControl lblOption = (HtmlGenericControl)e.Item.FindControl("lblOption");

            lblOption.InnerText = item.Label;
            lblOption.Attributes["for"] = chk.ClientID;

            /*
            if (item.ID == 18 && Entity.BasketContents.Contains("Event"))
            {
                chk.Checked = true;
            }*/

            if (item.ID == 20)
            {
                chk.Checked = true;
            }

            if (item.ID == 1 && (Entity.MembershipA != "" || Entity.MembershipB != ""))
            {
                chk.Checked = true;
            }

        }

    }
}