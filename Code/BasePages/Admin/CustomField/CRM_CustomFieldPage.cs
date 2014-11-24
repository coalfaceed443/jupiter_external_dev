using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;
using CRM.Code.Managers;

namespace CRM.Code.BasePages.Admin.Persons
{
    public class CRM_CustomFieldPage<T> : AdminPage<T>
    {
        public CRM_FormField Entity { get; set; }

        public new void Page_PreInit(object sender, EventArgs e)
        {
            base.Page_PreInit(sender, e);

            Entity = db.CRM_FormFields.SingleOrDefault(a => a.ID.ToString() == Request.QueryString["id"]);

            if (Entity == null)
                NoticeManager.SetMessage("Field not found", "/admin/customfields/list.aspx");
        }
    }
}