using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Helpers;
using CRM.Code.Models;
using CRM.Code.BasePages.Admin;
using System.Reflection;
using CRM.Code.Interfaces;
using CRM.Code.Utils.Ordering;
using CRM.Code;
using System.Data;
using CRM.Code.Managers;
using CRM.Code.Utils.Time;
using CRM.Controls.Admin.SharedObjects.List.Query;
using System.Linq.Dynamic;
using System.Collections;
using CRM.Code.Auth;
namespace CRM.Controls.Admin.SharedObjects.List
{

    public partial class UtilListView : System.Web.UI.UserControl
    {
        private int _Width = 950;
        public int Width
        {
            get
            {
                return _Width;
            }
            set
            {
                _Width = value;
            }
        }

        protected global::CRM.Controls.Admin.SharedObjects.List.Query.DataQuery ucDataQuery;
        protected DataQueryManager DQManager;
        protected List<_DataTableColumn> DataTableSchema;
        protected MainDataContext db;
        public Type Type { get; set; }
        public IEnumerable<object> DataSet { get; set; }

        public bool ParsedViaQuery { get; set; }
        public bool ShowExport
        {
            get
            {
                return _ShowExport;
            }
            set
            {
                _ShowExport = value;
            }
        }
        private int _Columns = 10;
        protected int Columns
        {
            get
            {
                return _Columns;
            }
            set
            {
                _Columns = value;
            }
        }
        protected bool IsDataEmpty = false;
        protected bool _CanOrder = false;
        protected bool _ShowExport = true;
        public bool ShowCustomisation = true;
        public bool CanOrder
        {
            get
            {
                return _CanOrder;
            }
            set
            {
                _CanOrder = value;
            }
        }

        private int _ItemsPerPage = 40;
        public int ItemsPerPage
        {
            get
            {
                return _ItemsPerPage;
            }
            set
            {
                _ItemsPerPage = value;
            }
        }


        public string SessionID
        {
            get
            {
                return this.ClientID + "PAGE";
            }
        }

        public string SessionView
        {
            get
            {
                return this.ClientID + "View";
            }
        }
        private int DefaultviewID = 1;
        protected int ViewID { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                if (Session[SessionView] != null)
                {
                    ddlChangeView.SelectedValue = (string)Session[SessionView];
                }

                if (ddlChangeView.SelectedValue == "-1")
                    ViewID = ((AdminPage)Page).AdminUser.ID;
                else
                    ViewID = DefaultviewID;
            }
            else
            {
                ViewID = Convert.ToInt32(ddlChangeView.SelectedValue);
            }
            
            DQManager = new DataQueryManager(Type, ViewID);

            ddlChangeView.Visible = ShowCustomisation;
            ddlFields.Visible = ShowCustomisation;

            pnlQueryManager.Visible = ShowCustomisation;

            db = ((AdminPage)Page).db;

            btnExport.EventHandler = btnExport_Click;
            btnExport.Visible = _ShowExport;

            ucDataQuery.Type = Type;
            ucDataQuery.ReloadContainer = ReloadFilters;
            if (!Page.IsPostBack && Type != null)
            {
                rbMailOut.Checked = Request.QueryString["mailout"] == Boolean.TrueString;
                rbEmail.Checked = Request.QueryString["emailing"] == Boolean.TrueString;
                chkGroupByRelationship.Checked = Request.QueryString["groupbyrel"] == Boolean.TrueString;
                if (!String.IsNullOrEmpty(Request.QueryString["query"]))
                {
                    RePopulateQueriesList();
                    var query = Request.QueryString["query"];
                    ddlExistingQueries.SelectedValue = Request.QueryString["query"];
                    UpdateCurrentQuery(Request.QueryString["query"]);
                }

                ReloadFilters(sender, e);
                LoadView();

                RePopulateQueriesList();



            }
            this.Visible = Type != null;

            pnlCustomQuery.Visible = ParsedViaQuery;

           // pnlCustomQuery.Visible = ((AdminPage)Page).AdminUser.IsMasterAdmin;
        }
      
        public void Reinitialize()
        {
            DQManager = new DataQueryManager(Type, ViewID);
            LoadView();
            SetPageColSpan();
            this.Visible = true;
        }


        public void ReloadFilters(object sender, EventArgs e)
        {
            var filters = (from q in db.AdminDataQueryFilters
                           where q.AdminDataQuery._DataTableID == DQManager.GetDataTable().ID
                           where q.AdminDataQueryID.ToString() == ddlExistingQueries.SelectedValue
                           select q);

            rptQuery.DataSource = filters;
            rptQuery.DataBind();
                   
        }


        private void SetPageColSpan()
        {
            tdPage.Attributes["colspan"] = (Columns + (CanOrder ? 1 : 0)).ToString();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {

            DQManager = new DataQueryManager(Type, ViewID);
            DQManager.IncludeDataReference = rbMailOut.Checked || rbEmail.Checked;
            CSVExport.GenericExport(DQManager.GetSchema(), FilterDownForMail(FormData()), Response, DQManager.GetDataTable().TableReference);
        }

        protected void LoadView(AdminDataQuery query)
        {
            DQManager = new DataQueryManager(Type, ViewID);
            DataTableSchema = DQManager.GetSchema();

            ddlFields.Items.Clear();
            ddlFields.Items.Add(new ListItem("Add a new Column", String.Empty));

            ddlFields.DataSource = DQManager.usableFields;
            ddlFields.DataBind();

            Columns = DQManager.GetSchema().Count();

            dpMain.PageSize = _ItemsPerPage;
            SetPageColSpan();

            rptHeader.DataSource = DataTableSchema;
            rptHeader.DataBind();

            if (DataSet != null)
            {
                var items = (from p in DataSet
                            select p).ToArray();

                if (items.Any() && !(items.First() is ListOrderItem))
                {
                    CanOrder = false;
                }


                IEnumerable<ListData> data = FormData();

                data = FilterDownForMail(data);

                IsDataEmpty = !data.Any();
                
                lvItems.DataSource = data.ToList();
                lvItems.DataBind();
            }
        }

        protected void LoadView()
        {
            if (ddlChangeView.SelectedValue == "-1")
                ViewID = ((AdminPage)Page).AdminUser.ID;
            else
                ViewID = DefaultviewID;

            Session[SessionView] = ViewID.ToString();

            LoadView(null); 
        }

        protected void lvItems_ItemBound(object sender, ListViewItemEventArgs e)
        {

            if (e.Item is ListViewDataItem)
            {
                Repeater rptFields = (Repeater)e.Item.FindControl("rptFields");

                ListData item = (ListData)((ListViewDataItem)e.Item).DataItem;

                List<ListDataItem> dataobject = new List<ListDataItem>();

                foreach (_DataTableColumn col in DataTableSchema)
                {
                    dataobject.Add(new ListDataItem(col._DataFieldName, item.DataItem));
                }


                // an array of properties (datatablescheme) with the object attached to each property
                rptFields.DataSource = dataobject;
                rptFields.DataBind();

                
            }
        }

        protected void ddlFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFields.SelectedValue != "")
            {
                _DataTableColumn col = DQManager.AddColumn(ddlFields.SelectedValue, ((AdminPage)Page).AdminUser.ID, ddlFields.SelectedItem.Text);

                DQManager.GetDataTable()._DataTableColumns.Add(col);
                DQManager.db.SubmitChanges();

                Ordering.MoveToBottom(DQManager.GetSchema(), (_DataTableColumn)col);
                 DQManager.db.SubmitChanges();
                DQManager.Initialize(Type, ViewID);
            }
            LoadView();
        }

        protected void ddlChangeView_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session[SessionView] = ddlChangeView.SelectedValue;

            LoadView();
        }

        protected void lvItems_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            dpMain.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);

            Session[SessionID] = e.StartRowIndex;

            LoadView();
        }


        protected void btnMoveUp_Click(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            reorderItem(button, false);
        }

        protected void btnMoveTop_Click(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            var entities = FormData();
            object record = DataSet.SingleOrDefault(a => new ListData(a).ID == Int32.Parse(button.CommandArgument));
            Ordering.MoveToTop(entities, (CRM.Code.Utils.Ordering.ListOrderItem)record);
            db.SubmitChanges();

            LoadView();
        }

        protected void btnMoveBottom_Click(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            var entities = FormData();
            object record = DataSet.SingleOrDefault(a => new ListData(a).ID == Int32.Parse(button.CommandArgument));
            Ordering.MoveToBottom(entities, (CRM.Code.Utils.Ordering.ListOrderItem)record);
            db.SubmitChanges();

            LoadView();
        }

        protected void btnMoveDown_Click(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            reorderItem(button, true);
        }

        private IEnumerable<ListData> FormData()
        {

            dpMain.Visible = DataSet.Any();
            IsDataEmpty = !DataSet.Any();
            SetPageColSpan();

            IEnumerable<ListData> data = Enumerable.Empty<ListData>();

            if (CanOrder)
            {
                data = DataSet.OrderBy(d => ((CRM.Code.Utils.Ordering.ListOrderItem)d).OrderNo).Select(a => new ListData(a));
            }
            else
                data = DataSet.Select(a => new ListData(a));

            return data;

        }

        protected IEnumerable<ListData> FilterDownForMail(IEnumerable<ListData> data)
        {
            if (rbEmail.Checked)
            {
                data = from c in data
                       where c.IsEmailable
                       select c;
            }

            if (rbMailOut.Checked)
            {
                data = from c in data
                       where c.IsMailable
                       select c;

                if (typeof(IContact).IsAssignableFrom(Type))
                {
                    data = data.OrderBy(d => ((IContact)d.DataItem).PrimaryAddress.Postcode);
                }
            }

            if (chkGroupByRelationship.Checked)
            {
                IEnumerable<IGrouping<int?, ListData>> groupset = data.GroupBy(g => g.RelationshipID);

                var recordsWithRelation = groupset.Where(c => c.Key != null).Select(c => c.First());
                var recordsWithoutRelation = groupset.Where(c => c.Key == null).SelectMany(c => c);
                data = recordsWithRelation.Concat(recordsWithoutRelation);
            }

            return data;
        }

        private void reorderItem(ImageButton button, bool increase)
        {
            var entities = FormData();
            object record = DataSet.SingleOrDefault(a => new ListData(a).ID == Int32.Parse(button.CommandArgument));
            Ordering.ChangeOrder(entities, (CRM.Code.Utils.Ordering.ListOrderItem)record, increase);
            db.SubmitChanges();

            LoadView();
        }


        private void reorderHeader(ImageButton button, bool increase)
        {
            int admin = 1;
            if (ViewID == -1)
            {
                admin = new AuthAdmin(db).Authorise().ID;
            }

            var entities = DQManager.GetDataTable()._DataTableColumns.Where(d => d.AdminID == admin);
            _DataTableColumn record = DQManager.GetDataTable()._DataTableColumns.SingleOrDefault(a => a.ID == Int32.Parse(button.CommandArgument));
            Ordering.ChangeOrder(entities, record, increase);
            DQManager.db.SubmitChanges();

            LoadView();
        }

        protected void rptHeader_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            LinkButton lnkRemove = (LinkButton)e.Item.FindControl("lnkRemove");
            lnkRemove.CommandArgument = ((_DataTableColumn)e.Item.DataItem).ID.ToString();
        }

        protected void btnMoveLeft_Click(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            reorderHeader(button, false);
        }

        protected void btnMoveRight_Click(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            reorderHeader(button, true);
        }

        protected void lnkRemove_Click(object sender, EventArgs e)
        {
            _DataTableColumn dtc = DQManager.db._DataTableColumns.SingleOrDefault(d => d.ID.ToString() == ((LinkButton)sender).CommandArgument);

            if (dtc != null)
            {
                DQManager.db._DataTableColumns.DeleteOnSubmit(dtc);
                DQManager.db.SubmitChanges();
            }

            LoadView();
        }

        protected void ddlExistingQueries_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCurrentQuery();            
        }

        protected void RePopulateQueriesList()
        {
            ddlExistingQueries.DataSource = from q in (from a in db.AdminDataQueries
                                                       where a._DataTableID == DQManager.GetDataTable().ID
                                            where a.AdminID == ((AdminPage)Page).AdminUser.ID || a.IsPublic
                                            select a).ToArray()
                                            select new ListItem(q.Name + " [" + q.Admin.DisplayName + "]", q.ID.ToString());

            ddlExistingQueries.DataBind();
        }

        protected void UpdateCurrentQuery(string forceQuery)
        {
            AdminDataQuery adq = db.AdminDataQueries.SingleOrDefault(s => s.ID.ToString() == forceQuery);
               
            if (forceQuery != "")
            {
                if (((AdminPage)Page).AdminUser.ID == adq.AdminID)
                {
                    mvManageQuery.SetActiveView(viewDelete);
                    lnkDeleteQuery.Visible = true;
                }
                else
                    lnkDeleteQuery.Visible = false;

                ReloadFilters(null, null);

                pnlManageQuery.Visible = true;

                if (adq != null)
                {
                    ucDataQuery.AdminDataQueryID = adq.ID;
                }
                lnkRunQuery.Visible = true;
            }
            else
            {
                mvManageQuery.SetActiveView(viewAdd);
                ReloadFilters(null, null); 
                pnlManageQuery.Visible = false;
            }
        }

        protected void UpdateCurrentQuery()
        {
            UpdateCurrentQuery(ddlExistingQueries.SelectedValue);
        }

        protected void lnkAdd_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (txtName.Text.Length > 0)
                {
                    AdminDataQuery query = new AdminDataQuery()
                    {
                        _DataTableID = DQManager.GetDataTable().ID,
                        AdminID = ((AdminPage)Page).AdminUser.ID,
                        IsPublic = chkIsPublic.Checked,
                        Name = txtName.Text,
                        Timestamp = UKTime.Now,
                    };

                    db.AdminDataQueries.InsertOnSubmit(query);
                    db.SubmitChanges();

                    RePopulateQueriesList();
                    ddlExistingQueries.SelectedValue = query.ID.ToString();
                    ReloadFilters(sender, e);
                    pnlManageQuery.Visible = true;

                    ucDataQuery.Reset();
                    UpdateCurrentQuery();
                }
            }
        }

        protected void rptQuery_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            AdminDataQueryFilter adq = (AdminDataQueryFilter)e.Item.DataItem;
            DataQuery ucDataQuery = (DataQuery)e.Item.FindControl("ucDataQuery");
            ucDataQuery.Type = Type;
            ucDataQuery.LockEdit = false;
            ucDataQuery.Populate(adq);
        }

        protected void lnkRunQuery_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rptQuery.Items)
            {
                DataQuery dq = (DataQuery)item.FindControl("ucDataQuery");
                dq.lnkSave_Click(sender, e);
            }
            var query = HttpUtility.ParseQueryString(Request.Url.Query);

            if (query["query"] != null)
            {
                query.Remove("query");
                query.Remove("data");
                query.Remove("mailout");
                query.Remove("emailing");
                query.Remove("groupbyrel");
            }

            string redirect = Request.Url.AbsolutePath + (query.Count == 0 ? "?" : "?" + query + "&") + "query=" + ddlExistingQueries.SelectedValue;
            redirect += "&data=" + rbNone.Checked + "&mailout=" + rbMailOut.Checked + "&emailing=" + rbEmail.Checked + "&groupbyrel=" + chkGroupByRelationship.Checked;

    
            NoticeManager.SetMessage("Query Complete", redirect);
        }

        protected void lnkDeleteQuery_Click(object sender, EventArgs e)
        {
            AdminDataQuery query = db.AdminDataQueries.Single(c => c.ID.ToString() == ddlExistingQueries.SelectedValue);

            db.AdminDataQueryFilters.DeleteAllOnSubmit(query.AdminDataQueryFilters);
            db.AdminDataQueries.DeleteOnSubmit(query);
            db.SubmitChanges();
            ddlExistingQueries.SelectedValue = "";
            RePopulateQueriesList();

            Response.Redirect("list.aspx");
        }

    }
}