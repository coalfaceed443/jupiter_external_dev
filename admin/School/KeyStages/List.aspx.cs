﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;

using CRM.Code.Helpers;

namespace CRM.admin.School.KeyStages
{
    public partial class List : AdminPage<CRM_KeyStage>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucList.Type = typeof(CRM_KeyStage);
            BaseSet = CRM_KeyStage.BaseSet(db);
            CheckQuery(ucList);
            ucList.ItemsPerPage = 10;
            ucList.CanOrder = true;
        }
    }
}