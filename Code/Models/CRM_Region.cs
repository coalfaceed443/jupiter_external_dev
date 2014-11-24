using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Abstracts;
using CRM.Code.Interfaces;
using System.Web.UI.WebControls;

namespace CRM.Code.Models
{
    public partial class CRM_Region : absArchivable, IArchivable, IHistory, ICRMContext
    {
        public static IEnumerable<CRM_Region> BaseSet(MainDataContext db)
        {
            return db.CRM_Regions.Where(e => e.IsActive && !e.IsArchived).OrderBy(e => e.Name);
        }

        public ListItem ListItem
        {
            get
            {
                return new ListItem(this.Name, this.ID.ToString());
            }
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
                return "Region";
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
                return "/admin/region/details.aspx?id=" + this.ID;
            }
        }


        [IsListData("Schools assigned")]
        public int SchoolsAssigned
        {
            get
            {
                return this.CRM_Schools.Count();
            }
        }

        [IsListData("Is Active")]
        public string IsActiveOutput
        {
            get
            {
                return this.IsActive ? "Yes" : "No";
            }
        }
    }
}