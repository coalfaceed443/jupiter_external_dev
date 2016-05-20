using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Helpers;
using CRM.Code.Interfaces;
using CRM.Code.Utils.Enumeration;

namespace CRM.Code.Models
{
    public partial class CRM_Address : IAddress, IHistory
    {

        public enum Types
        {
            [StringValue("Home")]
            Home,
            [StringValue("Business")]
            Business
        }

        public object ShallowCopy()
        {
            return (CRM_Address)this.MemberwiseClone();
        }

        public List<string> Tokens
        {
            get
            {
                List<string> tokens = new List<string>();
                tokens.AddRange(JSONSet.ConvertToTokens(this.FormattedAddressBySep(" <br/>")));
                return tokens;
            }
        }

        public string TableName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        public string UID
        {
            get
            {
                return this.AddressLine1 + this.Postcode.Replace(" ", "");
            }
        }

        private int _parentID = 0;
        public int ParentID
        {
            get
            {
                return _parentID;
            }
            set
            {
                _parentID = value;
            }
        }

        public string FormattedAddress
        {
            get
            {
                return FormattedAddressBySep(", ");
            }
        }

        public string LabelOutput(string name, string organisation, string role)
        {

            string outputLabel = "";
            if (!String.IsNullOrEmpty(name))
            {
                outputLabel += name + Environment.NewLine;
            }

            if (!String.IsNullOrEmpty(role))
            {
                outputLabel += role + Environment.NewLine;
            }

            if (!String.IsNullOrEmpty(organisation))
            {
                outputLabel += organisation + Environment.NewLine;
            }



            return outputLabel += this.FormattedAddressBySep(Environment.NewLine);
        }

        public string FormattedAddressBySep(object Seperator)
        {
            string OutputAddress = "";

            OutputAddress += this.AddressLine1 + Seperator;

            if (!String.IsNullOrEmpty(this.AddressLine2))
                OutputAddress += this.AddressLine2 + Seperator;
            if (!String.IsNullOrEmpty(this.AddressLine3))
                OutputAddress += this.AddressLine3 + Seperator;
            if (!String.IsNullOrEmpty(this.Town))
                OutputAddress += this.Town + Seperator;
            if (!String.IsNullOrEmpty(this.County))
                OutputAddress += this.County + Seperator;
            if (!String.IsNullOrEmpty(this.Postcode))
                OutputAddress += this.Postcode;

            return OutputAddress;
        }


    }
}