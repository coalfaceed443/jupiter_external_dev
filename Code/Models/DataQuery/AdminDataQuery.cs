using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace CRM.Code.Models
{
    public partial class AdminDataQuery
    {
        public string ParsedQuery
        {
            get
            {
                StringBuilder query = new StringBuilder();
                var values = this.AdminDataQueryFilters.ToList();

                for (int i = 1; i <= values.Count(); i++)
                {
                    AdminDataQueryFilter filter = values[i -1];

                    if (!filter.IsCustomField)
                    {
                        query.Append("(" + filter.ParsedQuery + ") " + (i < values.Count() ? " " + (filter.Concat) : ""));
                    }
                }

                string output = query.ToString();

                if (output.EndsWith(" OR"))
                {
                    output = output.Substring(0, output.Length - " OR".Length);
                }

                if (output.EndsWith(" AND"))
                {
                    output = output.Substring(0, output.Length - " AND".Length);
                }

                return output;
            }
        }
    }
}