using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using CRM.Code.Utils.Time;

namespace CRM.Code.Utils.Rss
{
    public class Twitter
    {
        public static string CacheName = "opus-twitter";

        public static List<RssItem> GetTwitterRSS()
        {
            if (System.Web.HttpContext.Current.Cache[CacheName] != null)
                return (List<RssItem>)System.Web.HttpContext.Current.Cache[CacheName];

            List<RssItem> returnList = ProcessRSS("http://api.twitter.com/1/statuses/user_timeline.xml?screen_name=opusart");


            int expirationDuration = 6;
            if (!returnList.Any())
            {
                expirationDuration = 1;
            }

            System.Web.HttpContext.Current.Cache.Add(CacheName, returnList, null, DateTime.UtcNow.AddHours(expirationDuration),
                TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Default, null);

            return returnList;
        }

        public static List<RssItem> ProcessRSS(string feed)
        {
            List<RssItem> feedList = new List<RssItem>();

            string feedURL = feed;

            StringBuilder rssText = new StringBuilder();

            HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(feed);
            myRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.0; en-GB; rv:1.8.1.4) Gecko/20070515 Firefox/2";

            WebResponse myResponse = myRequest.GetResponse();

            Stream rssStream = myResponse.GetResponseStream();
            XmlDocument rssDoc = new XmlDocument();
            string twitterUsername = feed.Substring(feed.IndexOf("screen_name=") + "screen_name=".Length);
            try
            {
                rssDoc.Load(rssStream);
            }
            catch
            {
                RssItem dummyFeed = new RssItem()
                {
                    Link = "https://twitter.com/#!/" + twitterUsername,
                    Description = "Problem connecting to Twitter, please check back later!",
                    Title = "Error"
                };
                feedList.Add(dummyFeed);
                return feedList;
            }

            XmlNodeList rssItems = rssDoc.SelectNodes("statuses/status");

            for (int i = 0; i < rssItems.Count; i++)
            {
                // article //

                string heading;
                string description;
                string hyperlink;
                DateTime date = UKTime.Now.AddDays(-1);

                XmlNode rssDetail;

                rssDetail = rssItems.Item(i).SelectSingleNode("id");
                heading = rssDetail.InnerText;

                rssDetail = rssItems.Item(i).SelectSingleNode("text");
                description = rssDetail.InnerText;

                //rssDetail = rssItems.Item(i).SelectSingleNode("link");
                hyperlink = "https://twitter.com/#!/" + twitterUsername;

                rssDetail = rssItems.Item(i).SelectSingleNode("created_at");
                try
                {
                    //Date in the format: "Wed Mar 07 16:51:31 +0000 2012"
                    date = ParseTwitterDate(rssDetail.InnerText);
                }
                catch { }
                RssItem newFeed = null;

                newFeed = new RssItem(heading, Twitter.Format(description), date, hyperlink, RssItem.FeedTypes.Twitter);

                feedList.Add(newFeed);
            }


            return feedList;
        }

        public static string Format(string data)
        {
            string trans = data;

            Regex re = new Regex("http://(.+?)\\s");
            trans = re.Replace(trans, "<a href=\"http://$1\" target=\"_blank\">http://$1</a> ");

            re = new Regex("http://(.+?)$");
            trans = re.Replace(trans, "<a href=\"http://$1\" target=\"_blank\">http://$1</a> ");

            re = new Regex("@(.+?)\\b");
            trans = re.Replace(trans, "<a href=\"http://www.twitter.com/$1\" target=\"_blank\">@$1</a> ");

            re = new Regex("#(.+?)\\b");
            trans = re.Replace(trans, "<a href=\"http://www.twitter.com/$1\" target=\"_blank\">#$1</a> ");

            return trans;
        }

        public static DateTime ParseTwitterDate(string date)
        {
            int day = 1;
            int month = 1;
            int year = 2012;

            int hour = 0;
            int minute = 0;
            int second = 0;

            //Twitter sends the date across in the format "Wed Mar 07 16:51:31 +0000 2012"
            //                                             012345678901234567890123456789
            //                                                       1         2
            day = Int32.Parse(date.Substring(8, 2));

            string monthStr = date.Substring(4, 3);
            switch (monthStr.ToLower())
            {
                case "jan":
                    month = 1;
                    break;
                case "feb":
                    month = 2;
                    break;
                case "mar":
                    month = 3;
                    break;
                case "apr":
                    month = 4;
                    break;
                case "may":
                    month = 5;
                    break;
                case "jun":
                    month = 6;
                    break;
                case "jul":
                    month = 7;
                    break;
                case "aug":
                    month = 8;
                    break;
                case "sep":
                    month = 9;
                    break;
                case "oct":
                    month = 10;
                    break;
                case "nov":
                    month = 11;
                    break;
                case "dec":
                    month = 12;
                    break;
            }
            year = Int32.Parse(date.Substring(26, 4));

            hour = Int32.Parse(date.Substring(11, 2));
            minute = Int32.Parse(date.Substring(14, 2));
            second = Int32.Parse(date.Substring(17, 2));

            return new DateTime(year, month, day, hour, minute, second);
        }
    }
}