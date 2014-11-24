using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin.Calendar;
using CRM.Code.Models;
using CRM.Code.Managers;
using CRM.Code.Utils.Enumeration;
using CRM.Code.Utils.Time;

namespace CRM.admin.Calendar.RSVP
{
    public partial class Default : CRM_CalendarPage<CRM_CalendarAdmin>
    {
        protected bool CanAttend = false;
        protected CRM_CalendarAdmin myInvite;
        protected CRM.Code.Models.Admin Admin;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["attend"]) && Request.QueryString["attend"] == "true")
            {
                CanAttend = true;
            }

            ucNavCal.Entity = Entity;
            btnSendRSVP.EventHandler = btnSendRSVP_Click;
            myInvite = Entity.CRM_CalendarAdmins.FirstOrDefault(f => f.AdminID == AdminUser.ID);

            if (!Page.IsPostBack)
            {
                ddlStatus.DataSource = Enumeration.GetAll<CRM_CalendarAdmin.StatusTypes>();
                ddlStatus.DataBind();

                if (CanAttend)
                    ddlStatus.SelectedValue = ((byte)CRM_CalendarAdmin.StatusTypes.Attending).ToString();
                else
                    ddlStatus.SelectedValue = ((byte)CRM_CalendarAdmin.StatusTypes.NotAttending).ToString();
            }

            if (myInvite == null)
            {
                NoticeManager.SetMessage("You are no longer tagged on this event to RSVP, or are not logged in as the person who received this email", "/admin");
            }
            else
            {
                Admin = db.Admins.Single(c => c.ID == myInvite.CRM_Calendar.CreatedByAdminID);
            }
        }

        protected void btnSendRSVP_Click(object sender, EventArgs e)
        {
            byte status = Convert.ToByte(ddlStatus.SelectedValue);
            myInvite.Status = status;
            db.SubmitChanges();

            EmailManager manager = new EmailManager();
            manager.SendRSVP(txtMessageToTags.Text, myInvite, db, AdminUser);

            CRM.Code.Models.Admin admin = db.Admins.Single(c => c.ID == myInvite.CRM_Calendar.CreatedByAdminID);
            NoticeManager.SetMessage("RSVP Sent to " + admin.DisplayName);


        }
    }
}