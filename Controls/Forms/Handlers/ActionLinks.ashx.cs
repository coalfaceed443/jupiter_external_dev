using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;
using CRM.Code.Managers;
using System.Web.SessionState;
using CRM.Code.BasePages.Admin;
using CRM.Code.Utils.Time;
using CRM.Code.Auth;

namespace CRM.Controls.Forms.Handlers
{
    /// <summary>
    /// Summary description for ActionLinks
    /// </summary>
    public class ActionLinks : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            byte route = Convert.ToByte(HttpContext.Current.Request.QueryString["route"]);
            string recordid = HttpContext.Current.Request.QueryString["recordid"];
            string returnurl = HttpContext.Current.Request.QueryString["returnURL"];
            string message = "Done";

            using (MainDataContext db = new MainDataContext())
            {
                AuthAdmin auth = new AuthAdmin(db);

                if (auth.Authorise() == null)
                {
                    context.Response.Write("Admin Auth Error");
                    context.Response.End();
                }
                db.Dispose();
            }
            DateTime timekey = DateTime.Parse(HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["timekey"]));

            if (UKTime.Now > timekey.AddMinutes(62))
            {
                NoticeManager.SetMessage("This action has expired for security reasons - did you use your browser back button?", HttpUtility.UrlDecode(returnurl));
            }
            else
            {
                bool disableNoticeManager = false;
                using (MainDataContext db = new MainDataContext())
                {
                    switch (route)
                    {
                        case (byte)ActionLink.Route.RemoveAdminFromCalendarItem:
                                {
                                    CRM_CalendarAdmin calendarAdmin = db.CRM_CalendarAdmins.Single(c => c.ID.ToString() == recordid);

                                    message = calendarAdmin.AdminName + " removed from " + calendarAdmin.CRM_Calendar.DisplayName;
                                    db.CRM_CalendarAdmins.DeleteOnSubmit(calendarAdmin);
                                    db.SubmitChanges();


                                }
                            break;

                        case (byte)ActionLink.Route.RemoveFamilyPerson:
                            {
                                CRM_FamilyPerson familyPerson = db.CRM_FamilyPersons.Single(f => f.ID.ToString() == recordid);
                                message = familyPerson.CRM_Person.Fullname + " removed from the " + familyPerson.CRM_Family.Name + " family";
                                db.CRM_FamilyPersons.DeleteOnSubmit(familyPerson);
                                db.SubmitChanges();
                            }
                            break;

                        case (byte)ActionLink.Route.ArchiveTaskParticipant:
                            {
                                CRM_TaskParticipant participant = db.CRM_TaskParticipants.Single(t => t.ID.ToString() == recordid);
                                participant.IsArchived = true;
                                db.SubmitChanges();
                                message = participant.Name + " archived.";
                            }
                            break;

                        case (byte)ActionLink.Route.ReinstateTaskParticipant:
                            {
                                CRM_TaskParticipant participant = db.CRM_TaskParticipants.Single(t => t.ID.ToString() == recordid);
                                participant.IsArchived = false;
                                db.SubmitChanges();
                                message = participant.Name + " reinstated.";
                            }
                            break;

                        case (byte)ActionLink.Route.ArchivePassPerson:
                            {
                                CRM_AnnualPassPerson person = db.CRM_AnnualPassPersons.Single(t => t.ID.ToString() == recordid);
                                person.IsArchived = true;
                                db.SubmitChanges();
                                message = person.DisplayName + " archived.";
                            }
                            break;

                        case (byte)ActionLink.Route.ReinstatePassPerson:
                            {
                                CRM_AnnualPassPerson person = db.CRM_AnnualPassPersons.Single(t => t.ID.ToString() == recordid);
                                person.IsArchived = false;
                                db.SubmitChanges();
                                message = person.DisplayName + " reinstanted.";
                            }
                            break;


                        case (byte)ActionLink.Route.ToggleReadStatus:
                            {
                                NoteManager manager = new NoteManager();
                                bool IsRead = manager.IsRead(Convert.ToInt32(recordid));
                                disableNoticeManager = true;
                                if (IsRead)
                                {
                                    MarkAsUnread(recordid);
                                    message = "Marked as unread.";
                                }
                                else
                                {
                                    MarkAsRead(recordid);
                                    message = "Marked as read.";
                                }
                            
                            }    
                            break;
                        case (byte)ActionLink.Route.MarkNoteAsRead:
                            {
                                MarkAsRead(recordid);
                                db.SubmitChanges();
                                message = "Marked as read.";
                            }
                            break;

                        case (byte)ActionLink.Route.MarkNoteAsUnread:
                            {
                                MarkAsUnread(recordid);
                                db.SubmitChanges();
                                message = "Marked as unread.";
                            }
                            break;
                        case (byte)ActionLink.Route.DeleteOrganisationSchool:
                            {
                                CRM_OrganisationSchool orgSchool = db.CRM_OrganisationSchools.FirstOrDefault(s => s.ID.ToString() == recordid);
                                if (orgSchool != null)
                                {
                                    db.CRM_OrganisationSchools.DeleteOnSubmit(orgSchool);
                                    db.SubmitChanges();
                                    message = "Link removed";
                                }
                            }
                            break;
                        case (byte)ActionLink.Route.ToggleInviteIsAttended:
                            {
                                CRM_CalendarInvite invite = db.CRM_CalendarInvites.FirstOrDefault(s => s.ID.ToString() == recordid);
                                if (invite != null)
                                {
                                    invite.IsAttended = !invite.IsAttended;
                                    db.SubmitChanges();
                                    message = "Invite Attendance Toggled";
                                }
                            }
                            break;
                        case (byte)ActionLink.Route.ToggleInviteIsBooked:
                            {
                                CRM_CalendarInvite invite = db.CRM_CalendarInvites.FirstOrDefault(s => s.ID.ToString() == recordid);
                                if (invite != null)
                                {
                                    invite.IsBooked = !invite.IsBooked;
                                    db.SubmitChanges();
                                    message = "Invite Booked Toggled";
                                }
                            }
                            break;
                        case (byte)ActionLink.Route.ToggleInviteIsCancelled:
                            {
                                CRM_CalendarInvite invite = db.CRM_CalendarInvites.FirstOrDefault(s => s.ID.ToString() == recordid);
                                if (invite != null)
                                {
                                    invite.IsCancelled = !invite.IsCancelled;
                                    db.SubmitChanges();
                                    message = "Invite Cancellation Toggled";
                                }
                            }
                            break;
                        case (byte)ActionLink.Route.ToggleInviteIsInvited:
                            {
                                CRM_CalendarInvite invite = db.CRM_CalendarInvites.FirstOrDefault(s => s.ID.ToString() == recordid);
                                if (invite != null)
                                {
                                    invite.IsInvited = !invite.IsInvited;
                                    db.SubmitChanges();
                                    message = "Invite Toggled";
                                }
                            }
                            break;
                        case (byte)ActionLink.Route.DeleteInvite:
                            {
                                CRM_CalendarInvite invite = db.CRM_CalendarInvites.FirstOrDefault(s => s.ID.ToString() == recordid);
                                if (invite != null)
                                {
                                    db.CRM_CalendarInvites.DeleteOnSubmit(invite);
                                    db.SubmitChanges();
                                    message = "Invite Removed";
                                }
                            }
                            break;

                        case (byte)ActionLink.Route.ToggleGiftAidRecord:
                            {
                                CRM_FundraisingGiftProfileLog log = db.CRM_FundraisingGiftProfileLogs.FirstOrDefault(f => f.ID.ToString() == recordid);
                                if (log != null)
                                {
                                    if (!log.IsConfirmed)
                                    {
                                        log.TimestampConfirmed = UKTime.Now;
                                        log.IsConfirmed = true;
                                    }
                                    else
                                    {
                                        log.TimestampConfirmed = null;
                                        log.IsConfirmed = false;
                                    }

                                    db.SubmitChanges();
                                    message = "Gift aid record toggled";
                                }
                            }
                            break;

                        case (byte)ActionLink.Route.DeleteGiftAidRecord:
                            {
                                CRM_FundraisingGiftProfileLog log = db.CRM_FundraisingGiftProfileLogs.FirstOrDefault(f => f.ID.ToString() == recordid);
                                if (log != null)
                                {
                                    db.CRM_FundraisingGiftProfileLogs.DeleteOnSubmit(log);
                                    db.SubmitChanges();
                                    message = "Gift aid record deleted";
                                }
                            }
                            break;
                    }

                    db.Dispose();

                    if (!disableNoticeManager)
                        NoticeManager.SetMessage(message, HttpUtility.UrlDecode(returnurl));
                    else
                        HttpContext.Current.Response.Redirect(returnurl);
                }
            

            }

        }
        public void MarkAsRead(string recordid)
        {
            new NoteManager().MarkAsRead(Convert.ToInt32(recordid));
        }

        public void MarkAsUnread(string recordid)
        {
            new NoteManager().MarkAsUnread(Convert.ToInt32(recordid));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class ActionLink
    {
        private const string ControlURL = "/controls/forms/handlers/actionlinks.ashx";

        public static string FormURL(Route route, int RecordID, string returnURL)
        {
            return "/controls/forms/handlers/actionlinks.ashx?route=" + (byte)route + "&recordid=" + RecordID + "&returnurl=" + HttpUtility.UrlEncode(returnURL) + "&timekey=" + HttpUtility.UrlEncode(UKTime.Now.ToString());
        }

        public enum Route
        {
            RemoveFamilyPerson,
            ArchiveTaskParticipant,
            ReinstateTaskParticipant,
            ArchivePassPerson,
            ReinstatePassPerson,
            ToggleReadStatus,
            MarkNoteAsRead,
            MarkNoteAsUnread,
            RemoveAdminFromCalendarItem,
            DeleteOrganisationSchool,
            ToggleInviteIsInvited,
            ToggleInviteIsAttended,
            ToggleInviteIsCancelled,
            ToggleInviteIsBooked,
            DeleteInvite,
            ToggleGiftAidRecord,
            DeleteGiftAidRecord
        }
    }
}