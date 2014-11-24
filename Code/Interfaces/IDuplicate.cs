using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Helpers;
using CRM.Code.Models;

namespace CRM.Code.Interfaces
{
    public interface IDuplicate
    {
        List<string> Tokens { get; }
        IEnumerable<IDuplicate> GetBaseSet(MainDataContext db);
        string ViewRecord { get; }
        int ID {get;}
        IDictionary<int, string> GetKeys();
        Dictionary<byte, string> SearchDictionary { get; }
    }
}