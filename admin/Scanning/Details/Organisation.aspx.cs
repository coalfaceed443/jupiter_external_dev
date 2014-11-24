using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.BasePages.Admin.Persons;
using CRM.Code.Models;
using CRM.Code.Helpers;

namespace CRM.admin.Scanning.Details
{
    public partial class Organisation : CRM_PersonPage<CRM_PersonOrganisation>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucNavScan.Entity = Entity;

            lvOrganisations.Type = typeof(CRM_PersonOrganisation);
            lvOrganisations.DataSet = Entity.CRM_PersonOrganisations.ToArray().Select(p => (object)p);;
            lvOrganisations.ItemsPerPage = 10;
            lvOrganisations.Width = 700;
            lvOrganisations.ShowCustomisation = false;

            if (!Page.IsPostBack)
            {
                PopulateFields();
            }
        }

        protected void PopulateFields()
        {

        }
    }
}