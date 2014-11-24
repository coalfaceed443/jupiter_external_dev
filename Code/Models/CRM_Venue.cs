using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Helpers;
using CRM.Code.Interfaces;
using System.Web.UI.WebControls;

namespace CRM.Code.Models
{
    public partial class CRM_Venue : ICRMRecord, IAutocomplete, ICRMContext, IHistory
    {
        public object ShallowCopy()
        {
            return (CRM_Venue)this.MemberwiseClone();
        }

        public int ParentID
        {
            get
            {
                return 0;
            }
        }

        public string TableName
        {
            get
            {
                return "CRM_Venue";
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

        public string DisplayName
        {
            get
            {
                return OutputTableName + " : " + this.Name;
            }
        }

        public string OutputTableName
        {
            get
            {
                return "Venue Record";
            }
        }

        public ListItem ListItem
        {
            get
            {
                return new ListItem(this.Name + ", " + this.CRM_Address.Postcode, this.ID.ToString());
            }
        }

        public static IEnumerable<CRM_Venue> BaseSet(MainDataContext db)
        {
            return db.CRM_Venues.OrderBy(c => c.Name);
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
                return "/admin/setting/venues/details.aspx?id=" + this.ID;
            }
        }

    }
}