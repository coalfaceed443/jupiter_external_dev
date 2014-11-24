using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Net.Mail;
using System.Web.Configuration;
using System.Configuration;

namespace CRM.Code.Utils.Email
{
    public partial class Email
    {
        public static bool SendTemplateEmail(string email, string subject, MailAddressCollection mailTo, MailAddressCollection mailCc, MailAddressCollection mailBcc, List<string> attachments)
        {
            return SendTemplateEmail(email, subject, mailTo, mailCc, mailBcc, attachments, Constants.EmailsFrom);
        }

        public static bool SendTemplateEmail(string email, string subject, MailAddressCollection mailTo, MailAddressCollection mailCc, MailAddressCollection mailBcc, List<string> attachments, string emailFrom)
        {
            StreamReader f = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(Constants.EmailTemplatePath + "template.html"));
            StringBuilder template = new StringBuilder(f.ReadToEnd());
            f.Close();

            StringBuilder body = new StringBuilder(email);

            MailMessage mm = new MailMessage();

            if (Constants.IsRedirectAllMailToDev)
            {
                mm.To.Add(Constants.DeveloperEmail);
            }
            else
            {
                foreach (var item in mailTo)
                {
                    mm.To.Add(item);
                }

                foreach (var item in mailCc)
                {
                    mm.CC.Add(item);
                }

                foreach (var item in mailBcc)
                {
                    mm.Bcc.Add(item);
                }

                if (Constants.SendEmailsToDev)
                {
                    mm.Bcc.Add(Constants.DeveloperEmail);
                }
            }
            foreach (string att in attachments)
            {
                mm.Attachments.Add(new Attachment(att));
            }

            if (Email.IsValidEmail(emailFrom))
            {
                mm.From = new MailAddress(emailFrom);
            }
            else
            {
                mm.From = new MailAddress(Constants.EmailsFrom);
            }

            mm.Subject = subject;
            mm.IsBodyHtml = true;

            template.Replace("@SUBJECT@", subject);
            template.Replace("@BODY@", body.ToString());
            template.Replace("@DOMAIN@", Constants.DomainName);

            mm.Body = template.ToString();

            SmtpClient smtp = new SmtpClient();

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    smtp.Send(mm);
                    LogEmail(body.ToString(), subject, mailTo, mailCc, mailBcc);

                    foreach (Attachment a in mm.Attachments)
                        a.Dispose();

                    return true;
                }
                catch (Exception ex)
                {
                    Utils.Error.ErrorLog.AddEntry(ex);
                }
            }
            return false;
        }

        public static bool SendNoneTemplateEmail(string email, string subject, MailAddressCollection mailTo, MailAddressCollection mailCc, MailAddressCollection mailBcc, List<string> attachments)
        {
            MailMessage mm = new MailMessage();

            if (Constants.IsRedirectAllMailToDev)
            {
                mm.To.Add(Constants.DeveloperEmail);
            }
            else
            {
                foreach (var item in mailTo)
                {
                    mm.To.Add(item);
                }

                foreach (var item in mailCc)
                {
                    mm.CC.Add(item);
                }

                foreach (var item in mailBcc)
                {
                    mm.Bcc.Add(item);
                }

                if (Constants.SendEmailsToDev)
                {
                    mm.Bcc.Add(Constants.DeveloperEmail);
                }
            }

            foreach (string att in attachments)
            {
                mm.Attachments.Add(new Attachment(att));
            }

            mm.From = new MailAddress(Constants.EmailsFrom);
            mm.Subject = subject;
            mm.IsBodyHtml = true;

            mm.Body = email;

            SmtpClient smtp = new SmtpClient();

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    smtp.Send(mm);
                    LogEmail(email, subject, mailTo, mailCc, mailBcc);

                    foreach (Attachment a in mm.Attachments)
                        a.Dispose();

                    return true;
                }
                catch (Exception ex)
                {
                    Utils.Error.ErrorLog.AddEntry(ex);
                }
            }
            return false;
        }

        private static void LogEmail(string body, string subject, MailAddressCollection mailTo, MailAddressCollection mailCc, MailAddressCollection mailBcc)
        {
            string timeStamp = DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss-ffff");

            // create html file //

            try
            {
                string filePath = HttpContext.Current.Server.MapPath(Constants.ErrorLogPath + timeStamp + ".html");

                FileStream fs = File.Open(filePath, FileMode.CreateNew, FileAccess.Write);

                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);

                sw.WriteLine("<p><strong>Subject:</strong> " + subject + "</p>");
                sw.WriteLine("<p><strong>To:</strong> " + mailTo.ToString() + "</p>");
                sw.WriteLine("<p><strong>Cc:</strong> " + mailCc.ToString() + "</p>");
                sw.WriteLine("<p><strong>Bcc:</strong> " + mailBcc.ToString() + "</p>");
                sw.WriteLine("<p><strong>Timestamp:</strong> " + timeStamp + "</p>");

                sw.Write(body);

                sw.Close();
                fs.Close();

                // amend email log //

                string logFilePath = HttpContext.Current.Server.MapPath(Constants.EmailLogPath + "email-log.txt");

                fs = File.Open(logFilePath, FileMode.Append, FileAccess.Write);

                sw = new StreamWriter(fs, System.Text.Encoding.UTF8);

                sw.WriteLine(timeStamp + " - " + subject + " - " + mailTo.ToString());

                sw.Close();
                fs.Close();
            }
            catch(System.IO.IOException ex)
            {
                // throw away ex.
            }
        }

        /// <summary>
        /// Returns the supplied email surrounded in javascript to prevent spamming
        /// </summary>
        /// <param name="email">The email address to hide in the javascript.</param>
        /// <param name="displayName">The text that should display as the link.</param>
        /// <returns></returns>
        public static string JavascriptEmail(string email, string displayName)
        {
            if (!email.Contains("@"))
                return email;
            string jsFormat = "<script language=\"javascript\">" +
                                "user = '{0}'; site = '{1}';" +
                                "document.write('<a href=\"mailto:' + user + '@' + site + '\">');" +
                                "document.write('" + displayName + "</a>');" +
                              "</script>" +
                              "<noscript>&nbsp;</noscript>";
            return String.Format(jsFormat, email.Split('@')[0], email.Split('@')[1]);
        }

        /// <summary>
        /// Returns the supplied email surrounded in javascript to prevent spamming
        /// </summary>
        /// <param name="email">The email address to hide in the javascript.</param>
        /// <returns></returns>
        public static string JavascriptEmail(string email)
        {
            return JavascriptEmail(email, email);
        }

        /// <summary>
        /// Checks the given string to see if it is a valid email
        /// </summary>
        /// <param name="emailToCheck">The email to check</param>
        /// <returns>Whether the 'emailToCheck' is a valid email address</returns>
        public static bool IsValidEmail(string emailToCheck)
        {
            try
            {
                MailAddress ma = new MailAddress(emailToCheck);
                return true;
            }
            catch { return false; }
        }
    }
}