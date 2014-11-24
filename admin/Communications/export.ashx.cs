using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Data;
using System.Text;

namespace CRM.admin.Communications
{
    public class Export : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            using (MainDataContext db = new MainDataContext())
            {
                CRM_Communication comm = db.CRM_Communications.Single(s => s.ID.ToString() == context.Request.QueryString["id"]);
                context.Response.Clear();
                context.Response.ClearHeaders();
                context.Response.ClearContent();
                context.Response.AppendHeader("content-length", comm.FileStore.Length.ToString());
                context.Response.ContentType = "text/csv";

                string filename = CRM.Code.Utils.Url.Url.CreateFriendlyUrl(comm.Name) + "-communication-log-export-" + DateTime.UtcNow.ToString("dd-MM-yyyy") + ".csv";
                string format = "attachment; filename=\"{0}\"";

                context.Response.AppendHeader("content-disposition", String.Format(format, filename));
                context.Response.BinaryWrite(comm.FileStore.ToArray());
                context.ApplicationInstance.CompleteRequest();

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