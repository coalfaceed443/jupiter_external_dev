using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;

namespace CRM.Code.Helpers
{
    public class RotaContainer
    {
        public DateTime Time { get; set; }
        public List<AdminList> AdminLists { get; set; }
        public RotaContainer(IEnumerable<CRM.Code.Models.Admin> AdminList, DateTime _time)
        {
            Time = _time;
            AdminLists = new List<AdminList>();

            foreach (CRM.Code.Models.Admin admin in AdminList)
            {
                AdminLists.Add(new AdminList(Time, admin));
            }
        }



        public class AdminList
        {
            public IEnumerable<CRM_CalendarAdmin> CRM_CalendarAdmins { get; set; }
            public AdminList(DateTime Time, CRM.Code.Models.Admin Admin)
            {
                CRM_CalendarAdmins = Admin.CRM_CalendarAdmins.Where(c => c.CRM_Calendar.StartDateTime >= Time && c.CRM_Calendar.StartDateTime < Time.AddHours(1));
            }
        }


    }
}