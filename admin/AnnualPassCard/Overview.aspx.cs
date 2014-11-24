using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM;
using CRM.Code.Models;
using CRM.Code.BasePages.Admin;

namespace CRM.admin.AnnualPassCard
{
    public partial class Overview : AdminPage<CRM_AnnualPassPerson>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            lvAnnualPass.Type = typeof(CRM_AnnualPassPerson);
            BaseSet = db.CRM_AnnualPassPersons;
            CheckQuery(lvAnnualPass);
            lvAnnualPass.ItemsPerPage = 20;
        }
    }
}