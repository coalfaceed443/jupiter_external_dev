using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Abstracts;
using CRM.Code.Interfaces;
using System.Web.UI.WebControls;

namespace CRM.Code.Models
{
    public partial class CRM_Title : absArchivable, IArchivable, Utils.Ordering.ListOrderItem, ICRMContext
    {
        public ListItem ListItem
        {
            get
            {
                return new ListItem(this.Name, this.Name);
            }
        }

        public string DisplayName
        {
            get
            {
                return this.Name;
            }
        }

        [IsListData("Is Active")]
        public string ActiveOutput
        {
            get
            {
                return this.IsActive ? "Yes" : "No";
            }
        }

        public static IEnumerable<CRM_Title> BaseSet(MainDataContext db)
        {
            return db.CRM_Titles.Where(c => !c.IsArchived).OrderBy(o => o.OrderNo);
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
                return "/admin/person/titles/details.aspx?id=" + this.ID;
            }
        }

    }
}