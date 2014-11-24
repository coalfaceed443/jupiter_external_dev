using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin.Calendar;
using CRM.Code.Managers;
using CRM.Code.Utils.Time;
using CRM.Code.Utils.WebControl;
using CRM.Code.Models;
using CRM.Code.BasePages.Admin;
using CRM.Code.Helpers;
using CRM.Code.Utils.WebControl;
using CRM.Code.Models;
using CRM.Code.BasePages.Admin.Organisations;
using CRM.Code.Interfaces;

namespace CRM.admin.Calendar.Invite
{
    public partial class List : CRM_CalendarPage<CRM_CalendarInvite>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);

            AutoCompleteConfig Config = new AutoCompleteConfig(JSONSet.DataSets.contact);
            acContact.Config = Config;
            acContact.EventHandler = lnkAutoSearch;
            Search();

            ucNavCal.Entity = Entity;

        }

        protected void Search()
        {

            ucList.Type = typeof(CRM_CalendarInvite);
            BaseSet = GetBaseSet();
            CheckQuery(ucList);
            ucList.ItemsPerPage = 10;
        }

        protected void btnSubmitChanges_Click(object sender, EventArgs e)
        {
            Search();
        }

        protected IEnumerable<CRM_CalendarInvite> GetBaseSet()
        {
            return Entity.CRM_CalendarInvites;
        }

     
        protected void lnkAutoSearch(object sender, EventArgs e)
        {
            IContact Item = new ContactManager().Contacts.SingleOrDefault(c => c.Reference == acContact.SelectedID);

            if (Item != null)
            {
                if (!Entity.CRM_CalendarInvites.Any((a => a.Reference == Item.Reference)))
                {
                    CRM_CalendarInvite CRM_CalendarInvite = new CRM_CalendarInvite()
                    {
                        CRM_CalendarID = Entity.ID,
                        IsAttended = false,
                        IsBooked = false,
                        IsCancelled = false,
                        IsInvited = false,
                        LastAmendedAdminID = AdminUser.ID,
                        Reference = Item.Reference,
                        ContactName = Item.DisplayName,
                        LastUpdated = UKTime.Now
                    };

                    db.CRM_CalendarInvites.InsertOnSubmit(CRM_CalendarInvite);
                    db.SubmitChanges();
                }
            }

            NoticeManager.SetMessage(Item.DisplayName + " tagged to " + Entity.DisplayName);
        }


    }
}