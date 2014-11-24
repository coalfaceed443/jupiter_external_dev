using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;
using CRM.Code.Interfaces;
using CRM.Code.BasePages.Admin;
using System.Web.UI;
using CRM.Code.Auth;
namespace CRM.Code.Managers
{
    public class NoteManager
    {
        private MainDataContext db;
        private CRM.Code.Models.Admin CurrentAdmin;
        private string TargetReference;

        public NoteManager()
        {
            Initialize();
        }

        public NoteManager(string baseReference) : this()
        {
            TargetReference = baseReference;
            Initialize();
        }

        public void Initialize()
        {
            db = new MainDataContext();

            if (HttpContext.Current.CurrentHandler is AdminPage)
            {
                AdminPage adminPage = (AdminPage)HttpContext.Current.CurrentHandler;
                CurrentAdmin = adminPage.AdminUser;
            }
            else
            {
                AuthAdmin AuthAdmin = new AuthAdmin(db);
                CurrentAdmin = AuthAdmin.Authorise();
            }
        }

        public IEnumerable<CRM_NoteAdminRead> ReadNotes()
        {
            return from p in db.CRM_NoteAdminReads
                   where p.AdminID == CurrentAdmin.ID
                   where p.CRM_Note.TargetReference == TargetReference
                   select p;
        }

        public int TotalNotes()
        {
            return (from p in db.CRM_Notes
                   where p.TargetReference == TargetReference
                   select p).Count();
        }

        public int UnreadCount()
        {
            return TotalNotes() - ReadNotes().Count();
        }

        public void MarkAsRead(int NoteID)
        {
            CRM_NoteAdminRead log = GetReadLog(NoteID);

            if (log == null)
            {
                log = new CRM_NoteAdminRead()
                {
                    AdminID = CurrentAdmin.ID,
                    CRM_NoteID = NoteID
                };
                db.CRM_NoteAdminReads.InsertOnSubmit(log);
                db.SubmitChanges();
            }
        }

        public bool IsRead(int NoteID)
        {
            return GetReadLog(NoteID) != null;
        }

        public CRM_NoteAdminRead GetReadLog(int NoteID)
        {
            return db.CRM_NoteAdminReads.SingleOrDefault(s => s.AdminID == CurrentAdmin.ID && s.CRM_NoteID == NoteID);
        }

        public void MarkAsUnread(int NoteID)
        {
            CRM_NoteAdminRead log = GetReadLog(NoteID);
            if (GetReadLog(NoteID) != null)
            {
                db.CRM_NoteAdminReads.DeleteOnSubmit(log);
                db.SubmitChanges();
            }
        }
    }
}