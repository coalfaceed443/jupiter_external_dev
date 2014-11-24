using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Code.Helpers
{
    public class ListDataItem
    {
        public string DataName { get; set; }
        public object DataItem { get; set; }
        public int Width = 100;
        public string Alignment = "left";

        public ListDataItem(string dataName, object dataItem)
        {
            DataItem = dataItem;
            DataName = dataName;
        }
    }
}