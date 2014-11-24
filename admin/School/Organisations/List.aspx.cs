using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin.Calendar;
using CRM.Code.Managers;
using CRM.Code.Utils.Time;
using CRM.Code.Utils.WebControl;
using CRM.Code.Models;
using CRM.Code.BasePages.Admin;
using CRM.Code.Helpers;
using CRM.Code.Utils.WebControl;
using CRM.Code.Models;
using CRM.Code.BasePages.Admin.Organisations;

namespace CRM.admin.School.Organisations
{
    public partial class List : CRM_SchoolPage<CRM_OrganisationSchool>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);

            AutoCompleteConfig Config = new AutoCompleteConfig(JSONSet.DataSets.organisation);
            acOrganisation.Config = Config;
            acOrganisation.EventHandler = lnkAutoSearch;
            Search();

            ucNavSchool.Entity = Entity;

        }

        protected void Search()
        {

            ucList.Type = typeof(CRM_OrganisationSchool);
            BaseSet = GetBaseSet();
            CheckQuery(ucList);
            ucList.ItemsPerPage = 10;
        }

        protected void btnSubmitChanges_Click(object sender, EventArgs e)
        {
            Search();
        }

        protected IEnumerable<CRM_OrganisationSchool> GetBaseSet()
        {
            return Entity.CRM_OrganisationSchools;
        }

     
        protected void lnkAutoSearch(object sender, EventArgs e)
        {
            CRM.Code.Models.CRM_Organisation Item = db.CRM_Organisations.SingleOrDefault(c => c.ID.ToString() == acOrganisation.SelectedID);

            if (Item != null)
            {
                if (!Entity.CRM_OrganisationSchools.Any((a => a.CRM_OrganisationID == Item.ID)))
                {
                    CRM_OrganisationSchool CRM_OrganisationSchool = new CRM_OrganisationSchool()
                    {
                        CRM_SchoolID = Entity.ID,
                        CRM_OrganisationID = Item.ID
                    };

                    db.CRM_OrganisationSchools.InsertOnSubmit(CRM_OrganisationSchool);
                    db.SubmitChanges();
                }
            }

            NoticeManager.SetMessage(Item.DisplayName + " tagged to " + Entity.DisplayName);
        }


    }
}