using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Managers;
using CRM.Code.Models;
using CRM.Code.Helpers;

namespace CRM.admin.AnnualPassCard
{
    public partial class List : AdminPage<CRM_AnnualPassCard>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lvAnnualPass.Type = typeof(CRM_AnnualPassCard);
            BaseSet = db.CRM_AnnualPassCards;
            CheckQuery(lvAnnualPass);
            lvAnnualPass.ItemsPerPage = 10;
        }
    }
}