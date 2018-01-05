using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using CRM.Code.Utils.Enumeration;

namespace CRM.Code.Helpers
{
    public class JSONSet
    {
        public string name { get; set; }
        public string value { get; set; }
        public string typeName { get; set; }
        public string objreference { get; set; }
        public string image { get; set; }
        public string title { get; set; }
        public JSONSet(string Name, string type, string Reference, string imageURL, string Title)
        {
            name = Name;
            value = Name;
            typeName = type;
            objreference = Reference;
            image = imageURL;
            title = Title; 
        }


        /// <summary>
        /// Defaults to space seperator
        /// </summary>
        /// <param name="list">list of strings to concat</param>
        /// <param name="seperator">string to seperate each string item</param>
        /// <returns></returns>
        public static string FlattenList(List<string> list, string seperator)
        {
            string flat = "";

            IEnumerable<string> distinctList = list.Distinct();

            for (int i = 0; i < distinctList.Count(); i++)
            {
                flat += list[i] + (i + 1!= distinctList.Count() ? seperator : "");
            }

            return flat;
        }

        public static string FlattenList(List<string> list)
        {
            return FlattenList(list, " ");
        }

        public static string ConvertToJSON(object data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public static List<string> ConvertToTokens(string item)
        {
            return new List<string>()
            {"\"" + item.ToLower() + "\""};
           // return item.ToLower().Split(' ').ToList();
        }

        // no spaces, needed for ashx URL
        public enum DataSets
        {
            [StringValue("venue")]
            venue,
            [StringValue("person")]
            person,
            [StringValue("contact")]
            contact,
            [StringValue("organisation")]
            organisation,
            [StringValue("school")]
            school,
            [StringValue("family")]
            family,
            [StringValue("mytasks")]
            mytasks,
            [StringValue("schoolperson")]
            schoolperson,
            [StringValue("admins")]
            admin,
            [StringValue("schoolorgs")]
            schoolorgs,
            [StringValue("orgperson")]
            orgperson,
            [StringValue("archivedperson")]
            archivedperson


        }

    }
}