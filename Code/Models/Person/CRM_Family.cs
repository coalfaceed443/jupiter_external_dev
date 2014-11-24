using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;
using CRM.Code.Helpers;

namespace CRM.Code.Models
{
    public partial class CRM_Family : IAutocomplete, ICRMContext, IHistory, INotes
    {
        public object ShallowCopy()
        {
            return (CRM_Family)this.MemberwiseClone();
        }


        public static IEnumerable<CRM_Family> BaseSet(MainDataContext db)
        {
            return db.CRM_Families;
        }

        public List<string> Tokens
        {
            get
            {
                List<string> tokens = new List<string>();
                tokens.AddRange(JSONSet.ConvertToTokens(this.Name));
                return tokens;
            }
        }

        [IsListData("Members List")]
        public string MemberList
        {
            get
            {
                string memberList = "";
                for (int i = 0; i < this.CRM_FamilyPersons.Count(); i++)
                {
                    if (i < 3)
                        memberList += this.CRM_FamilyPersons[i].CRM_Person.Fullname + " [" + this.CRM_FamilyPersons[i].CRM_Person.ShortDateOfBirthOutput + "]";

                    if (i < 2 && (i + 1) != this.CRM_FamilyPersons.Count())
                        memberList += ", ";

                    if (i == 3)
                        memberList += "...";                    
                }

                return memberList;
            }
        }


        public string DisplayName
        {
            get
            {
                return OutputTableName + " : " + this.Name;
            }
        }

        public string OutputTableName
        {
            get
            {
                return "Family Record";
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
                return 0;
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
                return "/admin/families/details.aspx?id=" + this.ID;
            }
        }

    }
}