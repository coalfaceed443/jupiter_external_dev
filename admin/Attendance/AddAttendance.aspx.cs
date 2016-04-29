using CRM.Code.BasePages.Admin;
using CRM.Code.Managers;
using CRM.Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace CRM.admin.Attendance
{
    public partial class AddAttendance : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);

            btnSubmit.EventHandler = btnSubmit_OnClick;

            if (db.CRM_AttendanceLogGroups.Any(a => a.AddedTimeStamp.Date == DateTime.Today.Date))
            {
                spnPeopleToday.InnerText = db.CRM_AttendanceLogGroups.Where(a => a.AddedTimeStamp.Date == DateTime.Today.Date).Sum(a => a.CRM_AttendanceLogs.Sum(b => b.Quantity)).ToString();
            }

            if (!Page.IsPostBack)
            {

                dcDateOverride.ShowTime = true;

                repTypes.DataSource = db.CRM_AttendancePersonTypes.Where(a => !a.IsArchived && a.IsActive).OrderBy(a => a.OrderNo);
                repTypes.DataBind();
            }
        }

        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            CRM_AttendanceLogGroup newGroup = new CRM_AttendanceLogGroup();

            newGroup.AddedTimeStamp = DateTime.Now;

            if (dcDateOverride.Text != "")
            {
                newGroup.AddedTimeStamp = dcDateOverride.Value;
            }

            db.CRM_AttendanceLogGroups.InsertOnSubmit(newGroup);

            foreach (RepeaterItem RI in repTypes.Items)
            {
                int quantity = 0;
                int.TryParse(((HtmlInputText)RI.FindControl("txtQuantity")).Value, out quantity);

                int typeID = 0;
                int.TryParse(((HtmlInputHidden)RI.FindControl("hdn_typeID")).Value, out typeID);

                if (quantity > 0 && typeID != 0)
                {
                    CRM_AttendanceLog newlog = new CRM_AttendanceLog();

                    db.CRM_AttendanceLogs.InsertOnSubmit(newlog);

                    newlog.CRM_AttendanceLogGroup = newGroup;
                    newlog.CRM_AttendancePersonTypeID = typeID;
                    newlog.Quantity = quantity;
                }
            }

            db.SubmitChanges();

            NoticeManager.SetMessage(newGroup.CRM_AttendanceLogs.Sum(a => a.Quantity).ToString() + " Entries Added");

        }
    }
}