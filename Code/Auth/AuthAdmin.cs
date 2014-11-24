using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;

namespace CRM.Code.Auth
{
    public class AuthAdmin : Auth
    {
        public AuthAdmin(MainDataContext dbIn)
        {
            db = dbIn;
            CookieID = "admin";
        }

        public bool Login(string username, string password)
        {
            bool result = false;

            string encryptedPassword = GetHashedString(password);

            CRM.Code.Models.Admin admin = db.Admins.SingleOrDefault(p => p.Password == encryptedPassword && p.Username == username);

            if (admin != null)
            {
                CreateSession(admin.ID, CookieAuthTypes.Admin);
                result = true;

                admin.LastLogin = DateTime.UtcNow;
                db.SubmitChanges();
            }

            return result;
        }

        public CRM.Code.Models.Admin Authorise()
        {
            return db.Admins.SingleOrDefault(p => p.ID == GetRecordID(CookieAuthTypes.Admin));
        }
    }
}