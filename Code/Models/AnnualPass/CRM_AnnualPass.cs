using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Utils.Time;
using CRM.Code.Interfaces;
using CRM.Code.Utils.Enumeration;

namespace CRM.Code.Models
{
    public partial class CRM_AnnualPass : INotes, ICRMContext
    {
        [IsListData("Is Current")]
        public bool IsCurrent
        {
            get
            {
                return UKTime.Now >= this.StartDate && UKTime.Now <= this.ExpiryDate;
            }
        }

        public string ReplacePlaceholders(string output, CRM_Person person)
        {
            CRM_AnnualPass pass = this;
            output = output.Replace("@FIRSTNAME@", person.Firstname);
            output = output.Replace("@LASTNAME@", person.Lastname);
            output = output.Replace("@CARDNUMBER@", pass.CRM_AnnualPassCard.MembershipNumber.ToString());
            output = output.Replace("@STARTDATE@", pass.StartDate.ToString("dd-MM-yyyy"));
            output = output.Replace("@EXPIRY@", pass.ExpiryDate.ToString("dd-MM-yyyy"));
            output = output.Replace("@CURRENTPASS@", pass.CRM_AnnualPassType.Name);
            return output;
        }

        public string DisplayName
        {
            get
            {
                return OutputTableName + " : #" + this.CRM_AnnualPassCard.MembershipNumber + " : " + this.CRM_AnnualPassType.Name + " - expires " + this.ExpiryDateOutput;
            }
        }

        [IsListData("People on card")]
        public string PeopleOnCard
        {
            get
            {
                string people = "";

                foreach (CRM_AnnualPassPerson person in this.CRM_AnnualPassPersons)
                {
                    people += person.CRM_Person.Fullname + Environment.NewLine;
                }

                return people;
            }
        }

        [IsListData("Payment Method")]
        public string PaymentMethodOutput
        {
            get
            {
                return Enumeration.GetStringValue<CRM.Code.Helpers.PaymentType.Types>((int)this.PaymentMethod);
            }
        }

        public string OutputTableName
        {
            get
            {
                return "Annual Pass Record";
            }
        }

        [IsListData("Is Expired")]
        public bool IsExpired
        {
            get
            {
                return !IsCurrent;
            }
        }

        [IsListData("No. of People on Pass")]
        public int PeopleOnPass
        {
            get
            {
                return this.CRM_AnnualPassPersons.Count();
            }
        }

        [IsListData("Expiry Date")]
        public string ExpiryDateOutput
        {
            get
            {
                return this.ExpiryDate.ToString(Constants.DefaultDateStringFormat);
            }
        }

        [IsListData("Type of Pass")]
        public string TypeOfPass
        {
            get
            {
                return this.CRM_AnnualPassType.Name;
            }
        }


        [IsListData("View")]
        public string DetailsURLOutput
        {
            get
            {
                return Utils.Text.Text.ConvertUrlsToLinks(DetailsURL, "View");
            }
        }

        public string DetailsURL
        {
            get
            {
                return "/admin/annualpasscard/annualpass/details.aspx?id=" + this.CRM_AnnualPassCardID + "&pid=" + this.ID;
            }
        }
    }
}