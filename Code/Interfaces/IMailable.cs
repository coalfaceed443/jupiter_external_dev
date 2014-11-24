using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Code.Interfaces
{
    public interface IMailable
    {
        bool IsMailable { get; }
        bool IsEmailable { get; }
    }
}