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
    public partial class Families : CRM_PersonPage<CRM_FamilyPerson>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucNavScan.Entity = Entity;

            lvFamilies.Type = typeof(CRM_FamilyPerson);
            lvFamilies.DataSet = Entity.CRM_FamilyPersons.ToArray().Select(p => (object)p);;
            lvFamilies.ItemsPerPage = 10;
            lvFamilies.Width = 700;
            lvFamilies.ShowCustomisation = false;
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