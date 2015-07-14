using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using CRM.Code.Interfaces;
using CRM.Code.Models;

namespace CRM.Code.Helpers
{
    public class ListData : Utils.Ordering.ListOrderItem
    {
        public object DataItem { get; set; }
        public List<ListDataItem> Properties;

        public int ID
        {
            get
            {
                if (IsCRMRecord)
                    return ((ICRMRecord)DataItem).ID;
                else if (DataItem is Utils.Ordering.ListOrderItem)
                    return ((Utils.Ordering.ListOrderItem)DataItem).ID;
                else
                    return 0;
            }
            set
            {

            }
        }

        public string Address
        {
            get
            {
                if (DataItem is IContact && ((IContact)DataItem).PrimaryAddress != null)
                    return ((IContact)DataItem).PrimaryAddress.FormattedAddress;
                else
                    return null;

            }
        }

        public int? AddressID
        {
            get
            {

                if (DataItem is IContact)
                    return ((IContact)DataItem).AddressID;
                else
                    return null;
            }
        }

        public int? RelationshipID
        {
            get
            {
                if (DataItem is IContact)
                {
                    return ((IContact)DataItem).RelationshipID;
                }
                else
                {
                    return null;
                }
            }
        }
        
        public bool IsCRMRecord
        {
            get
            {
                return DataItem is ICRMRecord;
            }
        }

        public bool IsMailable
        {
            get
            {
                return DataItem is IMailable && ((IMailable)DataItem).IsMailable;
            }
        }

        public bool IsEmailable
        {
            get
            {
                return DataItem is IMailable && ((IMailable)DataItem).IsEmailable;
            }
        }

        public bool IsListOrderItem
        {
            get
            {
                return DataItem is Utils.Ordering.ListOrderItem;
            }
        }

        public int OrderNo
        {
            get
            {
                if (DataItem is Utils.Ordering.ListOrderItem)
                    return ((Utils.Ordering.ListOrderItem)DataItem).OrderNo;
                else
                    return 0;
            }
            set
            {
                if (DataItem is Utils.Ordering.ListOrderItem)
                    ((Utils.Ordering.ListOrderItem)DataItem).OrderNo = value;                
            }
        }

        public ListData(object dataItem)
        {
            DataItem = dataItem;
            Properties = new List<ListDataItem>();
            Type type = DataItem.GetType();
            var props = from pi in type.GetProperties()
                        where pi.PropertyType.Namespace == "System"
                        select pi;

            foreach (PropertyInfo pi in props)
            {
                ListDataItem item = new ListDataItem(pi.Name, dataItem);
                Properties.Add(item);
            }
        }
    }
}