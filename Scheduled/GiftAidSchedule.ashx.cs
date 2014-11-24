using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;
using CRM.Code.Utils.Time;

namespace CRM.Scheduled
{
    /// <summary>
    /// Summary description for GiftAidSchedule
    /// </summary>
    public class GiftAidSchedule : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            using (MainDataContext db = new MainDataContext())
            {
                var profilesForToday = from p in CRM_FundraisingGiftProfile.PublicBaseSet(db).ToArray()
                                       where p.NextPaymentDate.Date == UKTime.Now.Date
                                       where p.IsDateActive
                                       select p;

                foreach (CRM_FundraisingGiftProfile profile in profilesForToday)
                {
                    CRM_FundraisingGiftProfileLog log = new CRM_FundraisingGiftProfileLog()
                    {
                        AmountCharged = profile.AmountToCharge,
                        IsConfirmed = false,
                        CRM_FundraisingGiftProfileID = profile.ID,
                        PaymentReference = profile.PaymentReference,
                        TimestampCreated = UKTime.Now.Date,
                        TimestampConfirmed = null
                    };

                    db.CRM_FundraisingGiftProfileLogs.InsertOnSubmit(log);
                    db.SubmitChanges();

                    if (profile.IsDateActive)
                    {
                        DateTime nextMonth = UKTime.Now.AddMonths(profile.EveryXMonth);
                        profile.NextPaymentDate = new DateTime(nextMonth.Year, nextMonth.Month, profile.DayOfMonth);
                        db.SubmitChanges();
                    }
                
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