using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;
using CRM.Code.Managers;

namespace CRM.Code.BasePages.Admin.AnnualPass
{
    public class AnnualPassCardPage<T> : AdminPage<T>
    {
        public CRM_AnnualPassCard Entity { get; set; }

        public new void Page_PreInit(object sender, EventArgs e)
        {
            base.Page_PreInit(sender, e);

            Entity = db.CRM_AnnualPassCards.SingleOrDefault(a => a.ID.ToString() == Request.QueryString["id"]);
        }
    }
}