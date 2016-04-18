using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Code.Interfaces
{
    public interface ILabel
    {
        string Reference { get; }
        string LabelName { get;  }
        string LabelOrganisation { get;  }
        string LabelAddress { get;  }
        int LabelCRM_AddressID { get; }
        bool LabelIsPrimaryAddress { get; }
        string OutputTableName { get; }
    }
}