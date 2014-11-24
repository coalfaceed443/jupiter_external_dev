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