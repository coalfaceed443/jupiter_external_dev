using CRM.Code.Utils.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Code.Models
{
    public partial class HoldingPen
    {
        public static IEnumerable<HoldingPen> BaseSet(MainDataContext db)
        {
            return from p in db.HoldingPens
                   where p.DateCommitted == null
                   orderby p.DateReceived
                   select p;
        }

        public Int64 DateReceived_T
        {
            get
            {
                return DateReceived.Ticks;
            }
        }



        /// <summary>
        /// Don't move the order of these, the service relies on the integer value of the enum which webservices does not support.
        /// Moving this enum order will destroy the data integrity of the CRM merge tool.
        /// See MSDN https://social.msdn.microsoft.com/forums/vstudio/en-US/7f697ff4-ba14-4db3-a236-985d12e9a50e/web-service-enum-limitation
        /// </summary>
        public enum Origins
        {
            [StringValue("Website Checkout")]
            websitecheckout = 0,
            [StringValue("Website Membership")]
            websitemembership = 1
        }


        [IsListData("Date Received")]
        public string DateReceivedOutput
        {
            get
            {
                return this.DateReceived != null ? ((DateTime)this.DateReceived).ToString("dd/MM/yyyy HH:mm") : "";
            }
        }

        [IsListData("View")]
        public string DetailsURLOutput
        {
            get
            {
                return Utils.Text.Text.ConvertUrlsToLinks("/admin/merge/details.aspx?id=" + this.ID, "View");
            }
        }

        public string DetailsURL
        {
            get
            {
                return "/admin/merge/details.aspx?id=" + this.ID;
            }
        }
        
        [IsListData("Mail Preference")]
        public string MailPreference
        {
            get
            {
                return this.DoNotMail ? "Do not mail" : "OK to mail";
            }
        }

        [IsListData("Email Preference")]
        public string EMailPreference
        {
            get
            {
                return this.DoNotEmail ? "Do not email" : "OK to email";
            }
        }

        [IsListData("Pass Info Preference")]
        public string PassInfoPreference
        {
            get
            {
                return this.AlwaysSendPassInfo ? "Always send me Pass Info regardless of mail preferences" : "My pass info follows mail preferences";
            }
        }

    }
}