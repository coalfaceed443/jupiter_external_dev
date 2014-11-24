using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin.Persons;
using CRM.Code.Models;
using CRM.Code.Helpers;

namespace CRM.admin.CustomFields.Answers
{
    public partial class List : CRM_CustomFieldPage<CRM_FormField>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                lnkAdd.HRef = "details.aspx?id=" + Entity.ID;
            }

            navForm.Entity = Entity;

            ucList.Type = typeof(CRM_FormFieldItem);
            ucList.DataSet = from c in db.CRM_FormFieldItems
                             where c.CRM_FormFieldID == Entity.ID
                             where !c.IsArchived
                             orderby c.OrderNo
                             select (object)c;
            ucList.ItemsPerPage = 10;
            ucList.CanOrder = true;
            ucList.ShowExport = false;
            
        }

    }
}