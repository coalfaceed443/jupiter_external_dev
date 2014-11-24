using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Text;
using System.Net;
using System.IO;
using CRM.Code.Models;

namespace CRM.Code.Utils.SharedObject
{
    public class SharedObject
    {
        public static IEnumerable<T> GetSharedObjects<T>(MainDataContext db) where T : class
        {
            Type objectType = typeof(T);

            var items = db.Mapping.GetTables();

            IEnumerable<T> sharedObjects = Enumerable.Empty<T>();

            foreach (var item in items.Where(p => objectType.IsAssignableFrom(p.RowType.Type)))
            {
                var queryType = item.RowType.Type;
                sharedObjects = sharedObjects.Concat(db.GetTable(queryType).Cast<T>());
            }

            return sharedObjects;
        }
    }
}