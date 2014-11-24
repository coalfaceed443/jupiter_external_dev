using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Helpers;

namespace CRM.Code.Utils.WebControl
{
    public class AutoCompleteConfig
    {
        public string AdditionJSONQuery { get; set; }
        public string QueryString { get; set; }
        /// <summary>
        /// Control helper for ucAutocomplete
        /// </summary>
        /// <param name="additionalJSONQuery">Any additional query string parameters you wish to include in the autocomplete json query, found in /admin/JSONHandlers/JsonHandler.ashx</param>
        /// <param name="dataset">The dataset to tell the JSON Handler to query</param>
        public AutoCompleteConfig(JSONSet.DataSets dataset, string additionJSONQuery)
        {
            AdditionJSONQuery = additionJSONQuery;
            QueryString = "dataset=" + Enumeration.StringEnum.GetStringValue(dataset) + "&" + additionJSONQuery;
        }

        public AutoCompleteConfig(JSONSet.DataSets dataset) : this(dataset, String.Empty) { }
    }
}