using CRM.Code.Models;
using CRM.Code.Utils.Time;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CRM.TaskScript
{
    /// <summary>
    /// Summary description for Relations
    /// </summary>
    public class Relations : IHttpHandler
    {

        private enum accountPos : int
        {
            firstname,surname,relcode,isspouse,
            norelationships,relfirstname,relsurname
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

            string path = "/TaskScript/relationships.csv";
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

                        CRM_Person person = db.CRM_Persons.FirstOrDefault(f => f.Firstname.Trim().ToLower() == contentsSplit[(int)accountPos.firstname].Trim().ToLower()
                            && f.Lastname.Trim().ToLower() == contentsSplit[(int)accountPos.surname].Trim().ToLower() && f.Firstname.Trim() != "");

                        CRM_RelationCode unknownRelationCode = db.CRM_RelationCodes.Single(s => s.Name == "");;

                        CRM_Person personB = db.CRM_Persons.FirstOrDefault(f => f.Firstname.Trim().ToLower() == contentsSplit[(int)accountPos.relfirstname].Trim().ToLower()
                        && f.Lastname.Trim().ToLower() == contentsSplit[(int)accountPos.relsurname].Trim().ToLower());

                        if (person != null && personB == null)
                        {

                            CRM_Address relationAddress = person.PrimaryAddress;

                            CRM_Address address = new CRM_Address()
                            {
                                AddressLine1 = relationAddress.AddressLine1,
                                AddressLine2 = relationAddress.AddressLine2,
                                AddressLine3 = relationAddress.AddressLine3,
                                AddressLine4 = relationAddress.AddressLine4,
                                AddressLine5 = relationAddress.AddressLine5,
                                Town = relationAddress.Town,
                                CountryID = relationAddress.CountryID,
                                County = relationAddress.County,
                                Postcode = relationAddress.Postcode
                            };

                            db.CRM_Addresses.InsertOnSubmit(address);
                            db.SubmitChanges();

                            personB = new CRM_Person()
                            {
                                Title = "",
                                Firstname = contentsSplit[(int)accountPos.relfirstname],
                                Lastname = contentsSplit[(int)accountPos.relsurname],
                                DateAdded = UKTime.Now,
                                DateModified = UKTime.Now,
                                DateOfBirth = null,
                                IsArchived = false,
                                IsMale = null,
                                PrimaryEmail = "",
                                PrimaryTelephone = "",
                                IsContactEmail = false,
                                IsContactPost = true,
                                IsChild = false,
                                IsConcession = false,
                                IsCarerMinder = false,
                                IsDeceased = false,
                                IsGiftAid = false,
                                IsDoNotEmail = false,
                                IsDoNotMail = false,
                                Telephone2 = "",
                                PreviousNames = "",
                                CRM_AddressID = address.ID,
                                Password = "",
                                TempCode = ""
                            };

                            db.CRM_Persons.InsertOnSubmit(personB);
                            db.SubmitChanges();

                            CRM_FormFieldAnswer originAnswer = new CRM_FormFieldAnswer()
                            {
                                Answer = "Import",
                                CRM_FormFieldID = 2,
                                TargetReference = personB.Reference
                            };

                            db.CRM_FormFieldAnswers.InsertOnSubmit(originAnswer);
                            db.SubmitChanges();
                        }


                        if (person != null && personB != null && contentsSplit[(int)accountPos.relcode] != "Daughter" && contentsSplit[(int)accountPos.relcode] != "Son")
                        {
                            CRM_RelationCode relCode = db.CRM_RelationCodes.FirstOrDefault(f => f.Name.Trim().ToLower() == contentsSplit[(int)accountPos.relcode].Trim().ToLower());


                            if (relCode != null)
                            {
                                CRM_PersonRelationship personRel = new CRM_PersonRelationship();
                                personRel.PersonA = person;
                                personRel.PersonB = personB;
                                personRel.CRM_PersonIDAddress = person.ID;
                                personRel.PersonACode = relCode;
                                personRel.PersonBCode = unknownRelationCode;

                                string salutation = "";


                                // mr and mrs somebody
                                if (person.Lastname.Trim().ToLower() == personB.Lastname.Trim().ToLower() && person.Title != "" && personB.Title != "")
                                {
                                    if (person.Title == "Mr")
                                    {
                                        salutation = person.Title + " and " + personB.Title + " " + person.Lastname;
                                    }
                                    else
                                    {
                                        salutation = personB.Title + " and " + person.Title + " " + person.Lastname;
                                    }
                                }
                                else if ((person.Lastname.Trim().ToLower() != personB.Lastname.Trim().ToLower()) && person.Title != "" && personB.Title != "")
                                {
                                    // mr smith and mrs somebody

                                    if (person.Title == "Mr")
                                    {
                                        salutation = person.Title + " " + person.Lastname + " and " + personB.Title + " " + personB.Lastname;
                                    }
                                    else
                                    {
                                        salutation = personB.Title + " " + personB.Lastname + " and " + person.Title + " " + person.Lastname;
                                    }

                                }
                                else if (person.Lastname.Trim().ToLower() == personB.Lastname.Trim().ToLower())
                                {
                                    salutation = person.Firstname + " and " + personB.Firstname + " " + person.Lastname;
                                }
                                else
                                {
                                    salutation = person.Firstname + " " + person.Lastname + " and " + personB.Firstname + " " + personB.Lastname;
                                }

                                personRel.Salutation = salutation;

                                db.CRM_PersonRelationships.InsertOnSubmit(personRel);
                                db.SubmitChanges();

                            }
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