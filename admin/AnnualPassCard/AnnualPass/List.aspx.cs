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

using CRM.Code.BasePages.Admin.AnnualPass;
namespace CRM.admin.AnnualPassCard.AnnualPass
{
    public partial class List : AnnualPassCardPage<CRM_AnnualPass>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            lvAnnualPass.Type = typeof(CRM_AnnualPass);
            lvAnnualPass.DataSet = db.CRM_AnnualPasses.Where(c => c.CRM_AnnualPassCardID == Entity.ID && !c.IsArchived).OrderByDescending(r => r.ExpiryDate).Select(a => (object)a);
            lvAnnualPass.ItemsPerPage = 10;


        }

    }
}