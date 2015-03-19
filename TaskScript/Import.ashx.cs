using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;
using System.IO;
using CRM.Code.Utils.Time;

namespace CRM.TaskScript
{
    /// <summary>
    /// Summary description for Import
    /// </summary>
    public class Import : IHttpHandler
    {


        private enum accountPos : int
        {
            Constituent = 0, LegacyID, Title, Firstname, MiddleName, Surname,  //col F
            Age, Gender, Inactive, IndustryName, MaidenName, MartialStatus, NovalidAddress,  //col M
            NoEmail, BioSuffix, AddressWhole, BioOrgName, Address1, Address2, //col S
            Address3, Address4, Address5, City, County, AddrPosition, Postcode, AddrOrgName, // col AA
            CountryDesc, AddressType, AddressState, PhoneType11, PhoneNumber11, // col AF
            PhoneType12, PhoneNumber12, PhoneType21, PhoneNumber21, // col AJ
            PhoneType31, PhoneType41, PhoneType51, RelInd1Title, RelInd1Firstname, RelInd1Surname, // col AP
            Recip_Relation_Code, CnRelInd_1_01_Reciprocate, RelIndRelationCode, RelIndAddress1, // col AT
            RelIndAddress2, RelIndAddress3, RelIndAddress4, RelIndAddress5, RelIndPostcode, // col AY
            RelInd2Title, RelInd2Firstname, RelInd2Surname, RelInd2RecipRelationCode, RelInd2Reciprocate, Rel2RelationCode, // col BE, 
            RelInd2Address1, RelInd2Address2, RelInd2Address3, RelInd2Address4, // col BI
            RelInd2Address5, RelInd2Postcode, RelInd3Title, RelInd3Firstname, RelInd3Surname, RelInd3RelationCode, RelInd3Reciprocate, // col BP
            RelInd3Relation, RelInd3Address1, RelInd3Address2, RelInd3Address3, RelInd3Address4, // col BU
            RelInd3Address5, RelInd3Postcode, AttrCat1Date, AttrCat1Desc, AttrCat2Date, AttrCat2Desc,  // col CA
            MembershipID, MembershipLastRenew, MembershipExpiry, Membership2ID, // col CE
            Membership2LastRenew, Membership2Expiry, Membership3ID, // col CH
            Membership3LastRenew, Membership3Expiry, Membership4ID, // col CK
            Membership4LastRenew, Membership4Expiry, Membership5ID, // col CN
            Membership5LastRenew, Membership5Expiry,  // col CP
            OrgName, OrgPosition, OrgAddr1, OrgAddr2, OrgAddr3, OrgAddr4, OrgAddrCity, OrgAddrPostcode

        }


        public void ProcessRequest(HttpContext context)
        {
            RunImport(2, 10000);

        }

        protected int lineNumber = 0;
        public void RunImport(int startLine, int endLine)
        {
            MainDataContext db = new MainDataContext();

            HttpServerUtility Server = HttpContext.Current.Server;
            
            string path = "/TaskScript/FinalListforCoalFace.csv";
            using (StreamReader reader = new StreamReader(Server.MapPath(path)))
            {
                
                lineNumber = 0;

                while (!reader.EndOfStream)
                {

                    lineNumber++;

                    string contents = reader.ReadLine();
                    if (lineNumber >= startLine && lineNumber <= endLine)
                    {
                        string[] contentsSplit = contents.Split(',');
                        string errorMessage = "";


                        CRM_FormFieldItem ffItem = db.CRM_FormFieldItems.FirstOrDefault(f => f.Label == contentsSplit[(int)accountPos.Constituent]);

                        if (ffItem == null)
                        {
                            ffItem = new CRM_FormFieldItem()
                            {
                                Label = contentsSplit[(int)accountPos.Constituent],
                                CRM_FormFieldID = 1,
                                OrderNo = Code.Utils.Ordering.Ordering.GetNextOrderID(db.CRM_FormFieldItems),
                                IsActive = true,
                                IsArchived = false
                            };

                            db.CRM_FormFieldItems.InsertOnSubmit(ffItem);
                            db.SubmitChanges();
                        }

                        CRM_Person person = db.CRM_Persons.FirstOrDefault(f => f.LegacyID != null && f.LegacyID.ToString() == contentsSplit[(int)accountPos.LegacyID]);

                        if (person == null)
                        {


                            CRM_Address address = new CRM_Address();
                            address.AddressLine1 = contentsSplit[(int)accountPos.Address1];
                            address.AddressLine2 = contentsSplit[(int)accountPos.Address2];
                            address.AddressLine3 = contentsSplit[(int)accountPos.Address3];
                            address.AddressLine4 = contentsSplit[(int)accountPos.Address4];
                            address.AddressLine5 = contentsSplit[(int)accountPos.Address5];
                            address.Town = contentsSplit[(int)accountPos.City];
                            address.County = contentsSplit[(int)accountPos.County];
                            address.Postcode = contentsSplit[(int)accountPos.Postcode];
                            address.CountryID = 1;
                            db.CRM_Addresses.InsertOnSubmit(address);
                            db.SubmitChanges();

                            person = new CRM_Person();
                            db.CRM_Persons.InsertOnSubmit(person);
                            if (contentsSplit[(int)accountPos.AddressType] == "Business")
                                person.AddressType = (byte)CRM_Address.Types.Business;
                            person.AddressType = (byte)CRM_Address.Types.Home;
                            person.DateAdded = UKTime.Now;
                            person.DateModified = UKTime.Now;
                            person.IsArchived = false;
                            person.IsChild = false;
                            person.IsCarerMinder = false;
                            person.PreviousNames = "";
                            person.PrimaryEmail = "";
                            person.PrimaryTelephone = "";
                            person.Telephone2 = "";
                            person.IsDoNotMail = false;
                            person.LegacyID = Convert.ToInt32(contentsSplit[(int)accountPos.LegacyID]);
                            person.CRM_AddressID = address.ID;
                        
                        }


                        person.Title = contentsSplit[(int)accountPos.Title];
                        person.Firstname = contentsSplit[(int)accountPos.Firstname];
                        person.Lastname = contentsSplit[(int)accountPos.Surname];

                        if (contentsSplit[(int)accountPos.Gender] == "Male")
                            person.IsMale = true;
                        else if (contentsSplit[(int)accountPos.Gender] == "Female")
                            person.IsMale = false;
                        else
                            person.IsMale = null;

                        if (contentsSplit[(int)accountPos.NoEmail] == "Yes")
                            person.IsDoNotEmail = true;

                        db.SubmitChanges();

                        CRM_FormFieldAnswer answer = new CRM_FormFieldAnswer()
                        {
                            Answer = contentsSplit[(int)accountPos.Constituent],
                            CRM_FormFieldID = ffItem.CRM_FormFieldID,
                            TargetReference = person.Reference
                        };

                        db.CRM_FormFieldAnswers.InsertOnSubmit(answer);
                        db.SubmitChanges();


                        if (contentsSplit[(int)accountPos.PhoneNumber11].Contains("@"))
                        {
                            person.PrimaryEmail = contentsSplit[(int)accountPos.PhoneNumber11];
                        }
                        else
                        {
                            person.PrimaryTelephone = contentsSplit[(int)accountPos.PhoneNumber11];
                            person.PrimaryEmail = contentsSplit[(int)accountPos.PhoneNumber12];
                        }

                        if (!contentsSplit[(int)accountPos.PhoneNumber12].Contains("@"))
                        {
                            person.Telephone2 = contentsSplit[(int)accountPos.PhoneNumber12];
                        }

                        db.SubmitChanges();


                        if (!String.IsNullOrEmpty(contentsSplit[(int)accountPos.MembershipID].Trim()))
                        {


                            CRM_AnnualPassCard card = new CRM_AnnualPassCard()
                            {
                                MembershipNumber = Convert.ToInt32(contentsSplit[(int)accountPos.MembershipID])
                            };

                            db.CRM_AnnualPassCards.InsertOnSubmit(card);
                            db.SubmitChanges();

                            CRM_AnnualPass pass = new CRM_AnnualPass();
                            pass.CRM_AnnualPassCardID = card.ID;
                            pass.AmountPaid = 0;
                            pass.CRM_AnnualPassTypeID = 15;
                            pass.IsArchived = false;
                            pass.DiscountApplied = "";
                            pass.PrimaryContactReference = person.Reference;

                            try
                            {
                                if (!String.IsNullOrEmpty(contentsSplit[(int)accountPos.MembershipExpiry]) && String.IsNullOrEmpty((contentsSplit[(int)accountPos.MembershipLastRenew])))
                                {
                                    pass.StartDate = Code.Utils.Text.Text.FormatInputDate(contentsSplit[(int)accountPos.MembershipExpiry]);

                                }
                                else
                                {
                                    pass.StartDate = Code.Utils.Text.Text.FormatInputDate(contentsSplit[(int)accountPos.MembershipExpiry]).AddYears(-1);

                                }
                            }
                            catch {
                                pass.StartDate = UKTime.Now;
                            }

                            try
                            {
                                pass.ExpiryDate = Code.Utils.Text.Text.FormatInputDate(contentsSplit[(int)accountPos.MembershipExpiry]);
                            }
                            catch { pass.ExpiryDate = new DateTime(2075, 12, 31); }
                            
                            db.CRM_AnnualPasses.InsertOnSubmit(pass);
                            db.SubmitChanges();

                            CRM_AnnualPassPerson passPerson = new CRM_AnnualPassPerson()
                            {
                                CRM_AnnualPassID = pass.ID,
                                CRM_PersonID = person.ID,
                                IsArchived = false
                            };

                            db.CRM_AnnualPassPersons.InsertOnSubmit(passPerson);
                            db.SubmitChanges();
                        }

                        if (!String.IsNullOrEmpty(contentsSplit[(int)accountPos.OrgName]))
                        {

                            CRM_Organisation org = db.CRM_Organisations.FirstOrDefault(f => f.Name == contentsSplit[(int)accountPos.OrgName]);

                            if (org == null)
                            {
                                CRM_Address orgAddress = new CRM_Address()
                                {
                                    AddressLine1 = contentsSplit[(int)accountPos.OrgAddr1],
                                    AddressLine2 = contentsSplit[(int)accountPos.OrgAddr2],
                                    AddressLine3 = contentsSplit[(int)accountPos.OrgAddr3],
                                    AddressLine4 = contentsSplit[(int)accountPos.OrgAddr4],
                                    AddressLine5 = "",
                                    County = "",
                                    Town = contentsSplit[(int)accountPos.OrgAddrCity],
                                    Postcode = contentsSplit[(int)accountPos.Postcode],
                                    CountryID = 1
                                };

                                db.CRM_Addresses.InsertOnSubmit(orgAddress);
                                db.SubmitChanges();

                                org = new CRM_Organisation()
                                {
                                    CRM_OrganisationTypeID = 1,
                                    CRM_AddressID = orgAddress.ID,
                                    Name = contentsSplit[(int)accountPos.OrgName],
                                    IsArchived = false,
                                    PrimaryContactReference = person.Reference
                                };
                                db.CRM_Organisations.InsertOnSubmit(org);
                                db.SubmitChanges();
                            }

                            CRM_Role role = db.CRM_Roles.FirstOrDefault(f => f.Name == contentsSplit[(int)accountPos.OrgPosition]);

                            if (role == null)
                            {
                                role = new CRM_Role();
                                db.CRM_Roles.InsertOnSubmit(role);
                                role.Name = contentsSplit[(int)accountPos.OrgPosition];
                                db.SubmitChanges();
                            }

                            CRM_PersonOrganisation personOrg = new CRM_PersonOrganisation()
                            {
                                CRM_OrganisationID = org.ID,
                                CRM_RoleID = role.ID,
                                IsArchived = false,
                                Email = "",
                                Telephone = "",
                                CRM_PersonID = person.ID
                            };

                            db.CRM_PersonOrganisations.InsertOnSubmit(personOrg);
                            db.SubmitChanges();


                        }
                    }

                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}