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
namespace CRM.admin.Calendar.UserTags
{
    public partial class List : CRM_CalendarPage<CRM_Calendar>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);

            AutoCompleteConfig Config = new AutoCompleteConfig(JSONSet.DataSets.admin, "&adminuserid=" + AdminUser.ID);
            acAdminUser.Config = Config;
            acAdminUser.AtLeastTextRequired = true;
            acAdminUser.EventHandler = lnkAutoSearch;
            Search();

            ucNavCal.Entity = Entity;

        }

        protected void Search()
        {
            lvItems.DataSource = from v in Entity.CRM_CalendarAdmins
                                 orderby v.AdminName
                                 select v;

            lvItems.DataBind();

        }

        protected void btnSubmitChanges_Click(object sender, EventArgs e)
        {
            Search();
        }

        protected IEnumerable<CRM_CalendarAdmin> GetBaseSet()
        {
            return Entity.CRM_CalendarAdmins.ToArray();
        }

     
        protected void lnkAutoSearch(object sender, EventArgs e)
        {
            CRM.Code.Models.Admin Item = db.Admins.SingleOrDefault(c => c.ID.ToString() == acAdminUser.SelectedID);

            if (Item != null)
            {
                if (!Entity.CRM_CalendarAdmins.Any((a => a.AdminID == Item.ID)))
                {
                    CRM_CalendarAdmin CRM_CalendarAdmin = new CRM_CalendarAdmin();
                    CRM_CalendarAdmin.Timestamp = UKTime.Now;
                    CRM_CalendarAdmin.AdminID = Item.ID;
                    CRM_CalendarAdmin.CRM_CalendarID = Entity.ID;
                    CRM_CalendarAdmin.Status = (byte)CRM_CalendarAdmin.StatusTypes.NotResponded;

                    db.CRM_CalendarAdmins.InsertOnSubmit(CRM_CalendarAdmin);
                    db.SubmitChanges();

                    EmailManager manager = new EmailManager();
                    manager.SendNewInvite(txtMessage.Text, CRM_CalendarAdmin, db, ((AdminPage)Page).AdminUser);
                }
            }

            NoticeManager.SetMessage(Item.DisplayName + " tagged to " + Entity.DisplayName);
        }

        protected void lvItems_ItemBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item is ListViewDataItem)
            {                
                CRM_CalendarAdmin item = (CRM_CalendarAdmin)((ListViewDataItem)e.Item).DataItem;
                LinkButton lnkRemove = (LinkButton)e.Item.FindControl("lnkRemove");

                lnkRemove.CommandArgument = item.ID.ToString();
            }
        }

        protected void lnkRemove_Click(object sender, EventArgs e)
        {
            CRM_CalendarAdmin item = db.CRM_CalendarAdmins.Single(c => c.ID.ToString() == ((LinkButton)sender).CommandArgument);

            EmailManager manager = new EmailManager();
            manager.SendUserRemoved(txtMessage.Text, item, db, AdminUser);

            db.CRM_CalendarAdmins.DeleteOnSubmit(item);
            db.SubmitChanges();

            NoticeManager.SetMessage("User removed and informed");
        }


    }
}