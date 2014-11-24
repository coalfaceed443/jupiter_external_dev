using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Utils.Time;
using CRM.Code.Models;

namespace CRM.Scheduled
{
    /// <summary>
    /// Summary description for WrongSend
    /// </summary>
    public class WrongSend : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            MainDataContext db = new MainDataContext();


            var dueReminders = (from p in db.CRM_AnnualPasses
                                where !p.IsArchived
                                where !p.IsPending
                                where p.ExpiryDate.Date >= new DateTime(2014, 08, 01)
                                where p.ExpiryDate.Date < UKTime.Now
                                where p.PaymentMethod == (byte)CRM.Code.Helpers.PaymentType.Types.Cash ||
                                p.PaymentMethod == (byte)CRM.Code.Helpers.PaymentType.Types.CreditCard
                                select p);

            var incorrect = from p in dueReminders.ToArray()
                        where p.CRM_AnnualPassCard.CRM_AnnualPasses.Any(c => !c.IsExpired)
                        select p;

            HttpContext.Current.Response.Write(incorrect.Count() + "<br/>");
            foreach (CRM_AnnualPass pass in incorrect)
            {
                HttpContext.Current.Response.Write("<a href=\"" + pass.CRM_AnnualPassCard.DetailsURL + "\" target=\"_blank\">" + pass.CRM_AnnualPassCard.DetailsURL + "</a><br/>");
            }

            HttpContext.Current.Response.End();

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