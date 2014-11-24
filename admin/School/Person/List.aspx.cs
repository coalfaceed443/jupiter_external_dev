using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Utils.WebControl;
using CRM.Code.Models;
using CRM.Code.Helpers;
using CRM.Code.BasePages.Admin;

namespace CRM.admin.School.Person
{
    public partial class List : AdminPage<CRM_PersonSchool>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            acSchoolPerson.Config = new AutoCompleteConfig(JSONSet.DataSets.schoolperson);
            acSchoolPerson.EventHandler = lnkAutoSearch;

            ucList.Type = typeof(CRM_PersonSchool);
            BaseSet = db.CRM_PersonSchools;
            CheckQuery(ucList);
            ucList.ItemsPerPage = 30;
        }

        protected void lnkAutoSearch(object sender, EventArgs e)
        {
            CRM_PersonSchool Item = db.CRM_PersonSchools.SingleOrDefault(c => c.ID.ToString() == acSchoolPerson.SelectedID);

            if (Item != null)
                Response.Redirect(Item.DetailsURL);
        }

    }
}