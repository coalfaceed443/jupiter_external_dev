using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Code.Models
{
    public partial class TemplateEmail
    {
        public static IEnumerable<TemplateEmail> BaseSet(MainDataContext db)
        {
            return from p in db.TemplateEmails
                   orderby p.Name
                   select p;
        }

        [IsListData("Is Enabled")]
        public string IsEnabledOutput
        {
            get
            {
                return !this.IsDisabled ? "Yes" : "No";
            }
        }

        [IsListData("View")]
        public string AdminDetailsOutput
        {
            get
            {
                return Utils.Text.Text.ConvertUrlsToLinks(AdminDetailsURL, "Edit");
            }
        }

        public string AdminDetailsURL
        {
            get
            {
                return "/admin/communications/emails/details.aspx?id=" + this.ID;
            }
        }

        public struct TemplateEmails
        {
            public static string RenewalEmail = "Renewal Email";
        }

    }
}