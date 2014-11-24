using System;
using System.Collections.Generic;
using System.Web;
using System.Reflection;
using System.Collections;

namespace CRM.Code.Utils.Json
{    
    [global::System.Serializable]
    public class JSONException : Exception
    {
        public JSONException() { }
        public JSONException(string message) : base(message) { }
        public JSONException(string message, Exception inner) : base(message, inner) { }
        protected JSONException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    public enum JSONType
    {
        String,
        Number,
        Hash,
        Array,
        Unknown
    }
    
    /// <summary>
    /// JSON class that will take any C# object and render it to JSON
    /// This becomes especially useful when using C# 3.0 anonymous
    /// classes and Linq
    /// </summary>
    public class JSON
    {
        public string JSONValue { get; set; }
        public List<JSON> Children { get; set; }

        private Type type;
        private JSONType jsonType;

        public JSON(object obj) : this(obj, 0, false) { }
        public JSON(object obj, bool prettyPrint) : this(obj, 0, prettyPrint) { }

        /// <summary>
        /// Specifies the maximum depth of recursion we can reach before
        /// throwing an exception-this also prevents circular references
        /// causing a stack overflow
        /// </summary>
        private const int MAXIMUM_DEPTH = 20;          
                        
	    private JSON(object obj, int depth, bool prettyPrint)
	    {                                    
            if (++depth > MAXIMUM_DEPTH)
            {
                throw new JSONException("Depth too high; do any of your " +
                    "objects contains circular references?");
            }

            JSONValue = "";           

            if (obj == null)
            {
                // TODO: Should probably give it its own type
                jsonType = JSONType.String;
                JSONValue += "null";
                return;
            }
                   
            this.type = obj.GetType();
            this.jsonType = JSONType.Unknown;
            this.Children = new List<JSON>();
           
            if (this.type == typeof(string))
            {
                this.jsonType = JSONType.String;
                this.JSONValue += "\"" + this.EscapeString((string) obj) + "\"";
            }
            else
            if (this.type == typeof(double) || 
                this.type == typeof(float)  || 
                this.type == typeof(int)    ||
                this.type == typeof(byte)   ||
                this.type == typeof(decimal) )
            {
                this.jsonType = JSONType.Number;

                this.JSONValue = this.EscapeString(obj.ToString());
            }
            else if (this.type == typeof(bool))
            {
                this.jsonType = JSONType.Number;
                this.JSONValue += obj.ToString().ToLower();
            }            
            else if (type.GetInterface("IEnumerable", false) != null)
            {
                IEnumerable ienum = (IEnumerable)obj;
                string array = "[";


                foreach (object o in ienum)
                {
                    JSON item = new JSON(o);
                    array += item.JSONValue + ",";
                }

                this.JSONValue += array.TrimEnd(',') + "]";
            }
                 /* Object has public properties - guess we 
                  * want to treat it as a hash */
            else if (this.type.GetProperties().Length > 0)
            {
                this.jsonType = JSONType.Hash;              

                // we're only interested in public properties
                PropertyInfo[] properties =
                    type.GetProperties();

                string objectValues = "";

                foreach (PropertyInfo pr in properties)
                {
                    try
                    {                    
                        Object prValue = pr.GetValue(obj, null);

                        JSON jsonProperty =
                            new JSON(prValue, depth, prettyPrint);

                        objectValues +=
                            "\"" + pr.Name + "\":" + jsonProperty.JSONValue + ","; 
                    }
                    catch (Exception ex)
                    {
                        throw new JSONException("Property issue with " +
                                           type.Name + " with property "
                                           + pr.Name,
                                           ex);                           
                    }
                }                

                this.JSONValue += "{" + objectValues.TrimEnd(',') + "}";
            }            
            else
            {
                throw new JSONException("Could not convert type '" 
                    + type.Name + "' to JSON!");
           }

	    }

        private string EscapeString(string s)
        {
            return s.Replace("\"", "\'").Replace("&", "&amp;");
        }
    }
}
