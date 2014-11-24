using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Utils.SharedObject;
using CRM.Code.Models;

namespace Models
{
    public interface ISearchItem
    {
        int GetWeight(string search);
        int DisplayOrder { get; }
    }

    public static class SearchItem
    {
        public static IEnumerable<ISearchItem> GetAll(MainDataContext db)
        {
            return SharedObject.GetSharedObjects<ISearchItem>(db).OrderBy(p => p.DisplayOrder);
        }
    }
}