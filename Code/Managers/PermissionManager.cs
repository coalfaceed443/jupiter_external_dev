using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;
using System.Web.UI;
using CRM.Code.BasePages.Admin;

namespace CRM.Code.Managers
{
    /// <summary>
    /// Summary description for PermissionManager
    /// </summary>
    public class PermissionManager
    {
        public Models.Admin.AllowedSections CurrentSection { get; set; }
        private bool _CanView = false;
        public bool CanView { get { return _CanView; } }
        private bool _CanAdd = false;
        public bool CanAdd { get { return _CanAdd; } }
        private bool _CanUpdate = false;
        public bool CanUpdate { get { return _CanUpdate; } }
        private bool _CanDelete = false;
        public bool CanDelete { get { return _CanDelete; } }

        public PermissionManager(Models.Admin.AllowedSections CurrentSection)
        {
            this.CurrentSection = CurrentSection;
            MainDataContext db = MainDataContext.CurrentContext;
            Page page = HttpContext.Current.Handler as Page;

            _CanView = true;
            _CanAdd = true;
            _CanUpdate = true;
            _CanDelete = true;
            /*
            if (((AdminPage)page).AdminUser.IsMasterAdmin)
            {
                _CanView = true;
                _CanAdd = true;
                _CanUpdate = true;
                _CanDelete = true;
            }
            else
            {
                AdminPermission permission = db.AdminPermissions.SingleOrDefault(a => a.Section == (int)CurrentSection && a.AdminID == ((AdminPage)page).AdminUser.ID);
                if (permission != null)
                {
                    _CanView = true;
                    _CanAdd = permission.IsAdd;
                    _CanUpdate = permission.IsUpdate;
                    _CanDelete = permission.IsDelete;
                }
            }*/

            /* this permission manager is now redundant*/
        }
    }
}