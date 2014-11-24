using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Models;
using CRM.Code.BasePages.Admin.Persons;

namespace CRM.admin.Person.Gift
{
    public partial class List : CRM_PersonPage<CRM_FundraisingGiftProfile>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucNavPerson.Entity = Entity;
            ucList.Type = typeof(CRM_FundraisingGiftProfile);
            BaseSet = Entity.CRM_FundraisingGiftProfiles;
            ucList = CheckQuery(ucList);
            ucList.ItemsPerPage = 10;
            ucList.CanOrder = false;

        }
    }
}