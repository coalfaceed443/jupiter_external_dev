using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;

namespace CRM.Code.Models
{
    public partial class CRM_PersonOrganisation : IHistory, ICRMRecord, INotes, IContact, IAutocomplete, ICRMContext, IMailable, ILabel
    {

        #region Ilabel

        public string LabelName
        {
            get
            {
                return this.CRM_Person.Fullname;
            }
        }

        public string LabelOrganisation
        {
            get
            {
                return this.CRM_Organisation.Name;
            }
        }

        public string LabelAddress
        {
            get
            {
                return this.PrimaryAddress.LabelOutput(this.LabelName, this.LabelOrganisation);
            }
        }

        public int LabelCRM_AddressID
        {
            get
            {
                return this.PrimaryAddress.ID;
            }
        }


        public bool LabelIsPrimaryAddress
        {
            get
            {
                return this.CRM_Person.PrimaryAddressID == this.CRM_Organisation.CRM_AddressID;
            }
        }


        #endregion 

        public object ShallowCopy()
        {
            return (CRM_PersonOrganisation)this.MemberwiseClone();
        }

        [IsListData("Organisation")]
        public string DisplayOrganisation
        {
            get
            {
                return this.CRM_Organisation.Name;
            }
        }

        public int AddressID
        {
            get
            {
                return this.PrimaryAddress.ID;
            }
        }


        public int? RelationshipID
        {
            get
            {
                return null;
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

        [IsListData("Role")]
        public string DisplayRole
        {
            get
            {
                return this.CRM_Role.Name;
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

        [IsListData("Org Address 1")]
        public string OrgAddress1
        {
            get
            {
                return this.CRM_Organisation.CRM_Address.AddressLine1;
            }
        }

        [IsListData("Org Address 2")]
        public string OrgAddress2
        {
            get
            {
                return this.CRM_Organisation.CRM_Address.AddressLine2;
            }
        }

        [IsListData("Org Address 3")]
        public string OrgAddress3
        {
            get
            {
                return this.CRM_Organisation.CRM_Address.AddressLine3;
            }
        }

        [IsListData("Org Postcode")]
        public string OrgPostcode
        {
            get
            {
                return this.CRM_Organisation.CRM_Address.Postcode;
            }
        }

        public string CRM_PersonReference
        {
            get
            {
                return this.CRM_Person.Reference;
            }
        }


        public List<string> Tokens
        {
            get
            {
                List<string> tokens = new List<string>();
                tokens.AddRange(this.PrimaryAddress.Tokens);
                tokens.AddRange(this.CRM_Person.Tokens);
                tokens.AddRange(this.CRM_Organisation.Tokens);
                return tokens;
            }
        }

        public CRM_Address PrimaryAddress
        {
            get
            {
                return this.CRM_Organisation.CRM_Address;
            }
        }


        public string Fullname
        {
            get
            {
                return this.ArchivedOutput + this.CRM_Person.Fullname + " : " + this.CRM_Organisation.Name;
            }
        }

        public string DisplayName
        {
            get
            {
                return OutputTableName + " : " + this.CRM_Organisation.Name + " : " + this.CRM_Role.Name;
            }
        }

        public string OutputTableName
        {
            get
            {
                return "Person Organisation Record";
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
                return "/admin/person/organisations/details.aspx?id=" + this.CRM_PersonID + "&pid=" + this.ID;
            }
        }

        [IsListData("Is Archived")]
        public string ArchivedOutput
        {
            get
            {
                return this.IsArchived ? " [ARCHIVED] " : "";
            }
        }


        public static IEnumerable<CRM_PersonOrganisation> BaseSet(MainDataContext db)
        {
            return CRM_Person.BaseSet(db).SelectMany(c => c.CRM_PersonOrganisations);
        }


    }
}