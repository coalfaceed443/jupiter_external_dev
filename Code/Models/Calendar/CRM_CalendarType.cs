using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Code.Models
{
    public partial class CRM_CalendarType
    {
        public enum TypeList : int
        {
            groupbooking = 0,
            schoolvisit,
            party,
            external,
            generic = 4,
            task,
            cpd,
            Event,
            outreach
        }
    }
}