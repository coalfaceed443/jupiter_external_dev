using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Helpers;
using CRM.Code.Interfaces;
using System.Web.UI.WebControls;
using System.IO;
using CRM.Code.Utils.List;
using CRM.Code.Utils.Enumeration;
using System.Text;

namespace CRM.Code.Models
{
    public static class PersonExtension
    {
        public static string[] ConstituentTypes(this CRM_Person person, MainDataContext db)
        {
            IEnumerable<CRM_FormFieldResponse> answers = db.CRM_FormFieldResponses.Where(f => f.TargetReference == person.Reference && f.CRM_FormFieldID == 1);

            if (answers != null)
            {
                
                return answers.Select(s => s.CRM_FormFieldItem.Label).ToArray();
            }
            else return new string[] { };

        }
    }
    public partial class CRM_Person : IHistory, ICRMRecord, INotes, IDuplicate, IAutocomplete, IContact, ICustomField, ICRMContext, IMailable
    {
        public const string PlaceholderPhoto = PhotoDirectory + "placeholder" + PhotoFileExtension;
        public const string PhotoDirectory = "/_assets/media/persons/photos/";
        private const string PhotoFileExtension = ".png";
        public string PersonImageURL
        {
            get
            {
                return PhotoDirectory + this.ID + PhotoFileExtension;
            }
        }

        public int AddressID
        {
            get
            {
                return this.PrimaryAddressRecord.ID;
            }
        }

        private const string MergeCardFormat = "<span data-merge='{0}'>{1}</span>";

        public string MergeCard
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(String.Format(MergeCardFormat, "reference", this.Reference));
                sb.AppendLine(String.Format(MergeCardFormat, "title", this.Title) + " "
                    + String.Format(MergeCardFormat, "firstname", this.Firstname) + " "
                    + String.Format(MergeCardFormat, "lastname", this.Lastname));
                sb.AppendLine(String.Format(MergeCardFormat, "primaryemail", this.PrimaryEmail));
                sb.AppendLine(String.Format(MergeCardFormat, "address1", this.Address1));
                sb.AppendLine(String.Format(MergeCardFormat, "address2", this.Address2));
                sb.AppendLine(String.Format(MergeCardFormat, "address3", this.Address3));
                sb.AppendLine(String.Format(MergeCardFormat, "town", this.Town));
                sb.AppendLine(String.Format(MergeCardFormat, "county", this.County));
                sb.AppendLine(String.Format(MergeCardFormat, "postcode", this.Postcode));
                sb.AppendLine(String.Format(MergeCardFormat, "tel", this.PrimaryTelephone));

                return sb.ToString();
            }
        }

        #region Filtered Relationships

        public IEnumerable<CRM_PersonPersonal> PersonalRecords
        {
            get
            {
                return this.CRM_PersonPersonals.Where(r => !r.IsArchived);
            }
        }

        public IEnumerable<CRM_PersonOrganisation> OrganisationRecords
        {
            get
            {
                return this.CRM_PersonOrganisations.Where(r => !r.IsArchived);
            }
        }

        public IEnumerable<CRM_PersonSchool> SchoolRecords
        {
            get
            {
                return this.CRM_PersonSchools.Where(r => !r.IsArchived);
            }
        }

        #endregion

        #region Person Address

        [IsListData("Address 1")]
        public string Address1
        {
            get
            {
                return this.PrimaryAddress.AddressLine1;
            }
        }

        [IsListData("Address 2")]
        public string Address2
        {
            get
            {
                return this.PrimaryAddress.AddressLine2;
            }
        }

        [IsListData("Address 3")]
        public string Address3
        {
            get
            {
                return this.PrimaryAddress.AddressLine3;
            }
        }

        [IsListData("Constituent ID")]
        public int ConstituentID
        {
            get
            {
                return this.LegacyID == null ? 0 : (int)this.LegacyID;
            }
        }


        [IsListData("Town")]
        public string Town
        {
            get
            {
                return this.PrimaryAddress.Town;
            }
        }

        [IsListData("Address County")]
        public string County
        {
            get
            {
                return this.PrimaryAddress.County;
            }
        }

        [IsListData("Address Postcode")]
        public string Postcode
        {
            get
            {
                return this.PrimaryAddress.Postcode;
            }
        }

        [IsListData("Address Country")]
        public string Country
        {
            get
            {
                return this.PrimaryAddress.Country.Name;
            }
        }

        #endregion 
        
        #region Organisation Address

        [IsListData("Organisation Name")]
        public string OrganisationName
        {
            get
            {
                return PrimaryOrganisation != null ? PrimaryOrganisation.CRM_Organisation.Name : "";
            }
        }

        [IsListData("Organisation Role")]
        public string OrganisationRole
        {
            get
            {
                return PrimaryOrganisation != null ? PrimaryOrganisation.CRM_Role.Name : "";
            }
        }

        [IsListData("Organisation Address 1")]
        public string OrganisationAddress1
        {
            get
            {
                return PrimaryOrganisation != null ? PrimaryOrganisation.PrimaryAddress.AddressLine1 : "";
            }
        }

        [IsListData("Organisation Address 2")]
        public string OrganisationAddress2
        {
            get
            {
                return PrimaryOrganisation != null ? PrimaryOrganisation.PrimaryAddress.AddressLine2 : "";
            }
        }

        [IsListData("Organisation Address 3")]
        public string OrganisationAddress3
        {
            get
            {
                return PrimaryOrganisation != null ? PrimaryOrganisation.PrimaryAddress.AddressLine3 : "";
            }
        }

        [IsListData("Organisation Address 4")]
        public string OrganisationAddress4
        {
            get
            {
                return PrimaryOrganisation != null ? PrimaryOrganisation.PrimaryAddress.AddressLine4 : "";
            }
        }

        [IsListData("Organisation Address 5")]
        public string OrganisationAddress5
        {
            get
            {
                return PrimaryOrganisation != null ? PrimaryOrganisation.PrimaryAddress.AddressLine5 : "";
            }
        }

        [IsListData("Organisation Town")]
        public string OrganisationAddressTown
        {
            get
            {
                return PrimaryOrganisation != null ? PrimaryOrganisation.PrimaryAddress.Town : "";
            }
        }

        [IsListData("Organisation County")]
        public string OrganisationAddressCounty
        {
            get
            {
                return PrimaryOrganisation != null ? PrimaryOrganisation.PrimaryAddress.County : "";
            }
        }


        [IsListData("Organisation Country")]
        public string OrganisationAddressCountry
        {
            get
            {
                return PrimaryOrganisation != null ? PrimaryOrganisation.PrimaryAddress.Country.Name : "";
            }
        }

        [IsListData("Organisation Postcode")]
        public string OrganisationAddressPostcode
        {
            get
            {
                return PrimaryOrganisation != null ? PrimaryOrganisation.PrimaryAddress.Postcode : "";
            }
        }
        #endregion 

        #region Relationships

        public int? RelationshipID
        {
            get
            {
                if (this.PrimaryRelation != null)
                    return this.PrimaryRelation.CRM_PersonIDAddress;
                else return null;
            }
        }

        public IEnumerable<CRM_PersonRelationship> Relationships
        {
            get
            {
                return this.CRM_PersonRelationships.Where(c => !c.IsArchived).Concat(this.CRM_PersonRelationships1.Where(c => !c.IsArchived));
            }
        }

        [IsListData("Has Relationships")]
        public bool HasRelations
        {
            get
            {
                return this.Relationships.Any(c => !c.IsArchived);
            }
        }


        [IsListData("Primary Relationship")]
        public string PrimaryRelationshipOutput
        {
            get
            {
                string output = "";
                if (HasRelations)
                {
                    if (PrimaryRelation.PersonACode.Name != "" && PrimaryRelation.PersonBCode.Name != "")
                    {
                        output = PrimaryRelation.PersonACode.Name + " and " + PrimaryRelation.PersonBCode.Name;
                    }
                }

                return output;
            }
        }


        [IsListData("Relation Address 1")]
        public string RelationshipAddress1
        {
            get
            {
                return PrimaryRelation != null ? PrimaryRelation.Address1 : Address1;
            }
        }

        [IsListData("Relation Address 2")]
        public string RelationshipAddress2
        {
            get
            {
                return PrimaryRelation != null ? PrimaryRelation.Address2 : Address2;
            }
        }

        [IsListData("Relation Address 3")]
        public string RelationshipAddress3
        {
            get
            {
                return PrimaryRelation != null ? PrimaryRelation.Address3 : Address3;
            }
        }

        [IsListData("Relation Address 4")]
        public string RelationshipAddress4
        {
            get
            {
                return PrimaryRelation != null ? PrimaryRelation.Address4 : PrimaryAddress.AddressLine4;
            }
        }

        [IsListData("Relation Address 5")]
        public string RelationshipAddress5
        {
            get
            {
                return PrimaryRelation != null ? PrimaryRelation.Address5 : PrimaryAddress.AddressLine5;
            }
        }


        [IsListData("Relation Town")]
        public string RelationshipTown
        {
            get
            {
                return PrimaryRelation != null ? PrimaryRelation.Town : PrimaryAddress.Town;
            }
        }

        [IsListData("Relation County")]
        public string RelationshipCounty
        {
            get
            {
                return PrimaryRelation != null ? PrimaryRelation.County : PrimaryAddress.County;
            }
        }

        [IsListData("Relation Country")]
        public string RelationshipCountry
        {
            get
            {
                return PrimaryRelation != null ? PrimaryRelation.Country : Country;
            }
        }

        [IsListData("Relation Postcode")]
        public string RelationshipPostcode
        {
            get
            {
                return PrimaryRelation != null ? PrimaryRelation.Postcode : Postcode;
            }
        }


        [IsListData("Relation Salutation")]
        public string RelationshipSaltuation
        {
            get
            {
                return PrimaryRelation != null ? PrimaryRelation.Salutation : CalculatedSalutation;
            }
        }

        public CRM_PersonRelationship PrimaryRelation
        {
            get
            {
                return this.Relationships.FirstOrDefault();
            }
        }

        public string RelationListURL
        {
            get
            {
                return "/admin/person/relation/list.aspx?id=" + this.ID;
            }

        }

        #endregion 

        #region Annual Passes

        [IsListData("Annual Pass Holder")]
        public bool IsAnnualPassHolder
        {
            get
            {
                return ActivePasses.Any();
            }
        }

        [IsListData("Primary Contact Annual Pass Holder")]
        public bool IsPrimaryAnnualPassHolder
        {
            get
            {
                return this.NextToRenewActivePass != null && this.NextToRenewActivePass.PrimaryContactReference == this.Reference;
            }
        }

        public CRM_AnnualPass NextToRenewActivePass
        {
            get
            {
                return ActivePasses.OrderBy(c => c.ExpiryDate).FirstOrDefault();
            }
        }

        public Int64 NextExpiryDate_T
        {
            get
            {
                return NextExpiryDateTime.Ticks;
            }
        }

        public DateTime NextExpiryDateTime
        {
            get
            {
                if (IsAnnualPassHolder && NextToRenewActivePass != null)
                {
                    return NextToRenewActivePass.ExpiryDate;
                }
                else
                {
                    return new DateTime(2001, 01, 01);
                }
            }
        }

        [IsListData("Next Annual Pass Expiry")]
        public string NextExpiryDate
        {
            get
            {
                if (IsAnnualPassHolder && NextToRenewActivePass != null)
                {
                    return ActivePasses.OrderBy(c => c.ExpiryDate).First().ExpiryDate.ToString("dd/MM/yyyy");
                }
                else
                {
                    return "All Expired";
                }
            }
        }

        public IEnumerable<CRM_AnnualPass> ActivePasses
        {
            get
            {
                return this.CRM_AnnualPassPersons.Where(c => c.CRM_AnnualPass.ExpiryDate >= DateTime.Now && !c.IsArchived && !c.CRM_AnnualPass.IsArchived).Select(c => c.CRM_AnnualPass);
            }
        }

        #endregion

        #region Primary Address

        public class AddressPoolItem
        {
            public CRM_Address CRM_Address { get; set; }
            public string InternalName { get; set; }

            public int ID
            {
                get
                {
                    return CRM_Address.ID;
                }
            }

            public AddressPoolItem(CRM_Address cRM_Address, string internalName)
            {
                CRM_Address = cRM_Address;
                InternalName = internalName;
            }
        }


        /// <summary>
        /// Pools all the addresses against the person, through the various db relations
        /// </summary>
        public List<AddressPoolItem> AddressPool
        {
            get
            {
                List<AddressPoolItem> pool = new List<AddressPoolItem>();
                pool.Add(new AddressPoolItem(this.CRM_Address, "Person Address - " + this.CRM_Address.FormattedAddress));
                
                // personal addresses

                foreach (CRM_PersonPersonal record in PersonalRecords.Where(r => !r.IsArchived && r.IsMailable))
                {
                    pool.Add(new AddressPoolItem(record.CRM_Address, "Personal Address - " + record.Name + " - " + record.CRM_Address.FormattedAddress));
                }

                // organisation addresses

                foreach (CRM_PersonOrganisation record in OrganisationRecords.Where(r => !r.IsArchived && r.IsMailable))
                {
                    pool.Add(new AddressPoolItem(record.PrimaryAddress, "Organisation Address - " + record.Name + " - " + record.CRM_Role.Name + "  - " + record.PrimaryAddress.FormattedAddress));
                }

                // school addresses

                foreach (CRM_PersonSchool record in SchoolRecords.Where(r => !r.IsArchived && r.IsMailable))
                {
                    pool.Add(new AddressPoolItem(record.PrimaryAddress, "School Address - " + record.Name + " - " + record.PrimaryAddress.FormattedAddress));
                }

                return pool;
            }
        }

        /// <summary>
        /// bad naming conventions, we now have 2 conflicting 'primary address' pointers.  PrimaryAddressRecord should be used for reporting, and is read only.
        /// PrimaryAddress is full access but points to person address.
        /// </summary>
        public CRM_Address PrimaryAddressRecord
        {
            get
            {
                return this.CRM_Address1 == null ? this.PrimaryAddress : this.CRM_Address1;
            }
        }

        [IsListData("Primary Address 1")]
        public string PrimaryAddressLine1
        {
            get
            {
                return this.PrimaryAddressRecord.AddressLine1;
            }
        }


        [IsListData("Primary Address 2")]
        public string PrimaryAddressLine2
        {
            get
            {
                return this.PrimaryAddressRecord.AddressLine2;
            }
        }


        [IsListData("Primary Address 3")]
        public string PrimaryAddressLine3
        {
            get
            {
                return this.PrimaryAddressRecord.AddressLine3;
            }
        }


        [IsListData("Primary Address 4")]
        public string PrimaryAddressLine4
        {
            get
            {
                return this.PrimaryAddressRecord.AddressLine4;
            }
        }

        [IsListData("Primary Address 5")]
        public string PrimaryAddressLine5
        {
            get
            {
                return this.PrimaryAddressRecord.AddressLine4;
            }
        }

        [IsListData("Primary Address Town")]
        public string PrimaryAddressTown
        {
            get
            {
                return this.PrimaryAddressRecord.Town;
            }
        }

        [IsListData("Primary Address County")]
        public string PrimaryAddressCounty
        {
            get
            {
                return this.PrimaryAddressRecord.County;
            }
        }

        [IsListData("Primary Address Country")]
        public string PrimaryAddressCountry
        {
            get
            {
                return this.PrimaryAddressRecord.Country.Name;
            }
        }

        [IsListData("Primary Address Postcode")]
        public string PrimaryAddressPostcode
        {
            get
            {
                return this.PrimaryAddressRecord.Postcode;
            }
        }

        #endregion 

        public IEnumerable<CRM_CalendarInvite> Invites
        {
            get
            {
                return MainDataContext.CurrentContext.CRM_CalendarInvites.Where(c => c.Reference == this.Reference);
            }
        }

        public bool IsMailable
        {
            get
            {
                return this.IsDoNotMail == false && this.IsDeceased == false && this.IsArchived == false;
            }
        }

        public bool IsEmailable
        {
            get
            {
                return this.IsDoNotEmail == false && this.IsDeceased == false && this.IsArchived == false;
            }
        }

        [IsListData("No. attended")]
        public int TimesAttended
        {
            get
            {
                return Invites.Where(i => i.IsAttended).Count();
            }
        }

        [IsListData("No. invites")]
        public int TimesInvited
        {
            get
            {
                return Invites.Where(i => i.IsInvited).Count();
            }
        }

        [IsListData("No. cancellations")]
        public int TimesCancelled
        {
            get
            {
                return Invites.Where(i => i.IsCancelled).Count();
            }
        }

        [IsListData("Invite to attendance %")]
        public decimal InviteToAttendanceRatio
        {
            get
            {
                if (this.TimesAttended == 0 || this.TimesInvited == 0)
                {
                    return 0;
                }
                else
                    return (((decimal)this.TimesAttended / (decimal)this.TimesInvited)) * 100;
            }
        }

        public IEnumerable<IContact> Contacts
        {
            get
            {
                return Enumerable.Repeat<IContact>(this, 1).Concat(this.CRM_PersonOrganisations.Select(s => (IContact)s)
                    .Concat(this.CRM_PersonSchools.Select(s => (IContact)s))
                    .Concat(this.CRM_PersonPersonals.Select(s => (IContact)s)));
            }
        }

        public IEnumerable<CRM_Fundraising> RelatedFunds
        {
            get
            {
                return MainDataContext.CurrentContext.CRM_Fundraisings.ToArray().Where(c => this.Contacts.Any(ca => ca.Reference == c.PrimaryContactReference));
            }
        }

        [IsListData("Total Funds Given (£)")]
        public decimal ValueOfFunds
        {
            get
            {
                decimal amount = RelatedFunds.Sum(f => f.PledgedAmount);
                return amount;
            }
        }

        public string OutputPersonImageURL
        {
            get
            {
                if (File.Exists(HttpContext.Current.Server.MapPath(this.PersonImageURL)))
                {
                    return this.PersonImageURL;
                }
                else
                {
                    return PlaceholderPhoto;
                }
            }
        }

        public string Photo
        {
            get
            {
                return OutputPersonImageURL;
            }
        }

        [IsListData("Photo")]
        public string PhotoPreviewOutput
        {
            get
            {
                return "<span class=\"help\" title=\"<img src='" + this.OutputPersonImageURL + "\' alt='' />\"><img src=\"/_assets/images/admin/icons/account.png\" alt=\"person icon\" width=\"20\"></span>";
            }
        }

        public CRM_Person Parent_CRM_Person
        {
            get
            {
                return this;
            }
        }

        [IsListData("Is Donor")]
        public string IsDonorOutput
        {
            get
            {
                return this.IsDonor ? "Yes" : "No";
            }
        }

        public bool IsDonor
        {
            get
            {
                return this.RelatedFunds.Any();
            }
        }

        public object ShallowCopy()
        {
            return (CRM_Person)this.MemberwiseClone();
        }

        public string DisplayName
        {
            get
            {
                return OutputTableName + " : " + this.Fullname;
            }
        }

        public string OutputTableName
        {
            get
            {
                return "Person Record";
            }
        }

        public string TableName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        private int _parentID = 0;
        public int ParentID
        {
            get
            {
                return _parentID;
            }
            set
            {
                _parentID = value;
            }
        }

        public List<string> Tokens
        {
            get
            {
                List<string> tokens = new List<string>();
                tokens.AddRange(JSONSet.ConvertToTokens(this.ID.ToString()));
                tokens.AddRange(JSONSet.ConvertToTokens(this.Fullname));
                tokens.AddRange(JSONSet.ConvertToTokens(this.PreviousNames));
                tokens.AddRange(this.CRM_Address.Tokens);
                tokens.AddRange(JSONSet.ConvertToTokens(DateOfBirthOutput));
                return tokens;
            }
        }

        public enum SearchKeys
        {
            Firstname,
            Lastname,
            DoB,
            PrimaryEmail,
            Telephone,
            Postcode,
            Fullname
        }

        public static string DataKey
        {
            get
            { return "data-duplicate"; }
        }

        public IDictionary<int, string> GetKeys()
        {
            return Enumeration.GetAll<SearchKeys>();
        }

        public Dictionary<byte, string> SearchDictionary
        {
            get
            {
                return new Dictionary<byte, string>()
                {
                    {(byte)SearchKeys.DoB, this.DateOfBirthOutput},
                    {(byte)SearchKeys.PrimaryEmail, this.PrimaryEmail},
                    {(byte)SearchKeys.Telephone, this.PrimaryTelephone},
                    {(byte)SearchKeys.Postcode, this.Postcode},
                    {(byte)SearchKeys.Fullname, this.Firstname + " " + this.Lastname}
                };
            }
        }

        public string ShortDateOfBirthOutput
        {
            get
            {
                return this.DateOfBirth != null ? ((DateTime)this.DateOfBirth).ToString("dd MMM") : "";
            }
        }

        public CRM_Address PrimaryAddress
        {
            get
            {
                return this.CRM_Address;
            }
        }

        public string CRM_PersonReference
        {
            get
            {
                return this.Reference;
            }
        }

        public Int64 DateOfBirthOutput_T
        {
            get
            {
                return this.DateOfBirth != null ? ((DateTime)this.DateOfBirth).Ticks : 0;
            }
        }

        public Int64 DateModifiedOutput_T
        {
            get
            {
                return this.DateModified_T;
            }
        }

        [IsListData("Date of Birth")]
        public string DateOfBirthOutput
        {
            get
            {
                return this.DateOfBirth != null ? ((DateTime)this.DateOfBirth).ToString("dd/MM/yyyy") : "";
            }
        }

        public string Name
        {
            get
            {
                return Fullname;
            }
        }


        [IsListData("Date Added")]
        public string DateAddedOutput
        {
            get
            {
                return this.DateAdded.ToString(Constants.DefaultDateStringFormat);
            }
        }

        [IsListData("Last Modified")]
        public string DateModifiedOutput
        {
            get
            {
                return this.DateModified.ToString(Constants.DefaultDateStringFormat);
            }
        }

        public Int64 DateModified_T
        {
            get
            {
                return this.DateModified.Ticks;
            }
        }

        public Int64 DateAdded_T
        {
            get
            {
                return this.DateAdded.Ticks;
            }
        }

        [IsListData(true, "Fullname")]
        public string Fullname
        {
            get
            {
                return (this.IsArchived ? "[ARCHIVED] " : "") + this.Title + " " + this.Firstname.Trim() + " " + this.Lastname.Trim();
            }
        }

        public static IEnumerable<CRM_Person> BaseSet(MainDataContext db)
        {
            return db.CRM_Persons.Where(r => !r.IsArchived);
        }

        public IEnumerable<IDuplicate> GetBaseSet(MainDataContext db)
        {
            return CRM_Person.BaseSet(db).Select(l => (IDuplicate)l);
        }

        [IsListData("View")]
        public string ViewRecord
        {
            get
            {
                return "<a href=\"" + DetailsURL + "\">View</a>";
            }
        }

        public string DetailsURL
        {
            get
            {
                return "/admin/person/details.aspx?id=" + this.ID;
            }
        }

        [IsListData("View Personal")]
        public string ViewPersonalRecord
        {
            get
            {
                return "<a href=\"" + PersonalListURL + "\">View Personal</a>";
            }
        }


        public string PersonalListURL
        {
            get
            {
                return "/admin/person/personal/list.aspx?id=" + this.ID;
            }
        }

        [IsListData("View Organisations")]
        public string ViewOrganisationRecord
        {
            get
            {
                return "<a href=\"" + OrganisationListURL + "\">View Organisations</a>";
            }
        }

        public string OrganisationListURL
        {
            get
            {
                return "/admin/person/organisations/list.aspx?id=" + this.ID;
            }
        }

        [IsListData("View Schools")]
        public string ViewSchoolRecord
        {
            get
            {
                return "<a href=\"" + SchoolListURL + "\">View Schools</a>";
            }
        }

        public string SchoolListURL
        {
            get
            {
                return "/admin/person/schools/list.aspx?id=" + this.ID;
            }
        }

        [IsListData("View Families")]
        public string ViewFamilyRecord
        {
            get
            {
                return "<a href=\"" + FamilyListURL + "\">View Families</a>";
            }
        }

        public string FamilyListURL
        {
            get
            {
                return "/admin/person/families/list.aspx?id=" + this.ID;
            }
        }


        public string GiftRecordListURL
        {
            get
            {
                return "/admin/person/gift/list.aspx?id=" + this.ID;
            }
        }

        public string GiftRecordNewURL
        {
            get
            {
                return "/admin/person/gift/details.aspx?id=" + this.ID;
            }
        }


        public static List<ListItem> GetTitles()
        {
            return new List<ListItem>()
            {
                new ListItem(""),
                new ListItem("Mr"),
                new ListItem("Mrs"),
                new ListItem("Miss"),
                new ListItem("Ms"),
                new ListItem("Mst"),
                new ListItem("Sir"),                
                new ListItem("Dr"),
                new ListItem("Rev"),
                new ListItem("Lord"),
                new ListItem("Lady"),
                new ListItem("Mayor"),
                new ListItem("Rt Hon."),
                new ListItem("Mx")
            };
        }

        public CRM_PersonOrganisation PrimaryOrganisation
        {
            get
            {
                return this.CRM_PersonOrganisations.Where(r => !r.IsArchived && r.IsMailable).FirstOrDefault();
            }
        }

        public string CalculatedSalutation
        {
            get
            {
                return this.Fullname; 
            }
        }
    }
}