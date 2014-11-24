using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Models;
using System.Web.UI.HtmlControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Utils.Enumeration;

namespace CRM.Controls.Admin.Navigation
{
    public partial class Calendar : System.Web.UI.UserControl
    {
        public CRM.Code.Models.CRM_Calendar Entity { get; set; }

        public string Section { get; set; }
        protected bool DraftsOnly = false;
        protected bool AllowMasterPermissions = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Entity != null)
            {
                System.Web.UI.Control control = divHolder.FindControl(Section);
                if (control != null)
                {
                    ((HtmlGenericControl)control).Attributes["class"] += " current";
                }

                switch (Entity.CRM_CalendarType.FixedRef)
                {
                    case (int)CRM_CalendarType.TypeList.task:
                        {
                            navTask.Visible = true;
                        }
                        break;
                    case (int)CRM_CalendarType.TypeList.outreach:
                        {
                            lnkOutreach.InnerText = Entity.CRM_CalendarType.Name;
                            navOutreach.Visible = true;
                            lnkOutreach.HRef = "/admin/calendar/outreach/details.aspx?id=" + Entity.ID;
                        }
                        break;
                    case (int)CRM_CalendarType.TypeList.schoolvisit:
                    case (int)CRM_CalendarType.TypeList.groupbooking:
                        {
                            lnkGroupSchool.InnerText = Entity.CRM_CalendarType.Name;
                            navGroupBooking.Visible = true;
                            lnkGroupSchool.HRef = "/admin/calendar/groupbookings/details.aspx?id=" + Entity.ID;
                        }
                        break;
                    case (int)CRM_CalendarType.TypeList.party:
                        {
                            navParties.Visible = true;
                        }
                        break;
                    case (int)CRM_CalendarType.TypeList.cpd:
                        {
                            navCDP.Visible = true;
                        }
                        break;
                }
            }
            else
                this.Visible = false;
        }
    }
}