using CRM.Code;
using CRM.Code.BasePages.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM.admin.AnnualPassCard
{
    public partial class Reports : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnExportAudit.EventHandler = btnExportAudit_Click;
        }

        protected void btnExportAudit_Click(object sender, EventArgs e)
        {
            
            var startDate = new DateTime(DateTime.Now.Year - 1, 01, 01);
            var endDate = new DateTime(DateTime.Now.Year -1, 12, 31);

            var members = from p in db.CRM_AnnualPasses
                where p.StartDate >= startDate
                where p.StartDate <= endDate
                where !p.IsArchived
                select p;

            CSVExport.MemberAudit(members, Response);


        }

    }
}