using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;
using CRM.Code.Managers;

namespace CRM.Code.BasePages.Admin.Organisations
{
    public class CRM_SchoolPage<T> : AdminPage<T>
    {
        public CRM_School Entity { get; set; }

        public new void Page_PreInit(object sender, EventArgs e)
        {
            base.Page_PreInit(sender, e);

            Entity = db.CRM_Schools.SingleOrDefault(a => a.ID.ToString() == Request.QueryString["id"]);

            if (Entity == null)
                NoticeManager.SetMessage("School not found", "/admin/school/list.aspx");
        }
    }
}