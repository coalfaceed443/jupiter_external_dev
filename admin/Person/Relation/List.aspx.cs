using CRM.Code.BasePages.Admin;
using CRM.Code.BasePages.Admin.Persons;
using CRM.Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM.admin.Person.Relation
{
    public partial class List : CRM_PersonPage<CRM_PersonRelationship>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);

            ucList.Type = typeof(CRM_PersonRelationship);
            ucList.DataSet = Entity.Relationships.Where(r => !r.IsArchived).Select(p => (object)p);
            ucList.ItemsPerPage = 10;
            ucList.CanOrder = false;
            CRMContext = Entity;
            ucNavPerson.Entity = Entity;
        }
    }
}