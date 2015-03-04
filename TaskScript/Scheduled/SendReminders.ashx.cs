using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;
using CRM.Code.Utils.Time;
using CRM.Code.Managers;
using CRM.Code;

namespace CRM.Scheduled
{
    /// <summary>
    /// Summary description for SendReminders
    /// </summary>
    public class SendReminders : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            MainDataContext db = new MainDataContext();

            var dueReminders = from p in db.CRM_AnnualPasses
                               where !p.IsArchived
                               where !p.IsPending
                               where p.ExpiryDate.Date == UKTime.Now.Date
                               where p.PaymentMethod == (byte)CRM.Code.Helpers.PaymentType.Types.Cash ||
                               p.PaymentMethod == (byte)CRM.Code.Helpers.PaymentType.Types.CreditCard
                               select p;


            HttpContext.Current.Response.Write("Sending " + dueReminders.Count());

            foreach (CRM_AnnualPass pass in dueReminders)
            {
                try
                {
                    EmailManager manager = new EmailManager();
                    AnnualPassManager passManager = new AnnualPassManager();
                    CRM_Person person = passManager.GetPrimaryContact(pass);

                    if (person != null && !String.IsNullOrEmpty(person.PrimaryEmail))
                    {
                        manager.SendRenewalEmail(pass, db, person.PrimaryEmail);
                        HttpContext.Current.Response.Write("Sent");
                    }

                }
                catch {

                    EmailManager manager = new EmailManager();
                    manager.AddTo(Constants.DeveloperEmail);
                    manager.SendNonTemplate("Failed pass reminder on jupiter CRM - ID " + pass.ID.ToString(), "Failed Jupiter CRM reminder");

                }
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}