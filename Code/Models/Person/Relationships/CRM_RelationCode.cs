using CRM.Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace CRM.Code.Models
{
    public partial class CRM_RelationCode : ICRMContext, IHistory, ICRMRecord
    {
        public static IQueryable<CRM_RelationCode> BaseSet(MainDataContext db)
        {
            return from p in db.CRM_RelationCodes
                   orderby p.Name
                   select p;
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
                return "Relationship code";
            }
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
            return (CRM_Offer)this.MemberwiseClone();
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
                return "/admin/relationshipcode/details.aspx?id=" + this.ID;
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
    }
}