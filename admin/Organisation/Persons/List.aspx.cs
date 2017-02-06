using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;
using CRM.Code.BasePages.Admin.Organisations;

namespace CRM.admin.Organisation.Persons
{
    public partial class List : CRM_OrganisationPage<CRM_PersonOrganisation>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucList.Type = typeof(CRM_PersonOrganisation);
            BaseSet = Entity.CRM_PersonOrganisations.Where(r => !r.IsArchived);
            CheckQuery(ucList);
            ucList.ItemsPerPage = 10;
            ucList.CanOrder = false;

            CRMContext = Entity;

            ucNavOrg.Entity = Entity;
        }
    }
}