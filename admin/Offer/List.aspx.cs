using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;

using CRM.Code.Helpers;

namespace CRM.admin.Offer
{
    public partial class List : AdminPage<CRM_Offer>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucList.Type = typeof(CRM_Offer);
            BaseSet = CRM_Offer.BaseSet(db);
            CheckQuery(ucList);
            ucList.ItemsPerPage = 10;        
        }

    }
}