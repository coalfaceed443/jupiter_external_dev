using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;
using CRM.Code.Interfaces;
using CRM.Code.Helpers;
using CRM.Code.Utils.WebControl;
using CRM.Code.Managers;

namespace CRM.admin.Person
{
    public partial class List : AdminPage<CRM_Person>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            acPerson.Config = new AutoCompleteConfig(JSONSet.DataSets.contact);
            acPerson.EventHandler = lnkAutoSearch;
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);

            ucList.Type = typeof(CRM_Person);
            BaseSet = CRM_Person.BaseSet(db).OrderByDescending(b => b.DateModified);
            ucList = CheckQuery(ucList);
            ucList.ItemsPerPage = 10;
            ucList.CanOrder = false;

        }


        protected void lnkAutoSearch(object sender, EventArgs e)
        {
            ContactManager manager = new ContactManager();


            IContact Person = manager.Contacts.SingleOrDefault(c => c.Reference.ToString() == acPerson.SelectedID);
            
            if (Person != null)
                Response.Redirect(Person.Parent_CRM_Person.DetailsURL);
        }
    }
}