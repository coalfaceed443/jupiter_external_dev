using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;

using CRM.Code.Helpers;

namespace CRM.admin.Setting.Venues
{
    public partial class List : AdminPage<CRM_Venue>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucList.Type = typeof(CRM_Venue);
            BaseSet = CRM_Venue.BaseSet(db);
            CheckQuery(ucList);
            ucList.ItemsPerPage = 30;
            ucList.CanOrder = true;
        }

    }
}