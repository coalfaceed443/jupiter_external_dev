using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Utils.Enumeration;
using System.Web.UI;

namespace CRM.Code.Models
{

    public partial class CRM_FormField : Utils.Ordering.ListOrderItem
    {

      

        [IsListData("Edit")]
        public string Edit
        {
            get
            {
                return Utils.Text.Text.ConvertUrlsToLinks(this.DetailsURL, "Edit");
            }
        }

        [IsListData("Answers")]
        public string Answers
        {
            get
            {
                return Utils.Text.Text.ConvertUrlsToLinks(this.AnswersURL, "Answers");
            }
        }

        public string AnswersURL
        {
            get
            {
                return "/admin/customfields/answers/list.aspx?id=" + this.ID;
            }
        }

        public string DetailsURL
        {
            get
            {
                return "/admin/customfields/details.aspx?id=" + this.ID;
            }
        }


        [IsListData("Is Required")]
        public string IsRequiredOutput
        {
            get
            {
                return this.IsRequired ? "Yes" : "No";
            }
        }

        [IsListData("Is Active")]
        public string IsActiveOutput
        {
            get
            {
                return this.IsActive ? "Yes" : "No";
            }
        }

        public enum Types : byte
        {
            [StringValue("Single Line Text Box")]
            SingleLineTextBox,
            [StringValue("Multi Line Text Box")]
            MultiLineTextBox,
            [StringValue("Drop Down List")]
            DropDownList,
            [StringValue("Single Check Box")]
            SingleCheckBox,
            [StringValue("Multiple Check Boxes")]
            MultipleCheckBoxes,
            [StringValue("Multiple Radio Buttons")]
            MultipleRadioButtons,
        }

        [IsListData("Type")]
        public string TypeStringValue
        {
            get
            {
                return StringEnum.GetStringValue((Types)Enum.Parse(typeof(Types), Type.ToString()));
            }
        }

        public static IEnumerable<_DataTable> DataTableBaseSet(MainDataContext db)
        {
            return from t in db._DataTables
                    where t.IsAllowCustom
                    orderby t.FriendlyName
                    select t;
        }
    }
}