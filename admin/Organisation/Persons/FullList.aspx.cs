using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;
using CRM.Code.BasePages.Admin.Organisations;
using CRM.Code.Utils.WebControl;
using CRM.Code.Helpers;

namespace CRM.admin.Organisation.Persons
{
    public partial class FullList : AdminPage<CRM_PersonOrganisation>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            acOrgPerson.Config = new AutoCompleteConfig(JSONSet.DataSets.orgperson);
            acOrgPerson.EventHandler = lnkAutoSearch;
            ucList.Type = typeof(CRM_PersonOrganisation);
            BaseSet = db.CRM_PersonOrganisations;
            CheckQuery(ucList);
            ucList.ItemsPerPage = 30;
            ucList.CanOrder = false;

        }


        protected void lnkAutoSearch(object sender, EventArgs e)
        {
            CRM_PersonOrganisation Item = db.CRM_PersonOrganisations.SingleOrDefault(c => c.ID.ToString() == acOrgPerson.SelectedID);

            if (Item != null)
                Response.Redirect(Item.DetailsURL);
        }

    }
}