using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.Linq.Expressions;
using System.IO;
using System.Collections.Generic;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;
using CRM.Code.Helpers;

namespace CRM.Admin
{
    public partial class Default : AdminPage<CRM_Task>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            lvTasks.Type = typeof(CRM_Task);
            lvTasks.DataSet = from t in
                                  (from t in db.CRM_Tasks
                                   where t.DueDate <= DateTime.Now.AddDays(30)
                                   where t.DueDate >= DateTime.Now.Date
                                   where !t.IsComplete
                                   where !t.IsCancelled
                                   select t).ToArray()
                              where t.IsVisible(AdminUser.ID)
                              select (object)t;
            lvTasks.ItemsPerPage = 30;
            lvTasks.CanOrder = false;


            lvOverdue.Type = typeof(CRM_Task);
            lvOverdue.DataSet = from t in
                                  (from t in db.CRM_Tasks
                                   where t.DueDate < DateTime.Now.Date
                                   where !t.IsComplete
                                   where !t.IsCancelled
                                   select t).ToArray()
                              where t.IsVisible(AdminUser.ID)
                                select (object)t;
            lvOverdue.ItemsPerPage = 30;
            lvOverdue.CanOrder = false;

        }

    }

}