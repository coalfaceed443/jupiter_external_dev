using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;

using CRM.Code.Helpers;
using CRM.Code.BasePages.Admin.Persons;
using System.ComponentModel;
using CRM.Controls.Admin.SharedObjects.List;
using CRM.Code.Interfaces;
using System.Linq.Dynamic;
namespace CRM.admin.Person.Personal
{
    public partial class List : CRM_PersonPage<CRM_PersonPersonal>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);

            BaseSet = Entity.CRM_PersonPersonals;
            CheckQuery(ucList);
            ucList.Type = typeof(CRM_PersonPersonal);        
            ucList.ItemsPerPage = 10;
            ucList.CanOrder = false;         
            ucNavPerson.Entity = Entity;
            CRMContext = Entity;
            

        }


    }
}