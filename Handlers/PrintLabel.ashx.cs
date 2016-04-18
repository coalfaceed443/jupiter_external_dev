using CRM.Code.Interfaces;
using CRM.Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CRM.Handlers
{
    /// <summary>
    /// Summary description for PrintLabel
    /// </summary>
    public class PrintLabel : IHttpHandler
    {

        

        public void ProcessRequest(HttpContext context)
        {
            var jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            StringBuilder item = new StringBuilder();
            MainDataContext db = new MainDataContext();

            
            string basepersonid = HttpContext.Current.Request.QueryString["basepersonid"];
            string reference = HttpContext.Current.Request.QueryString["reference"];
            string addressID = HttpContext.Current.Request.QueryString["addressID"];

            CRM_Person person = db.CRM_Persons.SingleOrDefault(s => s.ID.ToString() == HttpContext.Current.Request.QueryString["basepersonid"]);

            List<ILabel> labels = person.Labels;

            ILabel label = labels.FirstOrDefault(f => f.Reference == reference);

            if (!String.IsNullOrEmpty(addressID))
            {
                label = labels.FirstOrDefault(f => f.LabelCRM_AddressID.ToString() == addressID);
            }


            string labelOutput = label.LabelName;

            if (!String.IsNullOrEmpty(label.LabelOrganisation))
            {
                labelOutput += label.LabelOrganisation;
            }

            labelOutput += label.LabelAddress;
            


            jsonSerializer.Serialize(new { Address = labelOutput }, item);
            HttpContext.Current.Response.Write(item.ToString());


        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}