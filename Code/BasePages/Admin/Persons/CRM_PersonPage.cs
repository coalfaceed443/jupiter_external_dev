using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;
using CRM.Code.Managers;

namespace CRM.Code.BasePages.Admin.Persons
{
    public class CRM_PersonPage<T> : AdminPage<T>
    {
        public CRM_Person Entity { get; set; }

        public new void Page_PreInit(object sender, EventArgs e)
        {
            base.Page_PreInit(sender, e);

            Entity = db.CRM_Persons.SingleOrDefault(a => a.ID.ToString() == Request.QueryString["id"]);

            if (Entity == null)
                NoticeManager.SetMessage("Person not found", "/admin/person/list.aspx");
        }
    }
}