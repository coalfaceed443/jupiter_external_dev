using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;
using CRM.Code.Helpers;

namespace CRM.Code.Managers
{
    public class RotaManager
    {
        private MainDataContext db;
        private DateTime QueryDate;
        public List<RotaContainer> RotaContainers;
        public IEnumerable<CRM.Code.Models.Admin> Admins;
        public RotaManager(DateTime DateToQuery)
        {
            db = new MainDataContext();
            Admins = GrabAdminList().OrderByDescending(o => o.CRM_CalendarAdmins.Count());
            #region Prepare date objects
            QueryDate = DateToQuery.Date;
            DateTime EndQuery = DateToQuery.Date.AddDays(1);           
            DateTime pDate = QueryDate;
            #endregion 

            RotaContainers = new List<RotaContainer>();

            while (pDate <= EndQuery)
            {
                RotaContainer container = new RotaContainer(Admins, pDate);
                RotaContainers.Add(container);
                pDate = pDate.AddHours(1);
            }

        }

        private IEnumerable<CRM.Code.Models.Admin> GrabAdminList()
        {
            return db.Admins.ToArray().OrderBy(a => a.CRM_CalendarInvites.Count());
        }
    }
}