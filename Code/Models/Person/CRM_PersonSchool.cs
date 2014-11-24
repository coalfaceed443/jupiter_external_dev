using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;

namespace CRM.Code.Models
{
    public partial class CRM_PersonSchool : IHistory, ICRMRecord, INotes, IContact, IAutocomplete, ICRMContext, IMailable
    {
        public object ShallowCopy()
        {
            return (CRM_PersonSchool)this.MemberwiseClone();
        }

        [IsListData("School")]
        public string DisplaySchool
        {
            get
            {
                return this.CRM_School.Name + " (" + this.CRM_School.CRM_Address.Town + ")";
            }
        }

        public string Photo
        {
            get
            {
                return Parent_CRM_Person.OutputPersonImageURL;
            }
        }

        public bool IsMailable
        {
            get
            {
                return Parent_CRM_Person.IsMailable;
            }
        }

        public bool IsEmailable
        {
            get
            {
                return Parent_CRM_Person.IsEmailable;
            }
        }


        public CRM_Person Parent_CRM_Person
        {
            get
            {
                return this.CRM_Person;
            }
        }

        public string Name
        {
            get
            {
                return this.Fullname;
            }
        }

        public string Fullname
        {
            get
            {
                return this.ArchivedOutput + this.CRM_Person.Fullname + " : " + this.CRM_School.Name;
            }
        }


        [IsListData("Title")]
        public string Title
        {
            get
            {
                return this.CRM_Person.Title;
            }
        }

        [IsListData("Firstname")]
        public string Firstname
        {
            get
            {
                return this.CRM_Person.Firstname;
            }
        }

        [IsListData("Lastname")]
        public string Lastname
        {
            get
            {
                return this.CRM_Person.Lastname;
            }
        }



        public string CRM_PersonReference
        {
            get
            {
                return this.CRM_Person.Reference;
            }
        }


        public CRM_Address PrimaryAddress
        {
            get
            {
                return this.CRM_School.CRM_Address;
            }
        }



        public List<string> Tokens
        {
            get
            {
                List<string> tokens = new List<string>();
                tokens.AddRange(this.PrimaryAddress.Tokens);
                tokens.AddRange(this.CRM_Person.Tokens);
                return tokens;
            }
        }

        [IsListData("Role")]
        public string DisplayRole
        {
            get
            {
                return this.CRM_Role.Name;
            }
        }

        [IsListData("School Address")]
        public string SchoolAddress
        {
            get
            {
                return this.CRM_School.CRM_Address.FormattedAddress;
            }
        }

        [IsListData("Person")]
        public string PersonName
        {
            get
            {
                return this.CRM_Person.Name;
            }
        }

        public string DisplayName
        {
            get
            {
                return OutputTableName + " : " + this.CRM_School.Name + " : " + this.CRM_Role.Name;
            }
        }

        public string OutputTableName
        {
            get
            {
                return "Person School Record";
            }
        }




        [IsListData("Person Address 1")]
        public string Address1
        {
            get
            {
                return this.CRM_Person.Address1;
            }
        }

        [IsListData("Person Address 2")]
        public string Address2
        {
            get
            {
                return this.CRM_Person.Address2;
            }
        }

        [IsListData("Person Address 3")]
        public string Address3
        {
            get
            {
                return this.CRM_Person.Address3;
            }
        }

        [IsListData("Person Postcode")]
        public string Postcode
        {
            get
            {
                return this.CRM_Person.Postcode;
            }
        }

        [IsListData("School Address 1")]
        public string OrgAddress1
        {
            get
            {
                return this.CRM_School.CRM_Address.AddressLine1;
            }
        }

        [IsListData("School Address 2")]
        public string OrgAddress2
        {
            get
            {
                return this.CRM_School.CRM_Address.AddressLine2;
            }
        }

        [IsListData("School Address 3")]
        public string OrgAddress3
        {
            get
            {
                return this.CRM_School.CRM_Address.AddressLine3;
            }
        }

        [IsListData("School Postcode")]
        public string OrgPostcode
        {
            get
            {
                return this.CRM_School.CRM_Address.Postcode;
            }
        }

        [IsListData("School Email")]
        public string SchoolEmail
        {
            get
            {
                return this.CRM_School.Email;
            }
        }


        public string TableName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        public int ParentID
        {
            get
            {
                return this.CRM_PersonID;
            }
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
                return "/admin/person/schools/details.aspx?id=" + this.CRM_PersonID + "&pid=" + this.ID;
            }
        }

        public static IEnumerable<CRM_PersonSchool> BaseSet(MainDataContext db)
        {
            return CRM_Person.BaseSet(db).SelectMany(c => c.CRM_PersonSchools);
        }

        [IsListData("Is Archived")]
        public string ArchivedOutput
        {
            get
            {
                return this.IsArchived ? " [ARCHIVED] " : "";
            }
        }

    }
}