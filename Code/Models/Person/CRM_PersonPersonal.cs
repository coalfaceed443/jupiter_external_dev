using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;
using CRM.Code.Helpers;

namespace CRM.Code.Models
{
    public partial class CRM_PersonPersonal : ICRMRecord, IHistory, INotes, IContact, IAutocomplete, ICRMContext, IMailable
    {
        public object ShallowCopy()
        {
            return (CRM_PersonPersonal)this.MemberwiseClone();
        }

        public string DisplayName
        {
            get
            {
                return OutputTableName + " : " + this.Description;
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
                return this.CRM_Person.Reference;
            }
        }

        public List<string> Tokens
        {
            get
            {
                List<string> tokens = new List<string>();
                tokens.AddRange(this.CRM_Address.Tokens);
                tokens.AddRange(this.CRM_Person.Tokens);
                return tokens;
            }
        }


        public string Name
        {
            get
            {
                return this.Fullname;
            }
        }


        public string Title
        {
            get
            {
                return this.CRM_Person.Title;
            }
        }


        public string Firstname
        {
            get
            {
                return this.CRM_Person.Firstname;
            }
        }

        public string Lastname
        {
            get
            {
                return this.CRM_Person.Lastname;
            }
        }

        public string Fullname
        {
            get
            {
                return this.ArchivedOutput + this.CRM_Person.Fullname;
            }
        }

        public string OutputTableName
        {
            get
            {
                return "Personal Record";
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
                return "/admin/person/personal/details.aspx?id=" + this.CRM_PersonID + "&pid=" + this.ID;
            }
        }

        [IsListData("Address")]
        public string FormattedAddress
        {
            get
            {
                return this.CRM_Address.FormattedAddress;
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

    }
}