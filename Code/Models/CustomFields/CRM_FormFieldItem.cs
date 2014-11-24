using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;

namespace CRM.Code.Models
{
    public class Web_CRM_FormFieldItem
    {

    }

    public partial class CRM_FormFieldItem : Web_CRM_FormFieldItem, Utils.Ordering.ListOrderItem, ICRMRecord
    {
        [IsListData("Is Active")]
        public string IsActiveOutput
        {
            get
            {
                return this.IsActive ? "Yes" : "No";
            }
        }

         public object ShallowCopy()
        {
            return (CRM_FormFieldItem)this.MemberwiseClone();
        }

        public string DisplayName
        {
            get
            {
                return OutputTableName + " : " + this.Label;
            }
        }

        public string OutputTableName
        {
            get
            {
                return "Custom Field Item Record";
            }
        }

        public string TableName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        [IsListData("View")]
        public string View
        {
            get
            {
                return Utils.Text.Text.ConvertUrlsToLinks(this.DetailsURL, "View");
            }
        }

        public string DetailsURL
        {
            get
            {
                return "/admin/customfields/answers/details.aspx?id=" + this.CRM_FormFieldID + "&fid=" + this.ID;
            }
        }
    }
}