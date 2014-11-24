using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;

using CRM.Code.Helpers;
using CRM.Code.BasePages.Admin.Persons;

namespace CRM.admin.Person.Organisations
{
    public partial class List : CRM_PersonPage<CRM_PersonOrganisation>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);

            ucList.Type = typeof(CRM_PersonOrganisation);
            ucList.DataSet = Entity.CRM_PersonOrganisations.Select(p => (object)p);;
            ucList.ItemsPerPage = 10;
            ucList.CanOrder = false;

            ucNavPerson.Entity = Entity;
            CRMContext = Entity;
        }


    }
}