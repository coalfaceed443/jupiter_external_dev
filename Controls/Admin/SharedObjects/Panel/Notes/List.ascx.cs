using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Interfaces;
using CRM.Code.Models;
using CRM.Code.Managers;

namespace CRM.Controls.Admin.SharedObjects.Notes
{
    public partial class List : System.Web.UI.UserControl
    {
        public INotes INotes { get; set; }
        protected string Reference;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Visible = INotes != null;

            if (INotes != null)
            {
                Reference = INotes.Reference;
                litNote.Text = INotes.DisplayName;
                NoteManager manager = new NoteManager(INotes.Reference);
                litUnreadCount.Text = manager.UnreadCount().ToString();
            }
        }
    }
}