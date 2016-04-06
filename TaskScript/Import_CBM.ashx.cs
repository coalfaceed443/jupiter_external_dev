using CRM.Code.Models;
using CRM.Code.Utils.Time;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace CRM.TaskScript
{
    /// <summary>
    /// Summary description for Import_CBM
    /// </summary>
    public class Import_CBM : IHttpHandler
    {

        private enum accountPos : int
        {
            first, last, company, address, city, state, zip, country, email
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

            using (StreamReader reader = new StreamReader(Server.MapPath("/taskscript/CBM_Europe_import.csv"), Encoding.GetEncoding("iso-8859-1")))
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

                        CRM_Person person = db.CRM_Persons.FirstOrDefault(f => f.Firstname.ToUpper().Trim() == contentsSplit[(int)accountPos.first].ToUpper().Trim()
                        && f.Lastname.ToUpper().Trim() == contentsSplit[(int)accountPos.last].ToUpper().Trim()
                        && (f.PrimaryEmail.ToUpper().Trim() == contentsSplit[(int)accountPos.email].ToUpper().Trim() || contentsSplit[(int)accountPos.email] == ""));


                        Country country = db.Countries.FirstOrDefault(f => f.Name == contentsSplit[(int)accountPos.country]);

                        if (country == null)
                        {
                            country = db.Countries.Single(s => s.ID == 1);
                            HttpContext.Current.Response.Write("country missing - " + contentsSplit[(int)accountPos.country]);
                        }


                        if (person == null)
                        {

                            HttpContext.Current.Response.Write("adding person - " + contentsSplit[(int)accountPos.last] + "<br/>");

                            CRM_Address address = new CRM_Address()
                            {
                                AddressLine1 = "",
                                AddressLine2 = "",
                                AddressLine3 = "",
                                AddressLine4 = "",
                                AddressLine5 = "",
                                CountryID = country.ID,
                                Postcode = "",
                                Town = "",
                                County = ""
                            };

                            if (contentsSplit[(int)accountPos.company].Trim() == "")
                            {
                                address.AddressLine1 = contentsSplit[(int)accountPos.address].Replace("@", ",");
                                address.Postcode = contentsSplit[(int)accountPos.zip].Replace("@", ",");
                                address.Town = contentsSplit[(int)accountPos.city].Replace("@", ",");
                                address.County = contentsSplit[(int)accountPos.state].Replace("@", ",");
                            }

                            db.CRM_Addresses.InsertOnSubmit(address);
                            db.SubmitChanges();


                            person = new CRM_Person()
                            {
                                CRM_AddressID = address.ID,
                                AddressType = (byte)CRM_Address.Types.Business,
                                DateAdded = UKTime.Now,
                                DateModified = UKTime.Now,
                                DateOfBirth = null,
                                Firstname = contentsSplit[(int)accountPos.first],
                                Lastname = contentsSplit[(int)accountPos.last],
                                IsArchived = false,
                                IsCarerMinder = false,
                                IsChild = false,
                                IsConcession = false,
                                IsContactEmail = true,
                                IsContactPost = true,
                                IsDeceased = false,
                                IsDoNotEmail = false,
                                IsDoNotMail = false,
                                IsGiftAid = false,
                                IsMale = null,
                                LegacyID = null,
                                PreviousNames = "",
                                PrimaryEmail = contentsSplit[(int)accountPos.company].Trim() == "" ? contentsSplit[(int)accountPos.email] : "",
                                PrimaryTelephone = "",
                                Telephone2 = "",
                                Title = "",
                            };

                            db.CRM_Persons.InsertOnSubmit(person);
                            db.SubmitChanges();

                        }
                        else
                        {
                            HttpContext.Current.Response.Write("person exists already - " + contentsSplit[(int)accountPos.last] + "<br/>");

                        }

                        CRM_FormFieldItem gallery = db.CRM_FormFieldItems.Single(s => s.ID == 9);
                        CRM_FormFieldItem collector = db.CRM_FormFieldItems.Single(s => s.ID == 31);


                        CRM_FormFieldResponse galleryresponse = db.CRM_FormFieldResponses.FirstOrDefault(f => f.CRM_FormFieldItemID == gallery.ID && f.TargetReference == person.Reference);
                        CRM_FormFieldResponse collectorresponse = db.CRM_FormFieldResponses.FirstOrDefault(f => f.CRM_FormFieldItemID == collector.ID && f.TargetReference == person.Reference);

                        if (galleryresponse == null)
                        {
                            HttpContext.Current.Response.Write("adding gallery for - " + contentsSplit[(int)accountPos.last] + "<br/>");
                            galleryresponse = new CRM_FormFieldResponse()
                            {
                                CRM_FormFieldItemID = gallery.ID,
                                CRM_FormFieldID = gallery.CRM_FormFieldID,
                                TargetReference = person.Reference,
                                Answer = ""
                            };

                            db.CRM_FormFieldResponses.InsertOnSubmit(galleryresponse);
                            db.SubmitChanges();
                        }


                        if (collectorresponse == null)
                        {
                            HttpContext.Current.Response.Write("adding collector for - " + contentsSplit[(int)accountPos.last] + "<br/>");
                            collectorresponse = new CRM_FormFieldResponse()
                            {
                                CRM_FormFieldItemID = collector.ID,
                                CRM_FormFieldID = collector.CRM_FormFieldID,
                                TargetReference = person.Reference,
                                Answer = ""
                            };

                            db.CRM_FormFieldResponses.InsertOnSubmit(collectorresponse);
                            db.SubmitChanges();
                        }


                        string companystr = contentsSplit[(int)accountPos.company].Replace("@", ",").Trim();

                        if (companystr != "")
                        {

                            CRM_Organisation company = db.CRM_Organisations.FirstOrDefault(s => s.Name.ToUpper() == companystr.ToUpper());

                            if (company == null)
                            {
                                HttpContext.Current.Response.Write("adding company for - " + contentsSplit[(int)accountPos.last] + "<br/>");

                                CRM_Address address = new CRM_Address()
                                {
                                    AddressLine1 = contentsSplit[(int)accountPos.address].Replace("@", ",").Trim(),
                                    AddressLine2 = "",
                                    AddressLine3 = "",
                                    AddressLine4 = "",
                                    AddressLine5 = "",
                                    Town = contentsSplit[(int)accountPos.city].Replace("@", ","),
                                    County = "",
                                    Postcode = contentsSplit[(int)accountPos.zip].Replace("@", ","),
                                    CountryID = country.ID
                                };


                                db.CRM_Addresses.InsertOnSubmit(address);
                                db.SubmitChanges();

                                company = new CRM_Organisation()
                                {
                                    CRM_AddressID = address.ID,
                                    IsArchived = false,
                                    Name = companystr,
                                    PrimaryContactReference = "",
                                    CRM_OrganisationTypeID = 1
                                };

                                db.CRM_Organisations.InsertOnSubmit(company);
                                db.SubmitChanges();

                            }
                            else
                            {
                                HttpContext.Current.Response.Write("company already exists for - " + contentsSplit[(int)accountPos.last] + "<br/>");
                            }

                            CRM_PersonOrganisation personOrg = db.CRM_PersonOrganisations.FirstOrDefault(f => f.CRM_OrganisationID == company.ID && f.CRM_PersonID == person.ID);

                            if (personOrg == null)
                            {
                                HttpContext.Current.Response.Write("adding organisation link for - " + contentsSplit[(int)accountPos.last] + "<br/>");
                                personOrg = new CRM_PersonOrganisation()
                                {
                                    CRM_OrganisationID = company.ID,
                                    CRM_PersonID = person.ID,
                                    IsArchived = false,
                                    Telephone = "",
                                    Email = contentsSplit[(int)accountPos.email],
                                    CRM_RoleID = 7 // empty
                                };

                                db.CRM_PersonOrganisations.InsertOnSubmit(personOrg);
                                db.SubmitChanges();

                                person.PrimaryAddressID = company.CRM_AddressID;
                                db.SubmitChanges();
                            }
                            else
                            {
                                HttpContext.Current.Response.Write("organisation link already exists for - " + contentsSplit[(int)accountPos.last] + "<br/>");

                            }
                        }
                        else
                        {

                            HttpContext.Current.Response.Write("no company for - " + contentsSplit[(int)accountPos.last] + "<br/>");
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