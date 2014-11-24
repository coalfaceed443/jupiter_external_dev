using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using CRM.Code.WebControl;

namespace CRM.Code.Utils.Search
{
    /// <summary>
    /// Summary description for SearchSession
    /// </summary>
    public class SearchSession
    {
        public List<SearchSessionItem> Items = new List<SearchSessionItem>();
        public Dictionary<string, bool> OrderList = new Dictionary<string, bool>();

        protected string keyName;

        public SearchSession(string keyName)
        {
            this.keyName = keyName;
        }

        public void LoadCriteria()
        {
            if (HttpContext.Current.Session[keyName] != null)
            {
                SearchSession temp = (SearchSession)HttpContext.Current.Session[keyName];

                this.Items = temp.Items;
                this.OrderList = temp.OrderList;
            }
        }

        public void SaveCriteria(ControlCollection collection)
        {
            foreach (System.Web.UI.Control control in collection)
            {
                Type controlType = control.GetType();
                if (controlType.Name == "app_controls_forms_textbox_ascx")
                {
                    string t = ((UserControlBase)control).BaseValue;

                    if (!String.IsNullOrEmpty(t))
                        this.Add(control.ClientID, t, control.ID, ValidationDataType.String);
                    else
                        this.RemoveItem(control.ClientID);
                }
                if (controlType.Name == "TitledDropDownList")
                {
                    string t = ((DropDownList)control).SelectedValue;

                    int selectedInt = 0;
                    if (Int32.TryParse(t, out selectedInt) && selectedInt > -1)
                    {
                        this.Add(control.ClientID, t, control.ID, ValidationDataType.Integer);
                    }
                    else
                    {
                        this.RemoveItem(control.ClientID);
                    }
                }
            }

            HttpContext.Current.Session[keyName] = this;
        }
        public void Add(string clientID, string customWhere)
        {
            Add(clientID, "", "", ValidationDataType.String, customWhere);
        }

        public void Add(string clientID, string value, string property, ValidationDataType dataType, string customWhere)
        {
            SearchSessionItem item = Items.FirstOrDefault(a => a.ClientID == clientID);
            if (item != null)
            {
                item.Value = value;
                item.PropertyName = property;
                item.DataType = dataType;
                item.CustomWhere = customWhere;
            }
            else
            {
                Items.Add(new SearchSessionItem(clientID, value, property, dataType, customWhere));
            }
        }

        public void AddOrder(string orderBy, bool isDescending)
        {
            OrderList.Add(orderBy, isDescending);
        }

        public void Add(string clientID, string value, string property, ValidationDataType dataType)
        {
            Add(clientID, value, property, dataType, "");
        }

        public void RemoveItem(string clientID)
        {
            SearchSessionItem item = Items.FirstOrDefault(a => a.ClientID == clientID);
            if (item != null)
            {
                Items.Remove(item);
            }
        }

        public SearchSessionItem GetByClientID(string clientID)
        {
            return Items.FirstOrDefault(a => a.ClientID.ToLower() == clientID.ToLower());
        }

        public SearchSessionItem GetByPropertyName(string property)
        {
            return Items.FirstOrDefault(a => a.PropertyName.ToLower() == property.ToLower());
        }

        public string GenerateWhereQuery()
        {
            string returnString = "";
            foreach (SearchSessionItem item in Items)
            {
                if (!String.IsNullOrEmpty(returnString))
                    returnString += " AND ";

                if (!String.IsNullOrEmpty(item.CustomWhere))
                    returnString += item.CustomWhere;
                else
                {
                    if (item.DataType == ValidationDataType.String)
                    {
                        returnString += item.PropertyName + ".ToLower().Contains(" + item.TypedValue + ")";

                    }
                    else
                    {
                        returnString += item.PropertyName + " == " + item.TypedValue;
                    }
                }
            }
            return returnString;
        }

        public string GenerateOrderBy()
        {
            string returnString = "";
            foreach (KeyValuePair<string, bool> item in OrderList)
            {
                if (!String.IsNullOrEmpty(returnString))
                    returnString += ", ";

                returnString += item.Key;
                if (item.Value)
                    returnString += " descending";
            }
            return returnString;
        }
    }
}