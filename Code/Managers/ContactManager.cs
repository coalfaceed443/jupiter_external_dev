using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;
using CRM.Code.Interfaces;

namespace CRM.Code.Managers
{
    public class ContactManager : IDisposable
    {    
        private MainDataContext db;

        public ContactManager()
        {
            db = new MainDataContext();
        }

        public IEnumerable<IContact> Contacts
        {
            get
            {
                return (from p in CRM.Code.Utils.SharedObject.SharedObject.GetSharedObjects<IContact>(db)
                        select p);

            }
        }

        public IContact GetIContactByReference(string reference)
        {
            return Contacts.FirstOrDefault(f => f.Reference == reference);
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}