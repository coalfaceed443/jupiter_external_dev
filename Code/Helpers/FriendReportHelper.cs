using CRM.Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Code.Helpers
{
    public class FriendReportHelper
    {
        public CRM_AnnualPass CRM_AnnualPass { get; set; }
        public bool IsFriend { get; set; }
        public bool IsPersonalFriend { get; set; }
        public CRM_Person CRM_Person { get; set; }
    }
}