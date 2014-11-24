using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM;
using CRM.Code.Interfaces;
using CRM.Code.Managers;

namespace CRM.admin.History.Frames
{
    public partial class Comms : CRM.Code.BasePages.Admin.AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lvItems.DataSource = db.CRM_CommunicationLinks.Where(c => c.TargetReference == Request.QueryString["reference"]).OrderByDescending(c => c.CRM_Communication.Timestamp);
                lvItems.DataBind();
            }
        }
    }
}