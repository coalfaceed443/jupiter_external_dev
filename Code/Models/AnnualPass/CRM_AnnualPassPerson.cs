using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Controls.Forms.Handlers;
using CRM.Code.Interfaces;
using CRM.Code.Utils.Time;

namespace CRM.Code.Models
{
    public partial class CRM_AnnualPassPerson : ICRMContext
    {
        [IsListData("Display Name")]
        public string DisplayName
        {
            get
            {
                return this.CRM_Person.DisplayName;
            }
        }

        [IsListData("Name")]
        public string Name
        {
            get
            {
                return this.CRM_Person.Fullname;
            }
        }

        [IsListData("Photo")]
        public string Photo
        {
            get
            {
                return this.CRM_Person.PhotoPreviewOutput;
            }
        }


        [IsListData("Card Number")]
        public string PassNo
        {
            get
            {
                return this.CRM_AnnualPass.CRM_AnnualPassCard.MembershipNumber.ToString();
            }
        }


        [IsListData("People on card")]
        public string PeopleOnCard
        {
            get
            {
                return this.CRM_AnnualPass.PeopleOnCard;
            }
        }

        [IsListData("Payment Method")]
        public string PaymentMethodOutput
        {
            get
            {
                return this.CRM_AnnualPass.PaymentMethodOutput;
            }
        }


        [IsListData("Amount")]
        public decimal AmountPaid
        {
            get
            {
                return this.CRM_AnnualPass.AmountPaid;
            }
        }




        [IsListData("View Pass")]
        public string PassDetailsURLOutput
        {
            get
            {
                return this.CRM_AnnualPass.DetailsURLOutput;
            }
        }


        [IsListData("Expired")]
        public string Expired
        {
            get
            {
                return this.CRM_AnnualPass.ExpiryDate < UKTime.Now ? "Expired" : "In date";
            }
        }

        [IsListData("Expiry Date")]
        public string Expiry
        {
            get
            {
                return this.CRM_AnnualPass.ExpiryDate.ToString(Constants.DefaultDateStringFormat);
            }
        }

        public long Expiry_T
        {
            get
            {
                return this.CRM_AnnualPass.ExpiryDate.Ticks;
            }
        }

        [IsListData("Start Date")]
        public string StartDate
        {
            get
            {
                return this.CRM_AnnualPass.StartDate.ToString(Constants.DefaultDateStringFormat);
            }
        }


        [IsListData("Pass Type")]
        public string CardType
        {
            get
            {
                return this.CRM_AnnualPass.CRM_AnnualPassType.Name;
            }
        }


        [IsListData("Toggle Status")]
        public string ToggleStatusURL
        {
            get
            {
                string text = "";
                if (this.IsArchived)
                    text = "Archived";
                else
                    text = "Active";

                return Utils.Text.Text.ConvertUrlsToLinks(this.ActionRemoveFromPassURL, text);
            }
        }

        private string ActionRemoveFromPassURL
        {
            get
            {
                if (this.IsArchived)
                {
                    return ActionLink.FormURL(ActionLink.Route.ReinstatePassPerson, this.ID, this.PassDetailsURL);
                }
                else
                {
                    return ActionLink.FormURL(ActionLink.Route.ArchivePassPerson, this.ID, this.PassDetailsURL);
                }
            }
        }

        public CRM_Person Parent_CRM_Person
        {
            get
            {
                return this.CRM_Person;
            }
        }

        public string PassDetailsURL
        {
            get
            {
                return this.CRM_AnnualPass.DetailsURL;
            }
        }



    }
}