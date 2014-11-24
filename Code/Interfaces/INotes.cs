using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Code.Interfaces
{
    public interface INotes
    {
        string Reference { get; }
        string DisplayName { get; }
    }
}