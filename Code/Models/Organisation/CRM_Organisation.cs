using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;
using CRM.Code.Helpers;
using CRM.Code.Utils.List;
using CRM.Code.Utils.Enumeration;

namespace CRM.Code.Models
{
    public partial class CRM_Organisation : IHistory, ICRMRecord, ISchoolOrganisation, INotes, IDuplicate, ICustomField, ICRMContext
    {

        public static IEnumerable<CRM_Organisation> BaseSet(MainDataContext db)
        {
            return db.CRM_Organisations;
        }

        public IEnumerable<IDuplicate> GetBaseSet(MainDataContext db)
        {
            return CRM_Organisation.BaseSet(db).Select(l => (IDuplicate)l);
        }

        public object ShallowCopy()
        {
            return (CRM_Organisation)this.MemberwiseClone();
        }

        public string DisplayName
        {
            get
            {
                return ArchivedOutput + OutputTableName + " : " + this.Name;
            }
        }

        public string OutputTableName
        {
            get
            {
                return "Organisation Record";
            }
        }

        [IsListData("View")]
        public string ViewRecord
        {
            get
            {
                return "<a href=\"" + DetailsURL + "\">View</a>";
            }
        }

        public string DetailsURL
        {
            get
            {
                return "/admin/organisation/details.aspx?id=" + this.ID;
            }
        }

        [IsListData("Total Contacts")]
        public int Contacts
        {
            get
            {
                return this.CRM_PersonOrganisations.Count();
            }
        }

        public string TableName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        public int ParentID
        {
            get
            {
                return 0;
            }
        }

        public List<string> Tokens
        {
            get
            {
                List<string> tokens = new List<string>();
                tokens.AddRange(JSONSet.ConvertToTokens(this.Name));
                tokens.AddRange(this.CRM_Address.Tokens);
                return tokens;
            }
        }

        public enum SearchKeys
        {
            Name,
            Postcode,
            Address1
        }

        public static string DataKey
        {
            get
            { return "data-duplicate"; }
        }

        public IDictionary<int, string> GetKeys()
        {
            return Enumeration.GetAll<SearchKeys>();
        }

        public Dictionary<byte, string> SearchDictionary
        {
            get
            {
                return new Dictionary<byte, string>()
                {
                    {(byte)SearchKeys.Name, this.Name},
                    {(byte)SearchKeys.Postcode, this.CRM_Address.Postcode},
                    {(byte)SearchKeys.Address1, this.CRM_Address.AddressLine1}
                };
            }
        }

        public IEnumerable<CRM_CalendarInvite> Invites
        {
            get
            {
                return MainDataContext.CurrentContext.CRM_CalendarInvites.ToArray().Where(c => this.CRM_PersonOrganisations.Any(cpo => cpo.Reference == c.Reference));
            }
        }

        [IsListData("No. attended")]
        public int TimesAttended
        {
            get
            {
                return Invites.Where(i => i.IsAttended).Count();
            }
        }

        [IsListData("No. invites")]
        public int TimesInvited
        {
            get
            {
                return Invites.Where(i => i.IsInvited).Count();
            }
        }

        [IsListData("No. cancellations")]
        public int TimesCancelled
        {
            get
            {
                return Invites.Where(i => i.IsCancelled).Count();
            }
        }

        [IsListData("Invite to attendance %")]
        public decimal InviteToAttendanceRatio
        {
            get
            {
                if (this.TimesAttended != 0 && this.TimesInvited != 0)
                {
                    return (((decimal)this.TimesAttended / (decimal)this.TimesInvited)) * 100;
                }
                else return 0;
            }
        }


        [IsListData("Organisation Type")]
        public string OrganisationType
        {
            get
            {
                return this.CRM_OrganisationType.Name;
            }
        }

        [IsListData("Is Archived")]
        public string ArchivedOutput
        {
            get
            {
                return this.IsArchived ? " [ARCHIVED] " : "";
            }
        }
    }
}