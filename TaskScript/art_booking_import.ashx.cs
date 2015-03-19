using CRM.Code.Models;
using CRM.Code.Utils.Time;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CRM.TaskScript
{
    public class art_booking_import : IHttpHandler
    {

        private enum accountPos : int
        {
            Firstname,Lastname,Company,Address1,Address2,Address3,Address4,
            Postcode,Email,Phone1,Phone2
        }


        /// <summary>
        /// art_booking.xlsx https://coalface.sifterapp.com/issues/2898
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {

            RunImport(2, 10000);
        }

        protected int lineNumber = 0;
        protected void RunImport(int startLine, int endLine)
        {
            MainDataContext db = new MainDataContext();

            HttpServerUtility Server = HttpContext.Current.Server;
            
            string path = "/TaskScript/Art_Booking.csv";
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

                        CRM_Person person = db.CRM_Persons.FirstOrDefault(f => f.PrimaryEmail.Trim().ToLower() == contentsSplit[(int)accountPos.Email].Trim().ToLower() 
                            && f.Lastname.Trim().ToLower() == contentsSplit[(int)accountPos.Lastname].Trim().ToLower());

                        if (person == null)
                        {


                            CRM_Address address = new CRM_Address();
                            address.AddressLine1 = contentsSplit[(int)accountPos.Address1];
                            address.AddressLine2 = contentsSplit[(int)accountPos.Address2];
                            address.AddressLine3 = contentsSplit[(int)accountPos.Address3];
                            address.AddressLine4 = contentsSplit[(int)accountPos.Address4];
                            address.AddressLine5 = "";
                            address.Town = "";
                            address.County = contentsSplit[(int)accountPos.Address4];
                            address.Postcode = contentsSplit[(int)accountPos.Postcode];
                            address.CountryID = 1;
                            db.CRM_Addresses.InsertOnSubmit(address);
                            db.SubmitChanges();

                            person = new CRM_Person();
                            db.CRM_Persons.InsertOnSubmit(person);

                            person.AddressType = (byte)CRM_Address.Types.Home;
                            person.DateAdded = UKTime.Now;
                            person.IsArchived = false;
                            person.IsChild = false;
                            person.IsCarerMinder = false;
                            person.PreviousNames = "";
                            person.PrimaryEmail = contentsSplit[(int)accountPos.Email];
                            person.PrimaryTelephone = contentsSplit[(int)accountPos.Phone1];
                            person.Telephone2 = contentsSplit[(int)accountPos.Phone2];
                            person.IsDoNotMail = false;
                            person.LegacyID = null;
                            person.CRM_AddressID = address.ID;

                        }

                        person.DateModified = UKTime.Now;
                        person.Title = "";
                        person.Firstname = contentsSplit[(int)accountPos.Firstname];
                        person.Lastname = contentsSplit[(int)accountPos.Lastname];
                        person.IsMale = null;
                        person.IsDoNotEmail = false;

                        db.SubmitChanges();


                        if (!String.IsNullOrEmpty(contentsSplit[(int)accountPos.Company]))
                        {

                            CRM_Organisation org = db.CRM_Organisations.FirstOrDefault(f =>
                                f.Name.Trim().ToLower() == contentsSplit[(int)accountPos.Company].Trim().ToLower());

                            if (org == null)
                            {
                                CRM_Address orgAddress = new CRM_Address()
                                {
                                    AddressLine1 = contentsSplit[(int)accountPos.Address1],
                                    AddressLine2 = contentsSplit[(int)accountPos.Address2],
                                    AddressLine3 = contentsSplit[(int)accountPos.Address3],
                                    AddressLine4 = contentsSplit[(int)accountPos.Address4],
                                    AddressLine5 = "",
                                    County = "",
                                    Town = "",
                                    Postcode = contentsSplit[(int)accountPos.Postcode],
                                    CountryID = 1
                                };

                                db.CRM_Addresses.InsertOnSubmit(orgAddress);
                                db.SubmitChanges();

                                org = new CRM_Organisation()
                                {
                                    CRM_OrganisationTypeID = 1,
                                    CRM_AddressID = orgAddress.ID,
                                    Name = contentsSplit[(int)accountPos.Company],
                                    IsArchived = false,
                                    PrimaryContactReference = person.Reference
                                };
                                db.CRM_Organisations.InsertOnSubmit(org);
                                db.SubmitChanges();
                            }

                            CRM_Role role = db.CRM_Roles.FirstOrDefault(f => f.Name == "");

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