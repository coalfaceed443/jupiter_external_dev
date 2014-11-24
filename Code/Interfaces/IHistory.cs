using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Code.Interfaces
{
    public interface IHistory
    {
        int ID { get; }
        string TableName { get; }
        object ShallowCopy();
        int ParentID { get; }
    }
}