using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Code.Calendar
{
    public class CalendarItem
    {
        public DateTime Hour { get; set; }
        public IEnumerable<CalendarSlot> Slots { get; set; }
    }
}