using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Utils.Enumeration;
using CRM.Code.Helpers;
using CRM.Code.Interfaces;

namespace CRM.Code.Models
{
    /// <summary>
    /// Summary description for Order
    /// </summary>
    public partial class Admin : ICRMContext
    {
        public enum AllowedSections : byte
        {
            [StringValue("Admin Users")]
            AdminUsers,
            [StringValue("Calendar")]
            Calendar,
            [StringValue("NotSet")]
            NotSet
        }


        public List<string> Tokens
        {
            get
            {
                List<string> tokens = new List<string>();
                tokens.AddRange(JSONSet.ConvertToTokens(this.DisplayName));
                return tokens;
            }
        }


        public string LoggerName
        {
            get
            {
                return DisplayName + " (" + this.ID + ")";
            }
        }

        public string DisplayName
        {
            get
            {
                return this.FirstName + " " + this.Surname;
            }
        }

        public string TempPhotoFile
        {
            get
            {
                return CRM_Person.PhotoDirectory + this.ID + "_temp.png";
            }
        }
    }
}