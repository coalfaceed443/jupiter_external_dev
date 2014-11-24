using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Interfaces;

namespace CRM.Controls.Admin.SharedObjects.Panel
{
    public partial class LogHistory : System.Web.UI.UserControl
    {
        public IHistory IHistory { get; set; }
        public string TableName { get; set; }
        public string OverrideID { get; set; }
        public string ParentID { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IHistory != null)
            {
                TableName = IHistory.TableName;
                OverrideID = IHistory.ID.ToString();
                ParentID = IHistory.ParentID.ToString();
            }

            this.Visible = IHistory != null || TableName != null;
        }
    }
}