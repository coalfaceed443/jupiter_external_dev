using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;
using CRM.Code.Utils;
using CRM.Code.Utils.Ordering;

namespace CRM.admin
{
    /// <summary>
    /// Summary description for UpdateTitles
    /// </summary>
    public class UpdateTitles : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            MainDataContext db = new MainDataContext();
            
            foreach (CRM_Person person in db.CRM_Persons)
            {
                CRM_Title title = db.CRM_Titles.FirstOrDefault(f => f.Name.ToLower().Trim() == person.Title.ToLower().Trim());

                if (title == null)
                {
                    title = new CRM_Title()
                    {
                        Name = person.Title,
                        IsArchived = false,
                        IsActive = true,
                        OrderNo = Ordering.GetNextOrderID(db.CRM_Titles.OrderBy(o => o.OrderNo))
                    };

                    db.CRM_Titles.InsertOnSubmit(title);
                    db.SubmitChanges();
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