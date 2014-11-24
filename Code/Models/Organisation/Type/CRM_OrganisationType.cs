using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;
using CRM.Code.Abstracts;
using System.Web.UI.WebControls;

namespace CRM.Code.Models
{
    public partial class CRM_OrganisationType : absArchivable, IHistory, ICRMRecord, IArchivable, ICRMContext
    {
        public static IEnumerable<CRM_OrganisationType> BaseSet(MainDataContext db)
        {
            return db.CRM_OrganisationTypes.Where(o => o.IsActive && !o.IsArchived).OrderBy(o => o.Name);
        }

        public string DisplayName
        {
            get
            {
                return this.Name;
            }
        }

        public object ShallowCopy()
        {
            return (CRM_OrganisationType)this.MemberwiseClone();
        }

        public string OutputTableName
        {
            get
            {
                return "Organisation Type";
            }
        }

        public ListItem ListItem
        {
            get
            {
                return new ListItem(this.Name, this.ID.ToString());
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
                return "/admin/organisation/types/details.aspx?id=" + this.ID;
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
    }
}