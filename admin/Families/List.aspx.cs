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
namespace CRM.admin.Families
{
    public partial class List : AdminPage<CRM_Family>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            acFamily.Config = new AutoCompleteConfig(JSONSet.DataSets.family);
            acFamily.EventHandler = lnkAutoSearch;
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);

            ucList.Type = typeof(CRM_Family);
            BaseSet = db.CRM_Families;
            CheckQuery(ucList);
            ucList.ItemsPerPage = 30;
        }

        protected void lnkAutoSearch(object sender, EventArgs e)
        {
            CRM_Family Item = CRM_Family.BaseSet(db).SingleOrDefault(c => c.ID.ToString() == acFamily.SelectedID);

            if (Item != null)
                Response.Redirect(Item.DetailsURL);
        }
    }
}