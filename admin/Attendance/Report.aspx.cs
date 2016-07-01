using CRM.Code.BasePages.Admin;
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
    public partial class Report : AdminPage
    {
        public int PeopleToday;
        public int GroupsToday;
        public decimal averageGroupSizeToday;

        public int PeopleSearch;
        public int GroupsSearch;
        public decimal averageGroupSizeSearch;

        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);

            IEnumerable<CRM_AttendanceLogGroup> allGroups = db.CRM_AttendanceLogGroups;
            IEnumerable<CRM_AttendanceLogGroup> allGroupsToday = allGroups.Where(a => a.AddedTimeStamp.Date == DateTime.Today.Date);

            PeopleToday = allGroupsToday.Sum(a => a.CRM_AttendanceLogs.Sum(b => b.Quantity));
            GroupsToday = allGroupsToday.Count();
            
            if (GroupsToday > 0)
                averageGroupSizeToday = (decimal)PeopleToday / (decimal)GroupsToday;
       
            if (!Page.IsPostBack)
            {
                if (allGroups.Any())
                {
                    dcDateFrom.Value = DateTime.Now.AddMonths(-1);
                    dcDateTo.Value = allGroups.Max(a => a.AddedTimeStamp);
                }
            }

            ReloadSearch();
        }

        public void ReloadSearch()
        {
            if (!db.CRM_AttendanceLogGroups.Any()) return;

            IQueryable<CRM_AttendanceLogGroup> allGroups = db.CRM_AttendanceLogGroups.Where(a => a.AddedTimeStamp >= dcDateFrom.Value && a.AddedTimeStamp <= dcDateTo.Value);

            IQueryable<CRM_AttendanceLog> allLogs = allGroups.SelectMany(a => a.CRM_AttendanceLogs);

            repSearchTypeTotals.DataSource =
                db.CRM_AttendancePersonTypes.Where(a => !a.IsArchived)
                .OrderBy(a => a.OrderNo)
                .Select(a => new KeyValuePair<string, int>(a.Name, allLogs.Where(l => l.CRM_AttendancePersonType == a).Sum(l => (int?)l.Quantity) ?? 0));

            repSearchTypeTotals.DataBind();

            repEventsTotals.DataSource = allGroups.Where(a => a.CRM_AttendanceEvent != null).GroupBy(a => a.CRM_AttendanceEvent).Select(g => new KeyValuePair<string, int>(g.Key.Name, g.Sum(gi => gi.CRM_AttendanceLogs.Sum(al => al.Quantity))));
            repEventsTotals.DataBind();

            PeopleSearch = allGroups.Any() ? allGroups.Sum(a => a.CRM_AttendanceLogs.Any() ? a.CRM_AttendanceLogs.Sum(b => b.Quantity) : 0) : 0;
            GroupsSearch = allGroups.Count();
            if (GroupsSearch > 0)
                averageGroupSizeSearch = (decimal)PeopleSearch / (decimal)GroupsSearch;
            
        }

        protected void btnReSearch_Click(object sender, EventArgs e)
        {
            ReloadSearch();
        }

        protected void repSearchTypeTotals_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HtmlGenericControl spnTotal = (HtmlGenericControl)e.Item.FindControl("spnTotal");
            IGrouping<CRM_AttendancePersonType, CRM_AttendanceLog> grouped = (IGrouping<CRM_AttendancePersonType, CRM_AttendanceLog>)e.Item.DataItem;

            spnTotal.InnerText = grouped.Sum(a => a.Quantity).ToString();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {

            var attendance = (from p in db.CRM_AttendanceLogs
                              let AttendedOn = p.CRM_AttendanceLogGroup.AddedTimeStamp
                              where AttendedOn >= dcDateFrom.Value
                              where AttendedOn <= dcDateTo.Value
                              orderby AttendedOn
                              select new {
                                 PersonType = p.CRM_AttendancePersonType.Name,
                                 p.Quantity,
                                 AttendedOn,
                                 RecordCreatedOn = p.CRM_AttendanceLogGroup.DateInserted,
                                 Event = p.CRM_AttendanceLogGroup.CRM_AttendanceEvent.Name,
                                 Origin = p.CRM_AttendanceLogGroup.OriginType == 0 ? "Manual Input by Jupiter Staff" : "Website Automated"
                             }).ToArray();

            CRM.Code.CSVExport.GenericExport(attendance, "attendance-export-");
        }
    }
}