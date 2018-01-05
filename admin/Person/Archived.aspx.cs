using CRM.Code.BasePages.Admin;
using CRM.Code.Helpers;
using CRM.Code.Interfaces;
using CRM.Code.Managers;
using CRM.Code.Models;
using CRM.Code.Utils.WebControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM.admin.Person
{
    public partial class Archived : AdminPage<CRM_Person>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            acPerson.Config = new AutoCompleteConfig(JSONSet.DataSets.archivedperson);
            acPerson.EventHandler = lnkAutoSearch;
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);

            ucList.Type = typeof(CRM_Person);
            BaseSet = db.CRM_Persons.Where(r => r.IsArchived).OrderByDescending(b => b.DateModified);
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