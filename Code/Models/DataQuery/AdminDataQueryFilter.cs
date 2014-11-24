using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Code.Models
{
    public partial class AdminDataQueryFilter
    {
        public string ParsedQuery
        {
            get
            {
                string parsed = "";

                string field = this.DataFieldName;
                string value = this.Value;

                bool doToLower = true;
                decimal trydec = 0;
         
                doToLower = !Decimal.TryParse(value, out trydec);
       
                switch (this.Operator)
                {
                    case "!Contains":
                        {
                            if (doToLower)
                                parsed = "!" + field + ".ToLower().Contains(\"" + value + "\".ToLower())";
                            else
                                parsed = "!" + field + ".ToString().Contains(\"" + value + "\")";
                        }
                        break;
                    case "Contains":
                        {

                            if (doToLower)
                                parsed = field + ".ToLower().Contains(\"" + value + "\".ToLower())";
                            else
                                parsed = field + ".ToString().Contains(\"" + value + "\")";
                        } break;
                    case ">":
                    case ">=":
                    case "<=":
                    case "<":
                    case "!=":
                        object item = null;
                        try
                        {
                            item = Convert.ToInt32(value);
                        }
                        catch
                        {
                            try
                            {
                                item = Convert.ToDateTime(value).Ticks;

                                parsed = field + "_T " + this.Operator + " " + item;
                                break;
                            }
                            catch
                            {
                                try
                                {
                                    item = Convert.ToDecimal(value);
                                }
                                catch
                                {
                                    field = field + ".ToLower()";
                                    item = "\"" + value + "\"";
                                }
                            }
                        }

                        parsed = field + " " + this.Operator + " " + item;
                        break;

                    case "==":                    
                    default:
                        {

                            if (this.Value.ToLower() == "true" || this.Value.ToLower() == "false")
                            {
                                parsed = this.DataFieldName + " " + this.Operator + " " + value;
                            }
                            else
                                parsed = field + ".ToString() " + this.Operator + " \"" + value + "\"";
                            break;
                        }
                }

                return parsed;
            }
        }

        public bool IsCustomField
        {
            get
            {
                int result = 0;
                return Int32.TryParse(this.DataFieldName, out result);                
            }
        }
    }
}