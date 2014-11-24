using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Utils.Enumeration;

namespace CRM.Code.Models
{
    public partial class CRM_Communication
    {
        public static IEnumerable<CRM_Communication> BaseSet(MainDataContext db)
        {
            return db.CRM_Communications;
        }

        public enum Types
        {
            [StringValue("Mail")]
            mail,
            [StringValue("Email")]
            email
        }

        [IsListData("Total Contacts Linked")]
        public int TotalContacted
        {
            get
            {
                return this.CRM_CommunicationLinks.Count();
            }
        }

        public string Type
        {
            get
            {
                return Enumeration.GetStringValue<Types>(this.MailType);
            }
        }

        [IsListData("Uploaded")]
        public string Uploaded
        {
            get
            {
                return this.Timestamp.ToString("dd/MM/yyyy HH:mm");
            }
        }

        public string ExportListURL
        {
            get
            {
                return "/admin/communications/export.ashx?id=" + this.ID;
            }
        }

        [IsListData("View CSV")]
        public string ExportListLink
        {
            get
            {
                return Utils.Text.Text.ConvertUrlsToLinks(ExportListURL, "View");
            }
        }
    }
}