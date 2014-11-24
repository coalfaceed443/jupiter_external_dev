using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Net;
using System.IO;
using System.Text;

namespace CRM.Code.Utils.RemotePost
{
    public class RemotePost
    {
        private ArrayList ItemNames = new ArrayList();
        private ArrayList ItemValues = new ArrayList();

        public string Title = "";
        public string Url = "";
        public string Method = "post";
        public string FormName = "form1";

        public void Add(string name, string value)
        {
            ItemNames.Add(name);
            ItemValues.Add(value);
        }

        public void Post()
        {
            StreamReader f = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("/redirect.htm"));
            StringBuilder template = new StringBuilder(f.ReadToEnd());
            f.Close();

            StringBuilder formSB = new StringBuilder();
            System.Web.HttpContext.Current.Response.Clear();

            template.Replace("@URL@", Url);
            template.Replace("@METHOD@", Method);

            for (int i = 0; i < ItemNames.Count; i++)
            {
                formSB.AppendFormat("<input type='hidden' name='{0}' value='{1}' />\n", ItemNames[i], ItemValues[i]);
            }

            template.Replace("@FORM_ITEMS@", formSB.ToString());

            System.Web.HttpContext.Current.Response.Write(template.ToString());

            System.Web.HttpContext.Current.Response.End();
        }

        public static string PostData(string url, string postData)
        {
            HttpWebRequest request = null;

            Uri uri = new Uri(url + "?" + postData);
            request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";
        
            string result = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader readStream = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        result = readStream.ReadToEnd();
                    }
                }
            }
            return result;
        }
    }
}