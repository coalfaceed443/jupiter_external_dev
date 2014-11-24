using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using CRM.Code.Models;

namespace CRM.admin
{
    /// <summary>
    /// Summary description for CollectStructure
    /// </summary>
    public class CollectStructure : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            DirSearch("/admin");
        }

        static void DirSearch(string dir)
        {
            string virtualP = HttpContext.Current.Server.MapPath(dir);

            foreach (string d in Directory.GetDirectories(virtualP))
            {
                foreach (string f in Directory.GetFiles(d))
                {
                    if (!f.Contains(".cs") && !f.Contains(".ashx"))
                    {
                        string URL = f.Replace("D:\\coalface\\7stories-2012-07-03\\Trunk\\CRM\\admin", "");
                        string Friendly = URL.Replace("\\", " - ").Replace(".aspx", ""); // strip file parameters
                        Friendly = Regex.Replace(Friendly, "(\\B[A-Z])", " $1"); // replace CamelCase with spaces
                        Friendly = Friendly.Substring(3, Friendly.Length - 3); // strip initial root directory \

                        using (MainDataContext db = new MainDataContext())
                        {
                            CRM_SystemAccess access = db.CRM_SystemAccesses.SingleOrDefault(s => s.Path == URL);

                            if (access == null)
                            {
                                int type = Friendly.Contains("List") ? 0 : 1;

                                access = new CRM_SystemAccess()
                                {
                                    FriendlyName = Friendly,
                                    IsPermissable = true,
                                    Path = URL,
                                    Type = (byte)type 
                                };

                                db.CRM_SystemAccesses.InsertOnSubmit(access);
                                db.SubmitChanges();
                            }
                        }

                        HttpContext.Current.Response.Write(URL + ":" + Regex.Replace(Friendly, "(\\B[A-Z])", " $1") + "<br/>");
                    }
                }
                DirSearch(d.Replace("D:\\coalface\\7stories-2012-07-03\\Trunk\\CRM", ""));
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