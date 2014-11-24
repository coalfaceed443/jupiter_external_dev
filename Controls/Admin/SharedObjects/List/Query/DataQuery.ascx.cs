using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Managers;
using CRM.Code.Models;
using CRM.Code.BasePages.Admin;
namespace CRM.Controls.Admin.SharedObjects.List.Query
{
    public partial class DataQuery : System.Web.UI.UserControl
    {
        public bool ShowAdd { get; set; }
        public bool LockEdit { get; set; }
        private int ViewID = 1;
        private Type _type;
        public Type Type {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        private const string QueryKey = "AdminDataQuery";
        public int AdminDataQueryID
        {
            get
            {
                if (ViewState[QueryKey] != null)
                    return (int)ViewState[QueryKey];
                else
                    return 0;
            }
            set
            {
                ViewState[QueryKey] = value;
            }
           
        }

        private EventHandler _ReloadContainer;
        public EventHandler ReloadContainer 
        {
            get
            {
                return _ReloadContainer;
            }
            set
            {
                _ReloadContainer = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Type != null)
                    Reload();

                if (LockEdit)
                    mvOptions.Visible = false;

            }

        }

        public void Reset()
        {
            txtFilter.Text = "";
            ddlFilterColumn.SelectedIndex = 0;
            ddlFilter.SelectedIndex = 0;
        }

        public void Populate(AdminDataQueryFilter filter)
        {
            Reload();
            txtFilter.Text = filter.Value;
            ddlFilter.SelectedValue = filter.Operator;
            ddlFilterColumn.SelectedValue = filter.DataFieldName;
            ddlConcat.SelectedValue = filter.Concat;

            hdnID.Value = filter.ID.ToString();
            mvOptions.SetActiveView(viewEdit);
        }


        protected void Reload()
        {
            DataQueryManager DataQueryManager = new DataQueryManager(Type, ViewID);
            var usableFields = DataQueryManager.GetAllFields();

            ddlFilterColumn.DataSource = usableFields;
            ddlFilterColumn.DataBind();

            lnkAdd.Visible = ShowAdd;
        }

        protected void lnkAdd_Click(object sender, EventArgs e)
        {
            using (MainDataContext db = new MainDataContext())
            {
                Save(db, null);
                db.Dispose();
            }

            ReloadContainer.Invoke(sender, e);
        }

        protected void Save(MainDataContext db, string filterID)
        {
            AdminDataQueryFilter filter = GetFilter(db, filterID);

            if (filter == null)
            {
                filter = new AdminDataQueryFilter();
                db.AdminDataQueryFilters.InsertOnSubmit(filter);
                filter.AdminDataQueryID = AdminDataQueryID;
            }
                           
            filter.DataFieldName = ddlFilterColumn.SelectedValue;
            filter.Operator = ddlFilter.SelectedValue;
            filter.Value = txtFilter.Text;                
            filter.Concat = ddlConcat.SelectedValue;
                
            db.SubmitChanges();

        }

        private AdminDataQueryFilter GetFilter(MainDataContext db, string id)
        {
            return db.AdminDataQueryFilters.SingleOrDefault(c => c.ID.ToString() == id);
        }

        public void lnkSave_Click(object sender, EventArgs e)
        {
            using (MainDataContext db = new MainDataContext())
            {       
                Save(db, hdnID.Value);
                db.Dispose();
            }

        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            using (MainDataContext db = new MainDataContext())
            {
                db.AdminDataQueryFilters.DeleteOnSubmit(GetFilter(db, hdnID.Value));
                db.SubmitChanges();
                db.Dispose();
            }

            ((UtilListView)this.Parent.Parent.Parent.NamingContainer).ReloadFilters(sender, e);
        }

    }
}