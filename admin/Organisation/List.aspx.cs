using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Code.Models;
using CRM.Code.BasePages.Admin;
using CRM.Code.Helpers;
using CRM.Code.Utils.WebControl;

namespace CRM.admin.Organisation
{
    public partial class List : AdminPage<CRM_Organisation>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            acOrganisation.Config = new AutoCompleteConfig(JSONSet.DataSets.organisation);
            acOrganisation.EventHandler = lnkAutoSearch;
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);

            ucList.Type = typeof(CRM_Organisation);
            BaseSet = db.CRM_Organisations.ToArray();
            CheckQuery(ucList);
            ucList.ItemsPerPage = 10;
        }

        protected void lnkAutoSearch(object sender, EventArgs e)
        {
            CRM_Organisation Item = CRM_Organisation.BaseSet(db).SingleOrDefault(c => c.ID.ToString() == acOrganisation.SelectedID);

            if (Item != null)
                Response.Redirect(Item.DetailsURL);
        }
    }
}