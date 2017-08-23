using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.IO;
using System.Text;

using CRM.Code.Utils.Email;
using CRM.Code.Models;
using CRM.Code.Managers;
using CRM.Code.Utils.Time;
using CRM.Code.Utils.Enumeration;

namespace CRM.Code.Managers
{
    /// <summary>
    /// Container for all emails sent from the website
    /// </summary>
    public class EmailManager
    {
        private MailAddressCollection mailTo = new MailAddressCollection();
        private MailAddressCollection mailCc = new MailAddressCollection();
        private MailAddressCollection mailBcc = new MailAddressCollection();
        private List<string> attachments = new List<string>();

        public EmailManager() { }

        public EmailManager(string emailTo)
        {
            mailTo.Add(emailTo);
        }

        public void ClearRecipients()
        {
            mailTo = new MailAddressCollection();
            mailCc = new MailAddressCollection();
            mailBcc = new MailAddressCollection();
        }

        public void AddTo(string email)
        {
            mailTo.Add(email);
        }

        public void AddCc(string email)
        {
            mailCc.Add(email);
        }

        public void AddBcc(string email)
        {
            mailBcc.Add(email);
        }

        public void AddAttachment(string filepath)
        {
            string fullpath = HttpContext.Current.Server.MapPath(filepath);

            if (File.Exists(fullpath))
            {
                attachments.Add(fullpath);
            }
        }

        public void SendNonTemplate(string content, string subject)
        {
            Email.SendNoneTemplateEmail(content, subject, mailTo, mailCc, mailBcc, attachments);
        }

        public void SendRSVP(string message, CRM_CalendarAdmin Invite, MainDataContext db, Models.Admin CurrentUser)
        {
            System.IO.StringWriter htmlStringWriter = new System.IO.StringWriter();
            HttpContext.Current.Server.Execute("/app_emails/invites/RSVP.aspx", htmlStringWriter);

            string htmlOutput = htmlStringWriter.GetStringBuilder().ToString();
            CRM.Code.Models.Admin admin = db.Admins.Single(c => c.ID == Invite.CRM_Calendar.CreatedByAdminID);
            htmlOutput = htmlOutput.Replace("@NAME@", admin.DisplayName);
            htmlOutput = htmlOutput.Replace("@RESPONDER@", Invite.Admin.DisplayName);         
            htmlOutput = htmlOutput.Replace("@EVENTNAME@", Invite.CRM_Calendar.DisplayName);
            htmlOutput = htmlOutput.Replace("@DATETIME@", Invite.EventDate);
            htmlOutput = htmlOutput.Replace("@STATUS@", Invite.StatusOutput);
            htmlOutput = htmlOutput.Replace("@SENDERMESSAGE@", message);
            
            AddTo(admin.Email);
            Email.SendTemplateEmail(htmlOutput.ToString(), "An user has RSVP'd - " + Invite.EventName + " - " + Invite.EventDate, mailTo, mailCc, mailBcc, attachments);

            CRM_Note note = new CRM_Note();
            note.Body = htmlOutput.ToString();
            note.Title = "RSVP from " + admin.DisplayName;
            note.DateCreated = UKTime.Now;
            note.TargetReference = Invite.CRM_Calendar.Reference;
            note.OwnerAdminID = CurrentUser.ID;
            db.CRM_Notes.InsertOnSubmit(note);
            db.SubmitChanges();

        }

        public void SendVenueChange(string message, CRM_CalendarAdmin Invite, MainDataContext db, Models.Admin CurrentUser)
        {
            System.IO.StringWriter htmlStringWriter = new System.IO.StringWriter();
            HttpContext.Current.Server.Execute("/app_emails/invites/VenueChange.aspx", htmlStringWriter);

            string htmlOutput = htmlStringWriter.GetStringBuilder().ToString();

            htmlOutput = htmlOutput.Replace("@NAME@", Invite.Admin.FirstName);
            htmlOutput = htmlOutput.Replace("@EVENTNAME@", Invite.CRM_Calendar.DisplayName);    
            htmlOutput = htmlOutput.Replace("@DATETIME@", Invite.EventDate);
            htmlOutput = htmlOutput.Replace("@AMENDED@", CurrentUser.DisplayName);
            htmlOutput = htmlOutput.Replace("@SENDERMESSAGE@", message);
            htmlOutput = htmlOutput.Replace("@VENUEDETAILS@", Invite.CRM_Calendar.VenueDetailsURL);

            AddTo(Invite.Admin.Email);
            Email.SendTemplateEmail(htmlOutput.ToString(), "An event's venues have been amended - " + Invite.EventName + " - " + Invite.EventDate, mailTo, mailCc, mailBcc, attachments);
        }

        public void SendPasswordReset(string name, string email, string code)
        {
            System.IO.StringWriter htmlStringWriter = new System.IO.StringWriter();
            HttpContext.Current.Server.Execute("/app_emails/PasswordReset.aspx", htmlStringWriter);

            string htmlOutput = htmlStringWriter.GetStringBuilder().ToString();

            htmlOutput = htmlOutput.Replace("@NAME@", name);
            htmlOutput = htmlOutput.Replace("@URL@", "https://www.jupiterartland.org/checkout?code=" + code + "&email=" + email);

            mailTo.Add(email);

            Email.SendTemplateEmail(htmlOutput.ToString(), "Your password reset", mailTo, mailCc, mailBcc, attachments);
        }

        public void SendNewPassword(string name, string email, string code)
        {
            System.IO.StringWriter htmlStringWriter = new System.IO.StringWriter();
            HttpContext.Current.Server.Execute("/app_emails/NewPassword.aspx", htmlStringWriter);

            string htmlOutput = htmlStringWriter.GetStringBuilder().ToString();

            htmlOutput = htmlOutput.Replace("@NAME@", name);
            htmlOutput = htmlOutput.Replace("@URL@", "https://www.jupiterartland.org/checkout?code=" + code + "&email=" + email);

            mailTo.Add(email);

            Email.SendTemplateEmail(htmlOutput.ToString(), "Set your password", mailTo, mailCc, mailBcc, attachments);
        }

        public void SendUserRemoved(string message, CRM_CalendarAdmin Invite, MainDataContext db, Models.Admin CurrentUser)
        {
            System.IO.StringWriter htmlStringWriter = new System.IO.StringWriter();
            HttpContext.Current.Server.Execute("/app_emails/invites/userremoved.aspx", htmlStringWriter);

            string htmlOutput = htmlStringWriter.GetStringBuilder().ToString();

            htmlOutput = htmlOutput.Replace("@NAME@", Invite.Admin.FirstName);
            htmlOutput = htmlOutput.Replace("@EVENTNAME@", Invite.CRM_Calendar.DisplayName);    
            htmlOutput = htmlOutput.Replace("@DATETIME@", Invite.EventDate);
            htmlOutput = htmlOutput.Replace("@REMOVED@", CurrentUser.DisplayName);
            htmlOutput = htmlOutput.Replace("@SENDERMESSAGE@", message);

            AddTo(Invite.Admin.Email);
            Email.SendTemplateEmail(htmlOutput.ToString(), "You have been removed from an event - " + Invite.EventName + " - " + Invite.EventDate, mailTo, mailCc, mailBcc, attachments);

            CRM_Note note = new CRM_Note();
            note.Body = htmlOutput.ToString();
            note.Title = "User removed";
            note.DateCreated = UKTime.Now;
            note.TargetReference = Invite.CRM_Calendar.Reference;
            note.OwnerAdminID = CurrentUser.ID;
            db.CRM_Notes.InsertOnSubmit(note);
            db.SubmitChanges();
        }

        public void SendTimeChange(CRM_Calendar PreviousDateDetails, CRM_CalendarAdmin Invite, MainDataContext db, Models.Admin CurrentUser)
        {
            System.IO.StringWriter htmlStringWriter = new System.IO.StringWriter();
            HttpContext.Current.Server.Execute("/app_emails/invites/datetimechange.aspx", htmlStringWriter);

            string htmlOutput = htmlStringWriter.GetStringBuilder().ToString();

            htmlOutput = htmlOutput.Replace("@NAME@", Invite.Admin.FirstName);
            htmlOutput = htmlOutput.Replace("@EVENTNAME@", Invite.CRM_Calendar.DisplayName);
            htmlOutput = htmlOutput.Replace("@OLDDATETIME@", PreviousDateDetails.OutputDate);
            htmlOutput = htmlOutput.Replace("@NEWDATETIME@", Invite.EventDate);
            htmlOutput = htmlOutput.Replace("@CHANGED@", CurrentUser.DisplayName);

            AddTo(Invite.Admin.Email);
            Email.SendTemplateEmail(htmlOutput.ToString(), "An event you are tagged in has been rescheduled - " + Invite.EventName + " originally " + PreviousDateDetails.OutputDate, mailTo, mailCc, mailBcc, attachments);

            CRM_Note note = new CRM_Note();
            note.Body = htmlOutput.ToString();
            note.Title = "Reschedule sent";
            note.DateCreated = UKTime.Now;
            note.TargetReference = Invite.CRM_Calendar.Reference;
            note.OwnerAdminID = CurrentUser.ID;
            db.CRM_Notes.InsertOnSubmit(note);
            db.SubmitChanges();

        }

        public void SendNewInvite(string message, CRM_CalendarAdmin Invite, MainDataContext db, Models.Admin CurrentUser)
        {
            System.IO.StringWriter htmlStringWriter = new System.IO.StringWriter();
            HttpContext.Current.Server.Execute("/app_emails/invites/newinvite.aspx", htmlStringWriter);

            string htmlOutput = htmlStringWriter.GetStringBuilder().ToString();

            htmlOutput = htmlOutput.Replace("@NAME@", Invite.Admin.FirstName);
            htmlOutput = htmlOutput.Replace("@EVENTNAME@", Invite.CRM_Calendar.DisplayName);
            htmlOutput = htmlOutput.Replace("@DATETIME@", Invite.EventDate);
            htmlOutput = htmlOutput.Replace("@INVITED@", CurrentUser.DisplayName);
            htmlOutput = htmlOutput.Replace("@SENDERMESSAGE@", message);
            htmlOutput = htmlOutput.Replace("@ACCEPT@", Invite.CRM_Calendar.RSVPAttend);
            htmlOutput = htmlOutput.Replace("@DECLINE@", Invite.CRM_Calendar.RSVPNotAttend);

            AddTo(Invite.Admin.Email);
            Email.SendTemplateEmail(htmlOutput.ToString(), "You have been tagged to a new event - " + Invite.EventName + " - " + Invite.EventDate, mailTo, mailCc, mailBcc, attachments);

            CRM_Note note = new CRM_Note();
            note.Body = htmlOutput.ToString();
            note.Title = "Invite sent";
            note.DateCreated = UKTime.Now;
            note.TargetReference = Invite.CRM_Calendar.Reference;
            note.OwnerAdminID = CurrentUser.ID;
            db.CRM_Notes.InsertOnSubmit(note);
            db.SubmitChanges();

        }

        public void SendResetLink(CRM.Code.Models.Admin admin)
        {
            System.IO.StringWriter htmlStringWriter = new System.IO.StringWriter();
            HttpContext.Current.Server.Execute("/app_emails/resetlink.aspx", htmlStringWriter);
            
            string htmlOutput = htmlStringWriter.GetStringBuilder().ToString();

            htmlOutput = htmlOutput.Replace("@DISPLAYNAME@", admin.DisplayName);
            htmlOutput = htmlOutput.Replace("@EXPIRY@", ((DateTime)admin.LastReset).AddMinutes(5).ToString("dd/MM/yyyy HH:mm"));
            htmlOutput = htmlOutput.Replace("@RESETLINK@", admin.ResetLink);

            Email.SendTemplateEmail(htmlOutput.ToString(), "Your Password Reset Link", mailTo, mailCc, mailBcc, attachments);
        }

        public void SendEnquiry(string name, string email, string telephone, string message, string methodOfContact, string newsletterOpt)
        {
            StreamReader f = new StreamReader(HttpContext.Current.Server.MapPath(Constants.EmailTemplatePath + "contactform.html"));
            StringBuilder content = new StringBuilder(f.ReadToEnd());
            f.Close();

            content.Replace("@NAME@", name);
            content.Replace("@EMAIL@", email);
            content.Replace("@TELEPHONE@", telephone);
            content.Replace("@METHOD_OF_CONTACT@", methodOfContact);
            content.Replace("@NEWSLETTER_OPT@", newsletterOpt);
            content.Replace("@MESSAGE@", message.Replace(Environment.NewLine, "<br />"));

            Email.SendTemplateEmail(content.ToString(), "Website Enquiry", mailTo, mailCc, mailBcc, attachments);
        }

        public void SendRenewalEmail(CRM_AnnualPass annualPass, MainDataContext db, string emailTo)
        {
            TemplateEmail template = db.TemplateEmails.Single(c => c.FixedRef == TemplateEmail.TemplateEmails.RenewalEmail);
            string content = template.Body;

            CRM_Person person = new AnnualPassManager().GetPrimaryContact(annualPass);

            content = annualPass.ReplacePlaceholders(content, person);

            if (String.IsNullOrEmpty(emailTo))
            {
                AddTo(person.PrimaryEmail);
            }
            else
            {
                AddTo(emailTo);
            }

            if (!template.IsDisabled)
            {
                SendTemplate(content, annualPass.ReplacePlaceholders(template.Subject, person), template);
            }
        }



        private void SendTemplate(string content, string subject, TemplateEmail templateEmail)
        {

            if (templateEmail.IsToEnabled)
            {
                AddTo(templateEmail.ToEmail);
            }

            if (!String.IsNullOrEmpty(templateEmail.CCEmail))
                AddCc(templateEmail.CCEmail);

            if (!String.IsNullOrEmpty(templateEmail.BCCEmail))
                AddBcc(templateEmail.BCCEmail);

            Email.SendTemplateEmail(content.ToString(), subject, mailTo, mailCc, mailBcc, attachments, templateEmail.FromEmail);
        }
    }
}