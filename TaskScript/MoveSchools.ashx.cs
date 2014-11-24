using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;
using CRM.Code.Utils.Time;
using System.IO;

namespace CRM.TaskScript
{
    /// <summary>
    /// Summary description for MoveSchools
    /// </summary>
    public class MoveSchools : IHttpHandler
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

                        CRM_Person person = db.CRM_Persons.FirstOrDefault(f => f.LegacyID != null && f.LegacyID.ToString() == contentsSplit[(int)accountPos.LegacyID]);

                        if (person != null && contentsSplit[(int)accountPos.Constituent] == "Schools and universities")
                        {

                            /*
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
                            */

                            
                            if (!String.IsNullOrEmpty(contentsSplit[(int)accountPos.Address1]))
                            {

                                CRM_School org = db.CRM_Schools.FirstOrDefault(f => f.Name == contentsSplit[(int)accountPos.Address1]);

                                if (org == null)
                                {
                                    string name = "";

                                        CRM_Address orgAddress = new CRM_Address();
                                    if (!String.IsNullOrEmpty(contentsSplit[(int)accountPos.OrgAddr1].Trim()))
                                    {
                                        orgAddress = new CRM_Address()
                                        {
                                            AddressLine1 = contentsSplit[(int)accountPos.OrgAddr1],
                                            AddressLine2 = contentsSplit[(int)accountPos.OrgAddr2],
                                            AddressLine3 = contentsSplit[(int)accountPos.OrgAddr3],
                                            AddressLine4 = contentsSplit[(int)accountPos.OrgAddr4],
                                            AddressLine5 = "",
                                            County = "",
                                            Town = contentsSplit[(int)accountPos.OrgAddrCity],
                                            Postcode = contentsSplit[(int)accountPos.OrgAddrPostcode],
                                            CountryID = 1
                                        };

                                        db.CRM_Addresses.InsertOnSubmit(orgAddress);
                                        db.SubmitChanges();

                                        name = contentsSplit[(int)accountPos.OrgName];
                                    }
                                    else
                                    {
                                        
                                            orgAddress = new CRM_Address()
                                            {
                                            AddressLine1 = contentsSplit[(int)accountPos.Address1],
                                            AddressLine2 = contentsSplit[(int)accountPos.Address2],
                                            AddressLine3 = contentsSplit[(int)accountPos.Address3],
                                            AddressLine4 = contentsSplit[(int)accountPos.Address4],
                                            AddressLine5 = "",
                                            County = "",
                                            Town = contentsSplit[(int)accountPos.City],
                                            Postcode = contentsSplit[(int)accountPos.Postcode],
                                            CountryID = 1
                                            };
                                        

                                        db.CRM_Addresses.InsertOnSubmit(orgAddress);
                                        db.SubmitChanges();
                                        name = contentsSplit[(int)accountPos.Address1];
                                    }

                                    org = new CRM_School()
                                    {
                                        CRM_SchoolTypeID = 1,
                                        CRM_AddressID = orgAddress.ID,
                                        Name = name,
                                        IsArchived = false,
                                        PrimaryContactReference = person.Reference,
                                        SENSupportFreq = "",
                                        SageAccountNumber = "",
                                        Phone = "",
                                        Email = ""
                                    };
                                    db.CRM_Schools.InsertOnSubmit(org);
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

                                CRM_PersonSchool personOrg = new CRM_PersonSchool()
                                {
                                    CRM_SchoolID = org.ID,
                                    CRM_RoleID = role.ID,
                                    IsArchived = false,
                                    Email = "",
                                    Telephone = contentsSplit[(int)accountPos.PhoneNumber21],
                                    CRM_PersonID = person.ID
                                };

                                db.CRM_PersonSchools.InsertOnSubmit(personOrg);
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