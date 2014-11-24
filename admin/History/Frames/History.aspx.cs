using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM;

namespace CRM.admin.History.Frames
{
    public partial class History : CRM.Code.BasePages.Admin.AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var history = db.TableHistories.Where(h => h.TableName == Request.QueryString["name"]);

            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                history = history.Where(h => h.Key1 == Request.QueryString["id"]);
            }

            if (!String.IsNullOrEmpty(Request.QueryString["parentID"]))
            {
                history = history.Where(h => h.ParentID == Request.QueryString["parentID"]);
            }

            lvItems.DataSource = history;
            lvItems.DataBind();
        }
    }
}