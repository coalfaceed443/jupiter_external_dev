using CRM.Code;
using CRM.Code.BasePages.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM.admin.Organisation
{
    public partial class Reports : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            btnExportOrgContacts.EventHandler = btnExportOrgContacts_Click;


        }

        private void btnExportOrgContacts_Click(object sender, EventArgs e)
        {
            var dataset = from p in db.CRM_PersonOrganisations
                          where !p.CRM_Organisation.IsArchived
                          where !p.CRM_Person.IsArchived
                          let Role = p.CRM_Role.Name
                          orderby p.CRM_Organisation.Name
                          select new
                          {
                              p.CRM_OrganisationID,
                              p.CRM_Organisation.Name,
                              p.Title,
                              p.Firstname,
                              p.Lastname,
                              Role,
                              p.Email,
                              p.Telephone,
                              p.OrgAddress1,
                              p.OrgAddress2,
                              p.OrgAddress3,
                              p.OrgPostcode

                          };

            CSVExport.GenericExport(dataset.ToArray(), "org-contacts");
        }
    }
}