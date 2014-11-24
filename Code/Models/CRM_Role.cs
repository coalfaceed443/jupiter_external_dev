using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;

namespace CRM.Code.Models
{
    public partial class CRM_Role : IHistory, ICRMRecord, ICRMContext
    {
        public static IEnumerable<CRM_Role> BaseSet(MainDataContext db)
        {
            return db.CRM_Roles.OrderBy(c => c.Name);
        }

        public object ShallowCopy()
        {
            return (CRM_Note)this.MemberwiseClone();
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
                return "Role";
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
                return "/admin/role/details.aspx?id=" + this.ID;
            }
        }

        [IsListData("Contacts assigned")]
        public int ContactsAssigned
        {
            get
            {
                return this.CRM_PersonSchools.Count() + this.CRM_PersonOrganisations.Count();
            }
        }

    }
}