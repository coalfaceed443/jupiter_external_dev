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

namespace CRM.Code.Models
{
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
            Postcode
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
                    {(byte)SearchKeys.Firstname, this.Firstname},
                    {(byte)SearchKeys.Lastname, this.Lastname},
                    {(byte)SearchKeys.DoB, this.DateOfBirthOutput},
                    {(byte)SearchKeys.PrimaryEmail, this.PrimaryEmail},
                    {(byte)SearchKeys.Telephone, this.PrimaryTelephone},
                    {(byte)SearchKeys.Postcode, this.Postcode}
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
            return db.CRM_Persons;
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
    }
}