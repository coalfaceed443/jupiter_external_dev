using CRM.Code.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CRM.TaskScript
{
    /// <summary>
    /// Summary description for EdinSchoolsImport
    /// </summary>
    public class EdinSchoolsImport : IHttpHandler
    {

        private enum accountPos : int
        {
            establishmentType, establishmentSubType, estabilshmentName, contactTitle, contactName, // E
            address1, address2, towncity, postcode, phone, fax, email, website, // M
            notes, seed, nursery, annexe, neighbourhood, multimember, x, y, shortname, openinghours, // w
            religion, secondaryschool, meal, mealmore, eco, ecomore, catchmentmap, // ad
            glowsca, glowasm, kitchen, businessmanager, schoolcoord, ascphone, ccopening, ccbus, ccfacil, // an
            ccCafe, ccInternet, ccKey, ccDisabled, ccprojects, ccprogramme, ccmoreinfo, cclocation, ictcord, //aw
            htemail, avsosroll //ay
        }

        public void ProcessRequest(HttpContext context)
        {

            MainDataContext db = new MainDataContext();
            CRM_KeyStage primaryKeyStage = db.CRM_KeyStages.Single(s => s.ID == 1); // primary
            CRM_KeyStage secondaryKeyStage = db.CRM_KeyStages.Single(s => s.ID == 2); // secondary
            
            string path = "/TaskScript/Edinburgh_schools.csv";
            string pathSecondary = "/TaskScript/Edinburgh_schools_secondary.csv";

            RunImport(2, 10000, path, primaryKeyStage);
            RunImport(2, 10000, pathSecondary, secondaryKeyStage);
        }


        protected int lineNumber = 0;
        public void RunImport(int startLine, int endLine, string path, CRM_KeyStage stage)
        {
            MainDataContext db = new MainDataContext();

            HttpServerUtility Server = HttpContext.Current.Server;

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

                        CRM_School school = db.CRM_Schools.FirstOrDefault(s => s.Name == contentsSplit[(int)accountPos.estabilshmentName]
                            && s.CRM_Address.Postcode.Trim().Replace(" ", "").ToLower() == contentsSplit[(int)accountPos.postcode].Trim().Replace(" ", "").ToLower());

                        CRM_LEA lea = db.CRM_LEAs.Single(s => s.ID == 35); // edinburgh
                        CRM_SchoolType type = db.CRM_SchoolTypes.Single(s => s.ID == 1); // none
                        Country country = db.Countries.Single(s => s.ID == 1); // uk

                        if (school == null)
                        {
                            CRM_Address address = new CRM_Address()
                            {
                                AddressLine1 = contentsSplit[(int)accountPos.address1].Trim(),
                                AddressLine2 = contentsSplit[(int)accountPos.address2].Trim(),
                                AddressLine3 = "",
                                AddressLine4 = "",
                                AddressLine5 = "",
                                Town = contentsSplit[(int)accountPos.towncity],
                                County = "",
                                Postcode = contentsSplit[(int)accountPos.postcode],
                                CountryID = country.ID
                            };

                            
                            db.CRM_Addresses.InsertOnSubmit(address);
                            db.SubmitChanges();
                           
                            school = new CRM_School()
                            {
                                CRM_AddressID = address.ID,
                                ApproxPupils = 0,
                                CRM_LEAID = lea.ID,
                                CRM_RegionID = null,
                                CRM_SchoolTypeID = type.ID,
                                Email = contentsSplit[(int)accountPos.email].Trim(),
                                IsArchived = false,
                                IsSEN = false,
                                Name = contentsSplit[(int)accountPos.estabilshmentName].Trim(),
                                Phone = contentsSplit[(int)accountPos.phone],
                                SageAccountNumber = "",
                                PrimaryContactReference = "",
                                SENSupportFreq = "",
                            };

                            db.CRM_Schools.InsertOnSubmit(school);
                            db.SubmitChanges();


                            CRM_SchoolKeystage keystage = new CRM_SchoolKeystage()
                            {
                                CRM_SchoolID = school.ID,
                                CRM_KeyStageID = stage.ID,
                            };

                            db.CRM_SchoolKeystages.InsertOnSubmit(keystage);
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