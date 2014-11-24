using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;
using CRM.Code.Helpers;
using CRM.Controls.Admin.SharedObjects.List;

namespace CRM.Code.Interfaces
{
    public interface IDynamicQueryable
    {
        IEnumerable<ListData> QueryData(UtilListView sender, AdminDataQuery query);
    }
}