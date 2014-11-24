using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Models;
using CRM.Code.Helpers;
using CRM.Code.Interfaces;
using CRM.Code.BasePages.Admin;
using System.Web.Script.Serialization;

namespace CRM.admin.Duplicate
{
    public partial class List : AdminPage
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Type Type = Type.GetType(Request.QueryString["namespace"]);
            object myObject = Activator.CreateInstance(Type);

            MainDataContext db = new MainDataContext();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            object[] comparibles = (object[])serializer.DeserializeObject(Request.QueryString["compare"]);

            var collection = comparibles.Select(c => c);

            var items = from p in ((IDuplicate)myObject).GetBaseSet(db)
                        where p.ID.ToString() != Request.QueryString["id"]
                        select (object)p;


            ucList.Type = Type;

            try
            {

                //todo:compare both dictionaries against eachother.  Any and Count are calling same query twice so should be arranged into one object
                ucList.DataSet = from p in CRM_Person.BaseSet(db)
                                 where p.SearchDictionary.Any(c => (int)((Array)comparibles[c.Key]).Length > 0 && ((Array)comparibles[(int)((Array)comparibles[c.Key]).GetValue(0)]) != null && c.Value.ToLower().Trim() == ((string)((Array)comparibles[(int)((Array)comparibles[c.Key]).GetValue(0)]).GetValue(1)).ToLower().Trim() && c.Value != "")
                                 where p.ID.ToString() != Request.QueryString["id"]
                                 orderby p.SearchDictionary.Count(c => (int)((Array)comparibles[c.Key]).Length > 0 && ((Array)comparibles[(int)((Array)comparibles[c.Key]).GetValue(0)]) != null && c.Value.ToLower().Trim() == ((string)((Array)comparibles[(int)((Array)comparibles[c.Key]).GetValue(0)]).GetValue(1)).ToLower().Trim() && c.Value != "") descending
                                 select (object)p;

            }
            catch { }

            ucList.ItemsPerPage = 10000;
            ucList.ShowCustomisation = false;
            ucList.ShowExport = false;
        }

    }
}