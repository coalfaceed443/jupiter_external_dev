using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;

namespace CRM.Code.Managers
{
    public class AnnualPassManager
    {
        private MainDataContext db;
        public AnnualPassManager()
        {
            db = new MainDataContext();
        }

        public CRM_Person GetPrimaryContact(CRM_AnnualPass CRM_AnnualPass)
        {
            return db.CRM_Persons.SingleOrDefault(c => c.Reference == CRM_AnnualPass.PrimaryContactReference);
        }

        public IEnumerable<CRM_AnnualPassType> GetAnnualPassTypes()
        {
            return from p in db.CRM_AnnualPassTypes
                   where !p.IsArchived
                   select p;
        }

        public int NewestMemberID()
        {
            int latest = 100000;

            CRM_AnnualPassCard newestPass = db.CRM_AnnualPassCards.OrderByDescending(a => a.MembershipNumber).FirstOrDefault();

            if (newestPass != null)
                latest = newestPass.MembershipNumber;

            return latest;
        }

        public int GetNewMemberID()
        {
            return NewestMemberID() + 1;
        }
    }
}