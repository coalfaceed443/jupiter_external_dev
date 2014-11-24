using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;
using CRM.Controls.Forms.Handlers;

namespace CRM.Code.Models
{
    public partial class CRM_FamilyPerson : IHistory, ICRMRecord, INotes, ICRMContext, IMailable
    {
        public object ShallowCopy()
        {
            return (CRM_FamilyPerson)this.MemberwiseClone();
        }

        [IsListData("Family Name")]
        public string DisplayFamily
        {
            get
            {
                return this.CRM_Family.Name;
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

        [IsListData("Remove Family Member")]
        public string RemoveFamilyMember
        {
            get
            {
                return Utils.Text.Text.ConvertUrlsToLinks(RemoveFromFamilyURL, "Remove");
            }
        }
        
        [IsListData("Address Line One")]
        public string Address1
        {
            get
            {
                return this.CRM_Person.Address1;
            }
        }

        [IsListData("Address Line Two")]
        public string Address2
        {
            get
            {
                return this.CRM_Person.Address2;
            }
        }

        [IsListData("Address Line Three")]
        public string Address3
        {
            get
            {
                return this.CRM_Person.Address3;
            }
        }

        [IsListData("Town")]
        public string Town
        {
            get
            {
                return this.CRM_Person.CRM_Address.Town;
            }
        }

        [IsListData("County")]
        public string County
        {
            get
            {
                return this.CRM_Person.CRM_Address.County;
            }
        }

        [IsListData("Postcode")]
        public string Postcode
        {
            get
            {
                return this.CRM_Person.CRM_Address.Postcode;
            }
        }

        public string RemoveFromFamilyURL
        {
            get
            {
                return ActionLink.FormURL(ActionLink.Route.RemoveFamilyPerson, this.ID, HttpContext.Current.Request.RawUrl);
            }
        }

        [IsListData("Member Name")]
        public string MemberName
        {
            get
            {
                return this.CRM_Person.Fullname;
            }
        }

        [IsListData("Member DoB")]
        public string MemberDoB
        {
            get
            {
                return this.CRM_Person.DateOfBirthOutput;
            }
        }


        public string DisplayName
        {
            get
            {
                return OutputTableName + " : " + this.CRM_Family.Name;
            }
        }

        public string OutputTableName
        {
            get
            {
                return "Person Family Record";
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
                return Utils.Text.Text.ConvertUrlsToLinks(DetailsURL, "View");
            }
        }

        
        public string DetailsURL
        {
            get
            {
                return "/admin/person/families/details.aspx?id=" + this.CRM_PersonID + "&pid=" + this.ID;
            }
        }

    }
}