using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Code.Interfaces
{
    public interface ICRMRecord
    {
        string OutputTableName { get; }
        int ID { get; }
    }
}