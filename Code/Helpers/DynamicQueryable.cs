using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;
using System.Linq.Dynamic;

namespace CRM.Code.Helpers
{
    public class DynamicQueryable<T>
    {
        public IEnumerable<T> dataset { get; set; }
        public DynamicQueryable(IEnumerable<T> Dataset, AdminDataQuery query)  : this(Dataset)
        {
            dataset = Dataset;

            if (query != null)
                dataset = ExtendQuery(query);        
        }

        public DynamicQueryable(IEnumerable<T> Dataset)
        {
            dataset = Dataset;
        }

        public IEnumerable<T> ExtendQuery(AdminDataQuery query)
        {
            return dataset.AsQueryable().Where(query.ParsedQuery);
        }
    }
}