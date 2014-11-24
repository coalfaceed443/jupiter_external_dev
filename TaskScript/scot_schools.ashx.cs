using CRM.Code.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CRM.TaskScript
{
    /// <summary>
    /// Summary description for scot_schools
    /// </summary>
    public class scot_schools : IHttpHandler
    {

        private enum accountPos : int
        {
            to, name, address1, address2, postcodea, postcodeb
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

            using (StreamReader reader = new StreamReader(Server.MapPath("/taskscript/scotschools.csv")))
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

                        string postcode = contentsSplit[(int)accountPos.postcodea] + " " + contentsSplit[(int)accountPos.postcodeb];

                        CRM_School school = db.CRM_Schools.FirstOrDefault(s => s.Name == contentsSplit[(int)accountPos.name]
                            && s.CRM_Address.Postcode.Trim().Replace(" ", "").ToLower() == postcode.Trim().Replace(" ", "").ToLower());

                        CRM_LEA lea = db.CRM_LEAs.Single(s => s.ID == 25); // blank
                        CRM_SchoolType type = db.CRM_SchoolTypes.Single(s => s.ID == 1); // none
                        Country country = db.Countries.Single(s => s.ID == 1); // uk

                        if (school == null)
                        {
                            CRM_Address address = new CRM_Address()
                            {
                                AddressLine1 = contentsSplit[(int)accountPos.address1].Trim(),
                                AddressLine2 = "",
                                AddressLine3 = "",
                                AddressLine4 = "",
                                AddressLine5 = "",
                                Town = contentsSplit[(int)accountPos.address2],
                                County = "",
                                Postcode = postcode,
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
                                Email = "",
                                IsArchived = false,
                                IsSEN = false,
                                Name = contentsSplit[(int)accountPos.name].Trim(),
                                Phone = "",
                                SageAccountNumber = "",
                                PrimaryContactReference = "",
                                SENSupportFreq = "",
                            };

                            db.CRM_Schools.InsertOnSubmit(school);
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