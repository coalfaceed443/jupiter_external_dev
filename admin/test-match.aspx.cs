using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Managers;
using CRM.Code.Interfaces;

namespace CRM.admin
{
    public partial class test_match : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ContactManager man = new ContactManager();
            var items = (from p in man.Contacts
                        where p.PrimaryAddress.Postcode.Length > 0
                         group p by p.PrimaryAddress.Postcode.ToLower().Replace(" ", "") into a
                        where a.Count() > 1                       
                        select new {
                            Key = a.Key,
                            List = a
                        });

            var contacts = items.OrderBy(a => a.Key).SelectMany(a => a.List);

            foreach (IContact contact in contacts)
            {
                Response.Write(contact.PrimaryAddress.UID + " - " + contact.Fullname + "<br/>");
            }

        }
    }

    public class Relationship
    {
        public string Key { get; set; }
        public IContact Contact { get; set; }
    }
}