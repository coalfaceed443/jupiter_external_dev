using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace CRM.Code.Utils.Search
{
    /// <summary>
    /// Summary description for SearchSessionItem
    /// </summary>
    public class SearchSessionItem
    {
        private string[] Prefixes = { "txt", "ddl", "chk", "rad" };
        public string ClientID { get; set; }
        public string Value { get; set; }
        private string _propertyName = "";
        public string PropertyName { get { return _propertyName; } set { _propertyName = value; RemovePrefix(); } }
        public string CustomWhere { get; set; }
        public ValidationDataType DataType { get; set; }
        
        public SearchSessionItem(string client, string val, string property, ValidationDataType datatype)
        {
            ClientID = client;
            Value = val;
            PropertyName = property;
            DataType = datatype;
        }

        public SearchSessionItem(string client, string val, string property, ValidationDataType datatype, string customWhere)
        {
            ClientID = client;
            Value = val;
            PropertyName = property;
            DataType = datatype;
            CustomWhere = customWhere;
        }

        private void RemovePrefix()
        {
            foreach (string prefix in Prefixes)
            {
                _propertyName = _propertyName.Replace(prefix, "");
            }
        }

        public string TypedValue
        {
            get
            {
                switch(DataType)
                {
                    case ValidationDataType.String:
                        return "\"" + Value.ToLower() + "\"";
                    case ValidationDataType.Integer:
                    case ValidationDataType.Double:
                        return Value;
                    case ValidationDataType.Date:
                        return "DateTime.Parse(\"" + Value + "\")";
                    default:
                        return "\"" + Value + "\"";
                }
            }
        }
    }
}