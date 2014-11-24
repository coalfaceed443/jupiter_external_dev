using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Utils.Enumeration;

namespace CRM.Code.Helpers
{
    public class PaymentType
    {
        public enum Types
        {
            [StringValue("Unknown")]
            Unknown,            
            [StringValue("Cash")]
            Cash,
            [StringValue("Credit Card")]
            CreditCard,
            [StringValue("Personal Cheque")]
            Cheque,
            [StringValue("PayPal")]
            PayPal,
            [StringValue("Voucher")]
            Voucher,
            [StringValue("StandingOrder")]
            StandingOrder,
            [StringValue("Other")]
            Other

        }
    }
}