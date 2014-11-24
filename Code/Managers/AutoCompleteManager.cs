using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;
using CRM.Code.Interfaces;

namespace CRM.Code.Managers
{
    public class AutoCompleteManager : IDisposable
    {
        private MainDataContext db;
        public AutoCompleteManager()
        {
            db = new MainDataContext();
        }

        public IEnumerable<IAutocomplete> Records
        {
            get
            {
                return CRM.Code.Utils.SharedObject.SharedObject.GetSharedObjects<IAutocomplete>(db);
            }
        }

        public IAutocomplete GetIAutocompleteByReference(string reference)
        {
            return Records.FirstOrDefault(f => f.Reference == reference);
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}