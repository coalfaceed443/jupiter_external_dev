using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace CRM.Code.Interfaces
{
    public interface IArchivable
    {
        int ID { get; set; }
        bool IsActive { get; set; }
        bool IsArchived { get; set; }
        ListItem ListItem { get; }
    }
}