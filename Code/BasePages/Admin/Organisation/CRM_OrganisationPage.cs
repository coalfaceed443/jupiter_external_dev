using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;
using CRM.Code.Managers;

namespace CRM.Code.BasePages.Admin.Organisations
{
    public class CRM_OrganisationPage<T> : AdminPage<T>
    {
        public CRM_Organisation Entity { get; set; }

        public new void Page_PreInit(object sender, EventArgs e)
        {
            base.Page_PreInit(sender, e);

            Entity = db.CRM_Organisations.SingleOrDefault(a => a.ID.ToString() == Request.QueryString["id"]);

            if (Entity == null)
                NoticeManager.SetMessage("Organisation not found", "/admin/organisation/list.aspx");
        }
    }
}