using CRM.Code.BasePages.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Models;
namespace CRM.admin.AnnualPassCard
{
    public partial class Stats : AdminPage
    {
        protected IEnumerable<IGrouping<string, CRM_Person>> GeoMember;

        protected void Page_Load(object sender, EventArgs e)
        {
            GeoMember = from p in (from p in db.CRM_Persons
                       where p.CRM_AnnualPassPersons.Any(c => c.CRM_AnnualPass.ExpiryDate >= DateTime.Now)
                       select p).ToList()
                       where p.Town.Length > 2
                        group p by p.Town into c          
                        orderby c.Count() descending
                       select c;

        }

        
    }
}