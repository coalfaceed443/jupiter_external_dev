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

namespace CRM.admin.School
{
    public partial class List : AdminPage<CRM_School> 
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            acSchool.Config = new AutoCompleteConfig(JSONSet.DataSets.school);
            acSchool.EventHandler = lnkAutoSearch;
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);

            ucList.Type = typeof(CRM_School);
            BaseSet = db.CRM_Schools;
            CheckQuery(ucList);
            ucList.ItemsPerPage = 30;
        }

        protected void lnkAutoSearch(object sender, EventArgs e)
        {
            CRM_School Item = CRM_School.BaseSet(db).SingleOrDefault(c => c.ID.ToString() == acSchool.SelectedID);

            if (Item != null)
                Response.Redirect(Item.DetailsURL);
        }

    }
}