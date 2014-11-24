using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;
using CRM.Code.BasePages.Admin.Persons;
namespace CRM.admin.Person.Passes
{
    public partial class List : CRM_PersonPage<CRM_AnnualPassPerson>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            ucList.Type = typeof(CRM_AnnualPassPerson);
            BaseSet = Entity.CRM_AnnualPassPersons.Where(c => !c.IsArchived && !c.CRM_AnnualPass.IsArchived);
            CheckQuery(ucList);
            ucList.ItemsPerPage = 10;
            ucList.CanOrder = false;

            ucNavPerson.Entity = Entity;
            CRMContext = Entity;
        }
    }
}