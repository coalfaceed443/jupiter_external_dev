using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;
using CRM.Code.Abstracts;
using System.Web.UI.WebControls;
using CRM.Code.Utils.List;

namespace CRM.Code.Models
{
    public partial class CRM_Offer : absArchivable, IArchivable, IHistory, ICRMRecord, ICRMContext
    {
        public static IEnumerable<CRM_Offer> BaseSet(MainDataContext db)
        {
            return db.CRM_Offers.Where(o => !o.IsArchived).OrderBy(c => c.Name);
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
                return "Offer";
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
                return "/admin/offer/details.aspx?id=" + this.ID;
            }
        }

        [IsListData("Events assigned")]
        public int ContactsAssigned
        {
            get
            {
                return this.GroupBookingsAssigned;
            }
        }

        [IsListData("Group bookings assigned")]
        public int GroupBookingsAssigned
        {
            get
            {
                return this.CRM_CalendarGroupBookings.Count();
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