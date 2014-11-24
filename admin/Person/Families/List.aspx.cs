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

namespace CRM.admin.Person.Families
{
    public partial class List : CRM_PersonPage<CRM_FamilyPerson>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);

            ucList.Type = typeof(CRM_FamilyPerson);
            ucList.DataSet = Entity.CRM_FamilyPersons.Select(p => (object)p);
            ucList.ItemsPerPage = 10;
            ucList.CanOrder = false;

            ucNavPerson.Entity = Entity;
        }

    }
}