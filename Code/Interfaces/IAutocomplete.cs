using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Code.Interfaces
{
    public interface IAutocomplete
    {
        string Reference { get; }
        string Name { get; }
        string DetailsURL { get; }
        string OutputTableName { get; }
        string DisplayName { get; }
        List<string> Tokens { get; }
    }
}