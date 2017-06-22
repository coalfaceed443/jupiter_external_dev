using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;

namespace CRM.Code.Models
{
    public partial class CRM_AnnualPassType : ICRMContext, IHistory
    {

        public string NameandPrice
        {
            get
            {
                return this.Name + " - £" + DefaultPriceOutput;
            }
        }

        [IsListData("Default Price")]
        public string DefaultPriceOutput
        {
            get
            {
                return this.DefaultPrice.ToString("N2");
            }
        }

        public static IEnumerable<CRM_AnnualPassType> BaseSet(MainDataContext db)
        {
            return db.CRM_AnnualPassTypes.OrderBy(o => o.Name);
        }

        public object ShallowCopy()
        {
            return (CRM_AnnualPassType)this.MemberwiseClone();
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
                return "Annual Pass Type";
            }
        }


        [IsListData("Show on website")]
        public string ShowOnWebsiteOutput
        {
            get
            {
                return IsWebsite ? "Yes" : "No";
            }
        }

        [IsListData("Type")]
        public string TypeOutput
        {
            get
            {
                string type = "";

                switch (this.Type)
                {
                    case 0:
                        type = "Single";
                        break;

                    case 1:
                        type = "Joint";
                        break;

                    case 2:
                        type = "Group";
                        break;
                }

                return type;
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
                return "/admin/annualpasscard/types/details.aspx?id=" + this.ID;
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
    }
}