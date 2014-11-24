using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Text;
using System.Net;
using System.IO;
using CRM.Code.Utils.Time;

namespace CRM.Code.Utils.Rss
{
    /// <summary>
    /// Summary description for Facebook
    /// </summary>
    public class Facebook
    {
        public static string CacheName = "opus-facebook";
        public Facebook()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static List<RssItem> GetFaceBookRSS()
        {
            return ProcessRSS("http://www.facebook.com/feeds/page.php?format=rss20&id=63211576481");
        }

        public static List<RssItem> ProcessRSS(string feed)
        {
            if (System.Web.HttpContext.Current.Cache[CacheName] != null)
                return (List<RssItem>)System.Web.HttpContext.Current.Cache[CacheName];

            List<RssItem> feedList = new List<RssItem>();

            string feedURL = feed;

            StringBuilder rssText = new StringBuilder();

            HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(feed);
            myRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.0; en-GB; rv:1.8.1.4) Gecko/20070515 Firefox/2";

            WebResponse myResponse = myRequest.GetResponse();

            Stream rssStream = myResponse.GetResponseStream();
            XmlDocument rssDoc = new XmlDocument();

            try
            {
                rssDoc.Load(rssStream);
            }
            catch
            {
                RssItem dummyFeed = new RssItem()
                {
                    Link = "",
                    Description = "Problem connecting to facebook, please check back later!",
                    Title = "Error"
                };
                feedList.Add(dummyFeed);
                return feedList;
            }

            XmlNodeList rssItems = rssDoc.SelectNodes("rss/channel/item");

            for (int i = 0; i < rssItems.Count; i++)
            {
                // article //

                string heading;
                string description;
                string hyperlink;
                DateTime date = UKTime.Now.AddDays(-1);

                XmlNode rssDetail;

                rssDetail = rssItems.Item(i).SelectSingleNode("title");
                heading = rssDetail.InnerText;

                rssDetail = rssItems.Item(i).SelectSingleNode("description");
                description = rssDetail.InnerText;

                rssDetail = rssItems.Item(i).SelectSingleNode("link");
                hyperlink = rssDetail.InnerText;

                rssDetail = rssItems.Item(i).SelectSingleNode("pubDate");
                try
                {
                    //Date in the format: "Wed, 07 Mar 2012 14:44:00"
                    date = DateTime.Parse(rssDetail.InnerText);
                }
                catch { }

                RssItem newFeed = new RssItem(heading, description, date, hyperlink, RssItem.FeedTypes.Facebook);
                feedList.Add(newFeed);
            }

            int expirationDuration = 6;
            if (!feedList.Any())
            {
                expirationDuration = 1;
            }

            System.Web.HttpContext.Current.Cache.Add(CacheName, feedList, null, DateTime.UtcNow.AddHours(expirationDuration),
                TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Default, null);

            return feedList;
        }
    }
}