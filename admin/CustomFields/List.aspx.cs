using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Models;
using CRM.Code.BasePages.Admin;
using CRM.Code.Helpers;

namespace CRM.admin.CustomFields
{
    public partial class List : AdminPage<CRM_FormField>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                ddlTable.DataSource = CRM_FormField.DataTableBaseSet(db);
                ddlTable.DataBind();

                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                    ddlTable.SelectedValue = Request.QueryString["id"];
            }



            ucList.Type = typeof(CRM_FormField);
            ucList.DataSet = from c in db.CRM_FormFields
                             where c._DataTableID.ToString() == ddlTable.SelectedValue
                             where !c.IsArchived
                             orderby c.OrderNo
                             select ((object)c);
            ucList.ItemsPerPage = 10;
            
            lnkAdd.Visible = ddlTable.SelectedValue != "";
            lnkAdd.HRef = "details.aspx?sid=" + ddlTable.SelectedValue;
        }

        protected void ddlTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            ucList.Reinitialize();
        }

    }
}