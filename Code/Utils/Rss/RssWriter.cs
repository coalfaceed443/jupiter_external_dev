using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;
using CRM.Code.Models;

namespace CRM.Code.Utils.Rss
{
    public class RssWriter
    {
        protected MainDataContext db;
        protected string rssPath, description;
        protected List<RssItem> rssList = new List<RssItem>();

        public RssWriter(MainDataContext db, string rssPath, string description)
        {
            this.db = db;
            this.rssPath = rssPath;
            this.description = description;
        }

        public void AddItem(RssItem item)
        {
            rssList.Add(item);
        }

        public void GenerateRSS()
        {
            TextWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath(rssPath));

            FormHeader(sw, description);

            FormItems(sw, rssList);

            sw.WriteLine("</channel>");
            sw.WriteLine("</rss>");

            sw.Close();
        }

        protected static TextWriter FormHeader(TextWriter sw, string description)
        {
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");

            sw.WriteLine("<?xml-stylesheet type=\"text/xsl\" href=\"http://blogs.msdn.com/utility/FeedStylesheets/rss.xsl\" media=\"screen\"?>");
            sw.WriteLine("<rss version=\"2.0\" xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:slash=\"http://purl.org/rss/1.0/modules/slash/\" xmlns:wfw=\"http://wellformedweb.org/CommentAPI/\">");
            sw.WriteLine("<channel>");

            sw.WriteLine("<title>" + Constants.WebsiteName + "</title>");
            sw.WriteLine("<link>" + Constants.DomainName + "</link>");
            sw.WriteLine("<description>" + description + "</description>");
            sw.WriteLine("<dc:language>en</dc:language>");

            return sw;
        }

        protected static TextWriter FormItems(TextWriter sw, List<RssItem> RssList)
        {
            foreach (RssItem item in RssList)
            {
                sw.WriteLine("<item>");
                sw.WriteLine("<title>" + HttpUtility.HtmlEncode(item.Title).Replace("&ndash;", "-")
                            .Replace("&nbsp;", " ").Replace("&rsquo;", "&#39;")
                            .Replace("&lsquo;", "&#39;").Replace("&amp;", "&#38;") + "</title>");
                sw.WriteLine("<link>" + item.Link.Replace("//", "/") + "</link>");
                sw.WriteLine("<description>" + HttpUtility.HtmlEncode(item.Description).Replace("&ndash;", "-")
                            .Replace("&nbsp;", " ").Replace("&rsquo;", "&#39;")
                            .Replace("&lsquo;", "&#39;").Replace("&amp;", "&#38;") + "</description>");
                sw.WriteLine("<pubDate>" + item.Date.ToString("dd-MM-yyyy") + "</pubDate>");
                sw.WriteLine("</item>");
            }

            return sw;
        }
    }
}
