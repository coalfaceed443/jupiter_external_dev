using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin.Calendar;
using CRM.Code.Models;
using CRM.Code.History;
using CRM.Code.Helpers;
using CRM.Code.Utils.Enumeration;

namespace CRM.admin.Calendar.Duplicate
{
    public partial class Details : CRM_CalendarPage<CRM_Calendar>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucNavCal.Entity = Entity;
            btnSubmit.EventHandler = btnSubmit_Click;

            lvDuplicated.Type = typeof(CRM_Calendar);
            lvDuplicated.ShowCustomisation = false;
            if (!Page.IsPostBack)
            {

                if (Session[RecentAdditionsKey] != null)
                {
                    LoadList((List<CRM_Calendar>)Session[RecentAdditionsKey]);
                }

                txtDate.Value = Entity.StartDateTime;
                txtEndDate.Value = Entity.EndDateTime;
                txtEndRun.Value = Entity.EndDateTime.AddDays(1);

                chkDayList.DataSource = Enumeration.GetAll<DayOfWeek>();
                chkDayList.DataBind();

                foreach (ListItem item in chkDayList.Items)
                {
                    item.Selected = true;
                }

            }

            pnlEnd.Visible = ddlType.SelectedValue != "";
            
        }

        private string RecentAdditionsKey = "RecentAdditions" + HttpContext.Current.Request.QueryString["id"];
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                List<CRM_Calendar> CreatedItems = new List<CRM_Calendar>();

                if (Session[RecentAdditionsKey] != null)
                {
                    CreatedItems.AddRange((List<CRM_Calendar>)Session[RecentAdditionsKey]);
                }

                if (txtEndRun.Text.Length > 0 && txtEndRun.Value > txtDate.Value)
                {
                    DateTime start = txtDate.Value;
                    DateTime end = txtDate.Value.Date.AddHours(txtEndDate.Value.Hour).AddMinutes(txtEndDate.Value.Minute);

                    if (ddlType.SelectedValue != "")
                    {
                        while (start <= txtEndRun.Value)
                        {



                            int count = 0;

                            if (start.Date <= txtEndRun.Value.Date && count < 100)
                            {
                                count++;

                                DateTime relativeEndDate = start.Add((end - start));

                                foreach (ListItem item in chkDayList.Items)
                                {
                                    if (((byte)start.DayOfWeek).ToString() == item.Value && item.Selected)
                                    {
                                        CRM_Calendar calendar = new CRM_Calendar()
                                        {
                                            CancellationAdminID = null,
                                            CancellationReason = "",
                                            CRM_CalendarTypeID = Entity.CRM_CalendarTypeID,
                                            DateCancelled = null,
                                            StartDateTime = start,
                                            EndDateTime = relativeEndDate,
                                            Status = Entity.Status,
                                            TargetReference = Entity.TargetReference,
                                            RequiresCatering = Entity.RequiresCatering,
                                            PrimaryContactReference = Entity.PrimaryContactReference,
                                            PriceType = Entity.PriceType,
                                            PriceAgreed = Entity.PriceAgreed,
                                            IsInvoiced = false,
                                            IsCancelled = false,
                                            InvoiceTitle = "",
                                            InvoiceLastname = "",
                                            InvoiceFirstname = "",
                                            InvoiceAddressID = null,
                                            DisplayName = Entity.DisplayName,
                                            CreatedByAdminID = AdminUser.ID,
                                            PONumber = ""
                                        };


                                        if (Entity.StartDateTime != start)
                                        {
                                            db.CRM_Calendars.InsertOnSubmit(calendar);
                                            db.SubmitChanges();
                                            CRM.Code.History.History.RecordLinqInsert(AdminUser, calendar);
                                            CreatedItems.Add(calendar);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }

                            bool found = false;

                            switch (ddlType.SelectedValue)
                            {
                                case "day":
                                    found = true;
                                    start = start.AddDays(1);
                                    end = end.AddDays(1);
                                    break;

                                case "week":
                                    found = true;
                                    start = start.AddDays(7);
                                    end = end.AddDays(7);
                                    break;

                                case "month":
                                    found = true;
                                    start = start.AddMonths(1);
                                    end = end.AddMonths(1);
                                    break;

                            }
                        }
                    }
                    else
                    {
                        CRM_Calendar calendar = new CRM_Calendar()
                        {
                            CancellationAdminID = null,
                            CancellationReason = "",
                            CRM_CalendarTypeID = Entity.CRM_CalendarTypeID,
                            DateCancelled = null,
                            StartDateTime = start,
                            EndDateTime = txtEndDate.Value,
                            Status = Entity.Status,
                            TargetReference = Entity.TargetReference,
                            RequiresCatering = Entity.RequiresCatering,
                            PrimaryContactReference = Entity.PrimaryContactReference,
                            PriceType = Entity.PriceType,
                            PriceAgreed = Entity.PriceAgreed,
                            IsInvoiced = false,
                            IsCancelled = false,
                            InvoiceTitle = "",
                            InvoiceLastname = "",
                            InvoiceFirstname = "",
                            InvoiceAddressID = null,
                            DisplayName = Entity.DisplayName,
                            CreatedByAdminID = AdminUser.ID,
                            PONumber = ""
                        };


                        db.CRM_Calendars.InsertOnSubmit(calendar);
                        db.SubmitChanges();
                        CRM.Code.History.History.RecordLinqInsert(AdminUser, calendar);
                        CreatedItems.Add(calendar);

                    }
                }
                db.SubmitChanges();

                Session[RecentAdditionsKey] = CreatedItems;

                LoadList(CreatedItems);
            }
        }

        protected void LoadList(List<CRM_Calendar> items)
        {
            lvDuplicated.DataSet = items.Select(i => (object)i);
            lvDuplicated.Reinitialize();
            lvDuplicated.Visible = true;
        }


    }
}