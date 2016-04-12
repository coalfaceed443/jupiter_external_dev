using CRM.Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace CRM.Code.Models
{
    public partial class CRM_PersonRelationship : INotes, IHistory
    {

        public CRM_RelationCode PersonACode
        {
            get
            {
                return this.CRM_RelationCode;
            }
            set
            {
                this.CRM_RelationCode = value;
            }
        }


        public int ParentID
        {
            get
            {
                return 0;
            }
        }

        public string TableName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        public CRM_RelationCode PersonBCode
        {

            get
            {
                return this.CRM_RelationCode1;
            }
            set
            {
                this.CRM_RelationCode1 = value;
            }
        }

        public CRM_Person PersonA
        {
            get
            {
                return this.CRM_Person;
            }
            set
            {
                this.CRM_Person = value;
            }
        }
        
        [IsListData("Relationship to B")]        
        public string RelationshipToB
        {
            get
            {
                return PersonACode.Name;
            }
        }

        [IsListData("Relationship to A")]
        public string RelationshipToA
        {
            get
            {
                return PersonBCode.Name;
            }
        }

        [IsListData("Contact A Title")]
        public string TitleA
        {
            get
            {
                return PersonA.Title;
            }
        }

        [IsListData("Contact A Firstname")]
        public string FirstnameA
        {
            get
            {
                return PersonA.Firstname;
            }
        }

        [IsListData("Contact A Lastname")]
        public string LastnameA
        {
            get
            {
                return PersonA.Lastname;
            }
        }

        [IsListData("Contact B Title")]
        public string TitleB
        {
            get
            {
                return PersonB.Title;
            }
        }

        [IsListData("Contact B Firstname")]
        public string FirstnameB
        {
            get
            {
                return PersonB.Firstname;
            }
        }

        [IsListData("Contact B Lastname")]
        public string LastnameB
        {
            get
            {
                return PersonB.Lastname;
            }
        }

        [IsListData("Address 1")]
        public string Address1
        {
            get
            {
                return this.Address.AddressLine1;
            }
        }

        [IsListData("Address 2")]
        public string Address2
        {
            get
            {
                return this.Address.AddressLine2;
            }
        }

        [IsListData("Address 3")]
        public string Address3
        {
            get
            {
                return this.Address.AddressLine3;
            }
        }

        [IsListData("Address 4")]
        public string Address4
        {
            get
            {
                return this.Address.AddressLine4;
            }
        }

        [IsListData("Address 5")]
        public string Address5
        {
            get
            {
                return this.Address.AddressLine5;
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
                return "/admin/person/relation/details.aspx?pid=" + this.ID + "&id=" + this.PersonA.ID;
            }
        }

        [IsListData("Person A List")]
        public string PersonAList
        {
            get
            {
                return "<a href=\"" + this.PersonA.RelationListURL + "\">View</a>";
            }
        }

        [IsListData("Person B List")]
        public string PersonBList
        {
            get
            {
                return "<a href=\"" + this.PersonB.RelationListURL + "\">View</a>";
            }
        }
        
        

        [IsListData("Town")]
        public string Town
        {
            get
            {
                return this.Address.Town;
            }
        }

        [IsListData("County")]
        public string County
        {
            get
            {
                return this.Address.County;
            }
        }


        [IsListData("Postcode")]
        public string Postcode
        {
            get
            {
                return this.Address.Postcode;
            }
        }

        [IsListData("Country")]
        public string Country
        {
            get
            {
                return this.Address.Country.Name;
            }
        }

        public CRM_Person PersonB
        {
            get
            {
                return this.CRM_Person1;
            }
            set
            {
                this.CRM_Person1 = value;
            }
        }

        public CRM_Address Address
        {
            get
            {
                return this.CRM_Person2.PrimaryAddress;
            }
        }

        public string DisplayName
        {
            get
            {
                return OutputTableName + " : " + this.Salutation;
            }
        }

        public string OutputTableName
        {
            get
            {
                return "Relationship";
            }
        }

        public ListItem ListItem
        {
            get
            {
                return new ListItem(this.Salutation, this.ID.ToString());
            }
        }

        public object ShallowCopy()
        {
            return (CRM_PersonRelationship)this.MemberwiseClone();
        }

    }
}