using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;
using CRM.Code.Helpers;
using CRM.Code.Utils.List;

namespace CRM.Code.Models
{
    public partial class CRM_School : IHistory, ICRMRecord, ISchoolOrganisation, INotes, ICustomField, ICRMContext
    {
        public static IEnumerable<CRM_School> BaseSet(MainDataContext db)
        {
            return db.CRM_Schools;
        }

        public object ShallowCopy()
        {
            return (CRM_School)this.MemberwiseClone();
        }

        [DuplicateTest]
        public string DisplayName
        {
            get
            {
                return ArchivedOutput + OutputTableName + " : " + this.Name;
            }
        }

        public string OutputTableName
        {
            get
            {
                return "School Record";
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
                return "/admin/school/details.aspx?id=" + this.ID;
            }
        }

        [IsListData("Total Contacts")]
        public int Contacts
        {
            get
            {
                return this.CRM_PersonSchools.Count();
            }
        }

        [IsListData("Region")]
        public string Region
        {
            get
            {
                return this.CRM_RegionID == null ? "Not set" : this.CRM_Region.Name;
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

        public List<string> Tokens
        {
            get
            {
                List<string> tokens = new List<string>();
                tokens.AddRange(JSONSet.ConvertToTokens(this.Name));
                tokens.AddRange(this.CRM_Address.Tokens);
                return tokens;
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

        [IsListData("LEA")]
        public string LEA
        {
            get
            {
                return this.CRM_LEAID == null ? "None" : this.CRM_LEA.Name;                    
            }
        }

        [IsListData("Type")]
        public string Type
        {
            get
            {
                return this.CRM_SchoolType.Name;
            }
        }

        
        [IsListData("Key Stages")]
        public string OutputKeystages
        {
            get
            {
                return JSONSet.FlattenList(this.CRM_SchoolKeystages.Select(a => a.CRM_KeyStage.Name).ToList());
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