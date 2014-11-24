using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Utils.WebControl;
using CRM.Code.Models;
using CRM.Code.BasePages.Admin;
using CRM.Code.Helpers;
using CRM.Code.Utils.WebControl;

namespace CRM.admin.Calendar.Task
{
    public partial class List : AdminPage<CRM_Task>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);

            btnSubmitChanges.EventHandler = btnSubmitChanges_Click;

            acTaskName.EventHandler = lnkAutoSearch;
            AutoCompleteConfig config = new AutoCompleteConfig(JSONSet.DataSets.mytasks, "&adminuserid=" + AdminUser.ID);
            acTaskName.Config = config;
            acTaskName.AtLeastTextRequired = true;
            ucList.Type = typeof(CRM_Task);
            
            BaseSet = QueriedSet();
            ucList = CheckQuery(ucList);
            ucList.ItemsPerPage = 10;

        }

        protected void Search()
        {

            ucList.Type = typeof(CRM_Task);
            ucList.DataSet = QueriedSet().ToArray().Select(p => (object)p);;
            ucList.ItemsPerPage = 10;
            ucList.Reinitialize();
        }

        protected void btnSubmitChanges_Click(object sender, EventArgs e)
        {
            Search();
        }

        protected IEnumerable<CRM_Task> GetBaseSet()
        {
            int adminuserID = AdminUser.ID;
            return db.CRM_Tasks.ToArray().Where(c => c.IsVisible(adminuserID));
        }

        protected IEnumerable<CRM_Task> QueriedSet()
        {
            var data = GetBaseSet();

            if (acTaskName.Text != "")
            {
                data = from d in data.Where(d => d.Name.ToLower().Contains(acTaskName.Text.ToLower()))
                       select d;

            }

            if (txtDueDate.Text != "")
            {
                data = from d in data
                       where d.DueDate.Date == txtDueDate.Value
                       select d;
            }

            if (chkHideCompleted.Checked)
            {
                data = from d in data
                       where !d.IsComplete
                       select d;
            }

            if (chkOwnTasks.Checked)
            {
                data = from d in data
                       where d.CRM_TaskParticipants.Any(t => t.AdminID == AdminUser.ID)
                       select d;
            }

            return data;
        }

        protected void lnkAutoSearch(object sender, EventArgs e)
        {
            CRM_Task Item = GetBaseSet().SingleOrDefault(c => c.ID.ToString() == acTaskName.SelectedID);

            if (Item != null)
                Response.Redirect(Item.DetailsURL);
        }


    }
}