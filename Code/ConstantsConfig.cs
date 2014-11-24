using System;
using System.Collections;
using System.Text;
using System.Configuration;
using System.Xml;

namespace CRM.Code
{
    public class ConstantsConfig : ConfigurationSection
    {
        // Create a "errorLogPath" attribute.
        [ConfigurationProperty("errorLogPath", DefaultValue = "/_assets/media/logs/errors/", IsRequired = false)]
        public string ErrorLogPath
        {
            get
            {
                return (string)this["errorLogPath"];
            }
            set
            {
                this["errorLogPath"] = value;
            }
        }

        [ConfigurationProperty("buttonUCPath", DefaultValue = "/Controls/Forms/UserControlButton.ascx", IsRequired = false)]
        public string ButtonUCPath
        {
            get
            {
                return (string)this["buttonUCPath"];
            }
            set
            {
                this["buttonUCPath"] = value;
            }
        }

        [ConfigurationProperty("sendErrorEmails", DefaultValue = false, IsRequired = true)]
        public bool SendErrorEmails
        {
            get
            {
                return (bool)this["sendErrorEmails"];
            }
            set
            {
                this["sendErrorEmails"] = value;
            }
        }
        [ConfigurationProperty("websiteName", IsRequired = true)]
        public string WebsiteName
        {
            get
            {
                return (string)this["websiteName"];
            }
            set
            {
                this["websiteName"] = value;
            }
        }

        [ConfigurationProperty("defaultDateStringFormat", IsRequired = true)]
        public string DefaultDateStringFormat
        {
            get
            {
                return (string)this["defaultDateStringFormat"];
            }
            set
            {
                this["defaultDateStringFormat"] = value;
            }
        }

        [ConfigurationProperty("domainName", IsRequired = true)]
        public string DomainName
        {
            get
            {
                return (string)this["domainName"];
            }
            set
            {
                this["domainName"] = value;
            }
        }

        [ConfigurationProperty("emailLogPath", DefaultValue = "/_assets/media/logs/emails/", IsRequired = false)]
        public string EmailLogPath
        {
            get
            {
                return (string)this["emailLogPath"];
            }
            set
            {
                this["emailLogPath"] = value;
            }
        }

        [ConfigurationProperty("emailTemplatePath", DefaultValue = "/_assets/media/emails/", IsRequired = false)]
        public string EmailTemplatePath
        {
            get
            {
                return (string)this["emailTemplatePath"];
            }
            set
            { this["emailTemplatePath"] = value; }
        }

        [ConfigurationProperty("emailsFrom", DefaultValue = "dev@coalfacedevelopment.com", IsRequired = false)]
        public string EmailsFrom
        {
            get
            {
                return (string)this["emailsFrom"];
            }
            set
            { this["emailsFrom"] = value; }
        }

        [ConfigurationProperty("sendEmailsToDev", DefaultValue = false, IsRequired = false)]
        public bool SendEmailsToDev
        {
            get
            {
                return (bool)this["sendEmailsToDev"];
            }
            set
            { this["sendEmailsToDev"] = value; }
        }

        [ConfigurationProperty("developerEmail", DefaultValue = "dev@coalfacedevelopment.com", IsRequired = false)]
        public string DeveloperEmail
        {
            get
            {
                return (string)this["developerEmail"];
            }
            set
            { this["developerEmail"] = value; }
        }


        [ConfigurationProperty("campaignMonitorAPIKey", DefaultValue = "", IsRequired = false)]
        public string CampaignMonitorAPIKey
        {
            get
            {
                return (string)this["campaignMonitorAPIKey"];
            }
            set
            { this["campaignMonitorAPIKey"] = value; }
        }

        [ConfigurationProperty("emailsTo", DefaultValue = "", IsRequired = false)]
        public string EmailsTo
        {
            get
            {
                return (string)this["emailsTo"];
            }
            set
            { this["emailsTo"] = value; }
        }
        [ConfigurationProperty("isLive", DefaultValue = false, IsRequired = true)]
        public bool IsLive
        {
            get
            {
                return (bool)this["isLive"];
            }
            set
            { this["isLive"] = value; }
        }
        [ConfigurationProperty("isRedirectAllMailToDev", DefaultValue = false, IsRequired = true)]
        public bool IsRedirectAllMailToDev
        {
            get
            {
                return (bool)this["isRedirectAllMailToDev"];
            }
            set
            { this["isRedirectAllMailToDev"] = value; }
        }
        
        /*
        // Create a "color element."
        [ConfigurationProperty("color")]
        public ColorElement Color
        {
            get
            {
                return (ColorElement)this["color"];
            }
            set
            { this["color"] = value; }
        }*/
    }
    /*
    // Define the "font" element
    // with "name" and "size" attributes.
    public class FontElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "Arial", IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\", MinLength = 1, MaxLength = 60)]
        public String Name
        {
            get
            {
                return (String)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("size", DefaultValue = "12", IsRequired = false)]
        [IntegerValidator(ExcludeRange = false, MaxValue = 24, MinValue = 6)]
        public int Size
        {
            get
            { return (int)this["size"]; }
            set
            { this["size"] = value; }
        }
    }

    // Define the "color" element 
    // with "background" and "foreground" attributes.
    public class ColorElement : ConfigurationElement
    {
        [ConfigurationProperty("background", DefaultValue = "FFFFFF", IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\GHIJKLMNOPQRSTUVWXYZ", MinLength = 6, MaxLength = 6)]
        public String Background
        {
            get
            {
                return (String)this["background"];
            }
            set
            {
                this["background"] = value;
            }
        }

        [ConfigurationProperty("foreground", DefaultValue = "000000", IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\GHIJKLMNOPQRSTUVWXYZ", MinLength = 6, MaxLength = 6)]
        public String Foreground
        {
            get
            {
                return (String)this["foreground"];
            }
            set
            {
                this["foreground"] = value;
            }
        }

    }
    */
}
