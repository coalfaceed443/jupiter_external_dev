using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Models;
using CRM.Code.BasePages.Admin;
using CRM.Code.Utils.Time;

namespace CRM.Controls.Admin.Navigation
{
    public partial class NavigationHistory : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                using (MainDataContext db = new MainDataContext())
                {
                    AdminPage CurrentPage = (AdminPage)Page;
                    CRM.Code.Models.Admin currentAdmin = CurrentPage.AdminUser;

                    CRM_NavHistory History = new CRM_NavHistory()
                    {
                        FriendlyName = CurrentPage.GetPageTitle().Trim(),
                        AdminID = currentAdmin.ID,
                        LastAccessed = UKTime.Now,
                        URL = Request.RawUrl,
                        ContextName = CurrentPage.CRMContext != null ? CurrentPage.CRMContext.DisplayName : ""
                    };

                    db.CRM_NavHistories.InsertOnSubmit(History);
                    db.SubmitChanges();


                    var oldHistories = db.CRM_NavHistories.Where(a => a.AdminID == currentAdmin.ID).OrderByDescending(o => o.LastAccessed).Skip(8);
                    db.CRM_NavHistories.DeleteAllOnSubmit(oldHistories);
                    db.SubmitChanges();


                    rptHistory.DataSource = currentAdmin.CRM_NavHistories.OrderByDescending(o => o.LastAccessed).Take(8);
                    rptHistory.DataBind();

                    db.Dispose();
                }
            }
        }
    }
}