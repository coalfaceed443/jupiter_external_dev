using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Code.Models
{
    public partial class CRM_NavHistory
    {
        public string OutputName
        {
            get
            {
                return this.FriendlyName.Replace(" |  Seven   Stories  CRM", "");
            }
        }

    }
}