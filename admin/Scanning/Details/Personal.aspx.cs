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
    public partial class Personal : CRM_PersonPage<CRM_PersonPersonal>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucNavScan.Entity = Entity;

            lvPersonal.Type = typeof(CRM_PersonPersonal);
            lvPersonal.DataSet = Entity.CRM_PersonPersonals.ToArray().Select(p => (object)p);;
            lvPersonal.ItemsPerPage = 10;
            lvPersonal.Width = 700;
            lvPersonal.ShowCustomisation = false;
            
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