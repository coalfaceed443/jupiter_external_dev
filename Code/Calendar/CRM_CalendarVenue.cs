using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Code.Models
{
    public partial class CRM_CalendarVenue
    {
        public string OutputTimeRange
        {
            get
            {
                return this.DateTimeFrom.ToString("HH:mm") + " - " + this.DateTimeTo.ToString("HH:mm"); 
            }
        }

        public void DeleteFromDatabase(MainDataContext db, Models.Admin AdminUser)
        {
            CRM.Code.History.History.RecordLinqDelete(AdminUser, this);
            db.CRM_CalendarVenues.DeleteOnSubmit(this);
            db.SubmitChanges();
        }
    }
}