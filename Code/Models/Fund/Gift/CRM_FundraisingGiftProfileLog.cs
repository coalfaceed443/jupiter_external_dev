using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Controls.Forms.Handlers;

namespace CRM.Code.Models
{
    public partial class CRM_FundraisingGiftProfileLog
    {
        [IsListData("Date Created")]
        public string DateCreatedOutput
        {
            get
            {
                return this.TimestampCreated.ToString(Constants.DefaultDateStringFormat);
            }
        }

        public Int64 DateCreatedOutput_T
        {
            get
            {
                return this.TimestampCreated.Ticks;
            }
        }

        [IsListData("Date Confirmed")]
        public string DateConfirmedOutput
        {
            get
            {
                return this.TimestampConfirmed == null ? "Unconfirmed" : this.TimestampCreated.ToString(Constants.DefaultDateStringFormat);           
            }
        }

        public Int64 DateConfirmedOutput_T
        {
            get
            {
                return this.TimestampConfirmed == null ? 0 : ((DateTime)this.TimestampConfirmed).Ticks;  
            }
        }


        [IsListData("Amount Charged - Query")]
        public decimal AmountChargedQuery
        {
            get
            {
                return this.AmountCharged;
            }
        }
      
        [IsListData("Amount Charged")]
        public string AmountChargedOutput
        {
            get
            {
                return "£" + this.AmountCharged.ToString("N2");
            }
        }

        public string ConfirmedStatus
        {
            get
            {
                return this.IsConfirmed ? "Confirmed" : "TBC";
            }
        }


        [IsListData("Toggle Confirmed Status")]
        public string ConfirmedStatusToggleLink
        {
            get
            {
                return Utils.Text.Text.ConvertUrlsToLinks(ConfirmedStatusActionLink, ConfirmedStatus);
            }
        }


        public string ConfirmedStatusActionLink
        {
            get
            {
                return ActionLink.FormURL(ActionLink.Route.ToggleGiftAidRecord, this.ID, HttpContext.Current.Request.Url.AbsoluteUri);
            }
        }

        [IsListData("Delete Log")]
        public string DeleteLogLink
        {
            get
            {
                return Utils.Text.Text.ConvertUrlsToLinks(DeleteActionLink, "Delete");
            }
        }


        public string DeleteActionLink
        {
            get
            {
                return ActionLink.FormURL(ActionLink.Route.DeleteGiftAidRecord, this.ID, "/admin/fundraising/gifts/list.aspx");
            }
        }

        [IsListData("Next Payment Due")]
        public string PaymentDueDate
        {
            get
            {
                return this.CRM_FundraisingGiftProfile.NextPaymentDateOutput;
            }
        }

        [IsListData("Person name")]
        public string PersonName
        {
            get
            {
                return this.CRM_FundraisingGiftProfile.CRM_Person.Fullname;
            }
        }

    }
}