using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using CRM.Code.Models;

namespace CRM.Controls.Admin
{
    /// <summary>
    /// Summary description for CaptureImageStream
    /// </summary>
    public class CaptureImageStream : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            string contents = String.Empty;

            HttpContext.Current.Request.InputStream.Position = 0;
            using (StreamReader inputStream = new StreamReader(HttpContext.Current.Request.InputStream))
            {
                contents = inputStream.ReadToEnd();
            }

            var bytes = Convert.FromBase64String(contents);

            string tempFile = "";

            using (MainDataContext db = new MainDataContext())
            {
                Code.Models.Admin adminUser = db.Admins.Single(a => a.ID.ToString() == context.Request.QueryString["admin"]);
                tempFile = adminUser.TempPhotoFile;
                db.Dispose();
            }

            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(bytes);
                    stream.Seek(0, SeekOrigin.Begin);
                    FileStream file = new FileStream(context.Server.MapPath(tempFile), FileMode.Create, System.IO.FileAccess.Write);
                    stream.Read(bytes, 0, (int)stream.Length);
                    file.Write(bytes, 0, bytes.Length);
                    file.Close();
                    stream.Close();
                }
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
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