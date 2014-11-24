using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Utils.Enumeration;

namespace CRM.Code
{
    public class Constants
    {
        public static string DropDownNoticeSession
        {
            get
            {
                return "DropDownMessage";
            }
        }

        public static string ButtonUCPath
        {
            get
            {
                return ((ConstantsConfig)System.Configuration.ConfigurationManager.GetSection(
                    "constantsConfigGroup/constantsConfig")).ButtonUCPath;
            }
        }

        public static bool SendErrorEmails
        {
            get
            {
                return ((ConstantsConfig)System.Configuration.ConfigurationManager.GetSection(
                    "constantsConfigGroup/constantsConfig")).SendErrorEmails;
            }
        }

        public static string WebsiteName
        {
            get
            {
                return ((ConstantsConfig)System.Configuration.ConfigurationManager.GetSection(
                    "constantsConfigGroup/constantsConfig")).WebsiteName;
            }
        }

        public static string DefaultDateStringFormat
        {
            get
            {
                return ((ConstantsConfig)System.Configuration.ConfigurationManager.GetSection(
                    "constantsConfigGroup/constantsConfig")).DefaultDateStringFormat;
            }
        }

        public static string DomainName
        {
            get
            {
                return ((ConstantsConfig)System.Configuration.ConfigurationManager.GetSection(
                    "constantsConfigGroup/constantsConfig")).DomainName;
            }
        }

        public static string ErrorLogPath
        {
            get
            {
                return ((ConstantsConfig)System.Configuration.ConfigurationManager.GetSection(
                    "constantsConfigGroup/constantsConfig")).ErrorLogPath;
            }
        }

        public static string EmailLogPath
        {
            get
            {
                return ((ConstantsConfig)System.Configuration.ConfigurationManager.GetSection(
                    "constantsConfigGroup/constantsConfig")).EmailLogPath;
            }
        }

        public static string EmailTemplatePath
        {
            get
            {
                return ((ConstantsConfig)System.Configuration.ConfigurationManager.GetSection(
                    "constantsConfigGroup/constantsConfig")).EmailTemplatePath;
            }
        }
        public static string EmailsFrom
        {
            get
            {
                return ((ConstantsConfig)System.Configuration.ConfigurationManager.GetSection(
                    "constantsConfigGroup/constantsConfig")).EmailsFrom;
            }
        }
        public static bool SendEmailsToDev
        {
            get
            {
                return ((ConstantsConfig)System.Configuration.ConfigurationManager.GetSection(
                    "constantsConfigGroup/constantsConfig")).SendEmailsToDev;
            }
        }
        public static string DeveloperEmail
        {
            get
            {
                return ((ConstantsConfig)System.Configuration.ConfigurationManager.GetSection(
                    "constantsConfigGroup/constantsConfig")).DeveloperEmail;
            }
        }

        public static string CampaignMonitorAPIKey
        {
            get
            {
                return ((ConstantsConfig)System.Configuration.ConfigurationManager.GetSection(
                    "constantsConfigGroup/constantsConfig")).CampaignMonitorAPIKey;
            }
        }

        public static string EmailsTo
        {
            get
            {
                return ((ConstantsConfig)System.Configuration.ConfigurationManager.GetSection(
                    "constantsConfigGroup/constantsConfig")).EmailsTo;
            }
        }
        public static bool IsLive
        {
            get
            {
                return ((ConstantsConfig)System.Configuration.ConfigurationManager.GetSection(
                    "constantsConfigGroup/constantsConfig")).IsLive;
            }
        }

        public static bool IsRedirectAllMailToDev
        {
            get
            {
                return ((ConstantsConfig)System.Configuration.ConfigurationManager.GetSection(
                    "constantsConfigGroup/constantsConfig")).IsRedirectAllMailToDev;
            }
        }


        public enum Months : int
        {
            [StringValue("January")]
            January = 1,
            [StringValue("February")]
            February = 2,
            [StringValue("March")]
            March = 3,
            [StringValue("April")]
            April = 4,
            [StringValue("May")]
            May = 5,
            [StringValue("June")]
            June = 6,
            [StringValue("July")]
            July = 7,
            [StringValue("August")]
            August = 8,
            [StringValue("September")]
            September = 9,
            [StringValue("October")]
            October = 10,
            [StringValue("November")]
            November = 11,
            [StringValue("December")]
            December = 12
        }

        public static Dictionary<int, int> Years()
        {
            DateTime now = DateTime.Now;
            Dictionary<int, int> dictionary = new Dictionary<int, int>();

            for (int i = now.Year - 90; i < now.Year - 5; i++)
            {
                dictionary.Add(i, i);
            }

            return dictionary;
        }

        public struct Sessions
        {
            public const string AdminCategoryFilter = "__AdminCategoryFilter";
        }

        public struct Cookies
        {

        }

        public struct Caches
        {
            public const string AdminProfileJSON = "__AdminProfileJSON";
        }


        public static int[] DetailsPreviewDimensions = { 150, 150 };

    }
}