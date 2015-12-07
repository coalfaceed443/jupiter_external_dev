using CRM.Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Scheduled
{
    /// <summary>
    /// Summary description for duplicates
    /// </summary>
    public class duplicates : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            MainDataContext db = new MainDataContext();

            var dupes = from p in (from p in db.CRM_Persons
                        where !p.IsArchived
                        select p).ToList()
                        group p by p.Fullname + p.Postcode.Replace(" ", "") into g
                        where g.Count() > 1
                        where !g.All(c => c.WebsiteAccountID == null)
                        select g;

            HttpContext.Current.Response.Write(dupes.Count() + "<br/>");

            foreach (IGrouping<string, CRM_Person> item in dupes)
            {
                HttpContext.Current.Response.Write("===============<br/>");
                HttpContext.Current.Response.Write(item.Key + "<br/>");
                foreach (CRM_Person person in item)
                {
                    HttpContext.Current.Response.Write(person.WebsiteAccountID + " <a href=\"/admin/person/details.aspx?id=" + person.ID + "\" target=\"_blank\">" + person.Fullname + "</a><br/>");
                }
            }

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