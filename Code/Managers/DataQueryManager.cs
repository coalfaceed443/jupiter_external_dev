 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Auth;

namespace CRM.Code.Managers
{
    public class DataQueryManager : IDisposable
    {
        public MainDataContext db;
        private List<ListItem> fields = new List<ListItem>();
        private List<ListItem> partials = new List<ListItem>();
        private List<ListItem> custom = new List<ListItem>();
        public bool IncludeDataReference { get; set; }

        public IEnumerable<ListItem> usableFields;
        private int ViewID { get; set; }
        private Type _type;
        public Type Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }


        public DataQueryManager(Type _Type, int _viewID)
        {
            Type = _Type;
            ViewID = _viewID;
            db = new MainDataContext();

            Initialize(Type, ViewID);
        }

        public _DataTableColumn AddColumn(string field, int AdminID, string friendlyField)
        {
            _DataTableColumn col = new _DataTableColumn()
            {
                _DataFieldName = field,
                _DataTableID = GetDataTable().ID,
                AdminID = AdminID,
                _DataFieldFriendly = friendlyField
            };

            GetDataTable()._DataTableColumns.Add(col);
            db.SubmitChanges();

            return col;
        }

        public void Initialize(Type _Type, int ViewID)
        {
            usableFields = GetAllFields().Where(f => f.Text != "" && !GetSchema().Any(c => c._DataFieldName == f.Value)).OrderBy(a => a.Text);
        }

        public List<ListItem> GetAllFields()
        {
            List<ListItem> allfields = new List<ListItem>();

            allfields = (from s in db.DBML_Schemas
                        where s.table == GetDataTable().TableReference
                        where s.column_desc != null
                        select new ListItem((string)s.column_desc, s.column)).ToList();

            partials = (from pi in Type.GetProperties()
                        where pi.PropertyType.Namespace == "System"
                        where IsListData.GetTypeWithAttribute(Type, pi.Name) != null && IsListData.GetTypeWithAttribute(Type, pi.Name).UseInList
                        select new ListItem(IsListData.GetTypeWithAttribute(Type, pi.Name).FriendlyName, pi.Name)).ToList();

            custom = (from s in db.CRM_FormFields
                      where !s.IsArchived
                        where s._DataTableID == GetDataTable().ID
                        select new ListItem(s.Name, s.ID.ToString())).ToList();

            allfields.AddRange(partials);
            allfields.AddRange(custom);

            return allfields;
            
        }

        public _DataTable GetDataTable()
        {
            return db._DataTables.FirstOrDefault(f => f.TableReference == Type.Name);
        }

        public List<_DataTableColumn> GetSchema()
        {

            if (GetDataTable() == null)
            {
                _DataTable datatable = new _DataTable();
                datatable.TableReference = Type.Name;
                datatable.FriendlyName = Type.Name;
                datatable.IsAllowCustom = false;
                db._DataTables.InsertOnSubmit(datatable);
                db.SubmitChanges();
            }


            int viewID = ViewID;

            if (viewID == -1)
            {
                AuthAdmin auth = new AuthAdmin(db);
                viewID = auth.Authorise().ID;
            }

            List<_DataTableColumn> dtc = (from p in GetDataTable()._DataTableColumns
                                          where p.AdminID == viewID
                                              orderby p.OrderNo
                                              select p).ToList();

            if (IncludeDataReference)
            {
                var fields = GetAllFields();

                _DataTableColumn tempDTC = new _DataTableColumn();
                tempDTC.AdminID = 0;
                tempDTC._DataTableID = 0;
                tempDTC._DataFieldName = "Reference";
                tempDTC._DataFieldFriendly = "Reference";
                tempDTC.OrderNo = 999;
                dtc.Add(tempDTC);
                
            }

            return dtc;

        }

        public void Dispose()
        {
            db.Dispose();
            fields = null;
            partials = null;
            custom = null;
            usableFields = null;
            Type = null;
        }
    }
}