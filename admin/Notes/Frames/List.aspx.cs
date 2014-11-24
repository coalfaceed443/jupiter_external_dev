using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;
using CRM.Code.Helpers;

namespace CRM.admin.Notes.Frames
{
    public partial class List : AdminPage<CRM_Note>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucList.Type = typeof(CRM_Note);

            List<string> References = new List<string>();

            References = Request.QueryString["references"].Split(',').ToList();

            BaseSet = CRM_Note.BaseSet(db, References);
            CheckQuery(ucList);
            ucList.ItemsPerPage = 10;
            ucList.CanOrder = false;
            ucList.ShowCustomisation = false;
        }

    }
}