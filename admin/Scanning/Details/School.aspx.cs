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
    public partial class School : CRM_PersonPage<CRM_Person>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucNavScan.Entity = Entity;

            lvSchool.Type = typeof(CRM_PersonSchool);
            lvSchool.DataSet = Entity.CRM_PersonSchools.ToArray().Select(p => (object)p);;
            lvSchool.ItemsPerPage = 10;
            lvSchool.Width = 700;
            lvSchool.ShowCustomisation = false;

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