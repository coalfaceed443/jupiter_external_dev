using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Code.Models
{
    public partial class CRM_SystemAccessAdmin
    {
        
        public byte BespokeType
        {
            get
            {
                if (BespokeURL != null)
                {
                    return BespokeURL.Contains("details.aspx") ? (byte)Models.CRM_SystemAccess.PageTypes.details : (byte)Models.CRM_SystemAccess.PageTypes.list;
                }
                else
                    return (byte)Models.CRM_SystemAccess.PageTypes.list;
            }
        }
    }
}