using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;
using CRM.Code.Utils.Time;
using CRM.Code.Managers;

namespace CRM.admin.Scanning
{
    public partial class ScanResultFrame : AdminPage
    {
        protected CRM_AnnualPassCard CRM_AnnualPassCard;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!Page.IsPostBack)
            {

                string id = Request.QueryString["id"];

                if (id.StartsWith("000"))
                {
                    id = id.Substring(3, Request.QueryString["id"].Length - 3);
                    Response.Write("<p>Search as : " + id + "</p>");
                }

                lvItems.DataSource = (from a in db.CRM_AnnualPassPersons
                                      where a.CRM_AnnualPass.CRM_AnnualPassCard.MembershipNumber.ToString() == id &&
                                      !a.IsArchived
                                      select a);
                lvItems.DataBind();
                                        
            }
        }

        protected void lvItems_ItemBound(object sender, ListViewItemEventArgs e)
        {
              if (e.Item is ListViewDataItem)
              {
                  CRM_AnnualPassPerson dataItem = (CRM_AnnualPassPerson)((ListViewDataItem)e.Item).DataItem;

                  CheckBox chkCheckIn = ((CheckBox)e.Item.FindControl("chkCheckIn"));
                  DropDownList ddlReasonForVisit = (DropDownList)e.Item.FindControl("ddlReasonForVisit");

                  ddlReasonForVisit.Attributes["data-id"] = dataItem.ID.ToString();
                  chkCheckIn.Attributes["data-id"] = dataItem.ID.ToString();

                  ddlReasonForVisit.DataSource = from d in db.CRM_CalendarTypes
                                                 orderby d.OrderNo
                                                 select d;
                  ddlReasonForVisit.DataBind();

              }
        }

        protected void ddlReasonForVisit_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlReasonForVisit = (DropDownList)sender;

            CRM_CalendarType type = db.CRM_CalendarTypes.First(f => f.ID.ToString() == ddlReasonForVisit.SelectedValue);

            if (type.FixedRef == (int)CRM_CalendarType.TypeList.generic)
            {

                if (!db.CRM_Calendars.Any(c => c.CRM_CalendarType.FixedRef == (int)CRM_CalendarType.TypeList.generic && c.StartDateTime.Date == UKTime.Now.Date))
                {

                    CRM_Calendar Entity = new CRM_Calendar();
                    Entity.CancellationReason = "";
                    Entity.TargetReference = CRM_Calendar.DefaultTargetReference;
                    Entity.PriceAgreed = 0M;
                    Entity.PriceType = 0;
                    Entity.InvoiceTitle = "";
                    Entity.InvoiceFirstname = "";
                    Entity.InvoiceLastname = "";
                    Entity.DatePaid = null;
                    Entity.PONumber = "";
                    Entity.PrimaryContactReference = "";

                    Entity.CRM_CalendarType = db.CRM_CalendarTypes.Single(c => c.ID == Convert.ToInt32(ddlReasonForVisit.SelectedValue));
                    Entity.CreatedByAdminID = 1;
                    Entity.DisplayName = CRM_Calendar.DayVisit_FixedRef;
                    Entity.StartDateTime = UKTime.Now.Date.AddHours(9);
                    Entity.EndDateTime = UKTime.Now.Date.AddHours(10);
                    Entity.Status = 0;
                    Entity.RequiresCatering = false;
                    Entity.PrivacyStatus = 0;

                    db.CRM_Calendars.InsertOnSubmit(Entity);
                    db.SubmitChanges();
                }
            }

            IEnumerable<CRM_Calendar> calendarEvents = from c in db.CRM_Calendars
                                                       where !c.IsCancelled
                                                       where c.StartDateTime.Date == UKTime.Now.Date
                                                       where c.CRM_CalendarTypeID.ToString() == ddlReasonForVisit.SelectedValue
                                                       select c;
            DropDownList ddlAttending = (DropDownList)ddlReasonForVisit.Parent.FindControl("ddlAttending");
            ddlAttending.DataSource = calendarEvents;
            ddlAttending.DataBind();
        }

        protected void chkCheckIn_CheckChanged(object sender, EventArgs e)
        {
            CheckBox chkCheckIn = (CheckBox)sender;
            
            if (chkCheckIn.Checked)
            {
                CRM_AnnualPassPerson person = db.CRM_AnnualPassPersons.Single(c => c.ID.ToString() == chkCheckIn.Attributes["data-id"]);
                DropDownList ddlAttending = (DropDownList)chkCheckIn.Parent.FindControl("ddlAttending");
                CRM_Calendar CRM_Calendar = db.CRM_Calendars.SingleOrDefault(c => c.ID.ToString() == ddlAttending.SelectedValue);

                if (CRM_Calendar == null)
                {
                    CRM_Calendar = db.CRM_Calendars.FirstOrDefault(c => c.CRM_CalendarTypeID == (int)CRM_CalendarType.TypeList.generic && c.StartDateTime.Date == UKTime.Now.Date);

                    if (CRM_Calendar != null)
                    {
                        CRM_CalendarInvite attendance = new CRM_CalendarInvite()
                        {
                            CRM_CalendarID = CRM_Calendar.ID,
                            ContactName = person.DisplayName,
                            Reference = person.CRM_Person.Reference,
                            IsAttended = true,
                            IsBooked = false,
                            IsCancelled = false,
                            IsInvited = false,
                            LastAmendedAdminID = AdminUser.ID,
                            LastUpdated = UKTime.Now
                        };

                        db.CRM_CalendarInvites.InsertOnSubmit(attendance);

                        db.SubmitChanges();
                    }
                }
                else
                {

                    CRM_CalendarInvite attendance = CRM_Calendar.CRM_CalendarInvites.FirstOrDefault(f => f.Reference == person.CRM_Person.Reference);

                    if (attendance == null)
                    {
                        attendance = new CRM_CalendarInvite()
                        {
                            CRM_CalendarID = CRM_Calendar.ID,
                            ContactName = person.DisplayName,
                            Reference = person.CRM_Person.Reference,
                            IsAttended = true,
                            IsBooked = false,
                            IsCancelled = false,
                            IsInvited = false,
                            LastAmendedAdminID = AdminUser.ID,
                            LastUpdated = UKTime.Now
                        };

                        db.CRM_CalendarInvites.InsertOnSubmit(attendance);
                    }
                    else
                    {
                        attendance.IsAttended = true;
                        attendance.LastAmendedAdminID = AdminUser.ID;
                    }

                    db.SubmitChanges();
                }
                
            }
        }
    }
}