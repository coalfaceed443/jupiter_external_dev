using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace CRM.Code.Helpers
{
    /// <summary>
    /// Summary description for Vimeo
    /// </summary>
    public class Vimeo
    {
        public Vimeo()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static string GetVimeoIDFromURL(string url)
        {
            if (url.Contains("/"))
            {
                string[] slashItems = url.Split('/');

                string vimeoid = slashItems[slashItems.Length - 1];

                return vimeoid;
            }
            else return String.Empty;
        }


        /// <summary>
        /// http://vimeo.com/api/v2/video/16387161.xml see here for example nodes
        /// </summary>
        /// <param name="nodedesc"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string APIRequest(string nodedesc, string url)
        {
            string vimeoid = GetVimeoIDFromURL(url);

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load("http://vimeo.com/api/v2/video/" + vimeoid + ".xml");

                XmlNode node = doc.SelectSingleNode("videos/video/" + nodedesc + "/text()");
                return node.Value;
            }
            catch
            {
                return "Unable to find data, incorrect Vimeo ID";
            }
        }
    }
}