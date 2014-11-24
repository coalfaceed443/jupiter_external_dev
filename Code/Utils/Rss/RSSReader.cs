using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace CRM.Code.Utils.Rss
{
    /// <summary>
    /// Utility class for reading and caching RSS feeds
    /// </summary>
    public class RssReader
    {
        private class CachedFeed
        {
            public string Uri {get; set;}
            public DateTime Expiry {get; set;}
            public List<RssItem> RssItems {get; set;}
        }

        private List<CachedFeed> feeds = new List<CachedFeed>();

        private const string CACHEID = "xdfljhfgdfgdfgdl-rssfeeds";
        private const double DEFAULT_CACHE_TIMEOUT = 12.0; // refresh every 12 hours

        /// <summary>
        /// Instantiate RSSReader, initialising the cache if necessary
        /// </summary>
        public RssReader()
	    {
		    // initiate list of feeds from cache, if possible
            try
            {
                feeds = (List<CachedFeed>) HttpContext.Current.Cache[CACHEID];
                if (feeds == null)
                {
                    feeds = new List<CachedFeed>();
                    HttpContext.Current.Cache[CACHEID] = feeds;
                }
                
            }
            catch
            {
                feeds = new List<CachedFeed>();
                HttpContext.Current.Cache[CACHEID] = feeds;
            }
	    }

        /// <summary>
        /// Invalidates all cached RSS data
        /// </summary>
        public void Refresh()
        {
              feeds = new List<CachedFeed>();
              HttpContext.Current.Cache[CACHEID] = feeds;
        }

        /// <summary>
        /// Returns an IEnumerable of RssItem based on a given uri, which
        /// can be an internet URL or a local filesystem/network path 
        /// </summary>
        /// <param name="uri">URI of RSS Feed</param>
        /// <param name="timeout">Timeout, after which 
        /// data will be refreshed</param>
        /// <returns>IEnumerable of RssItem</returns>
        public IEnumerable<RssItem> Read(string uri, double timeout)
        {
            // attempt to fetch from cache

            CachedFeed feed = feeds.FirstOrDefault(p => p.Uri == uri);

            if (feed != null && 
                feed.Expiry > DateTime.UtcNow)
            {
                return feed.RssItems;
            }

            string url = uri;
            using (XmlTextReader reader = new XmlTextReader(url))
            {
                XmlDocument doc = new XmlDocument();

                for (int i=0;i<5; i++)
                {
                    try
                    {
                        doc.Load(reader);
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (i == 4) throw;
                    }
                }

                List<RssItem> rssItems = new List<RssItem>();

                if (doc["rss"] == null)
                {
                    rssItems.Add(new RssItem
                    {
                        Description = "This is unavailable right now... sorry!"
                    });
                    return rssItems;
                }

                XmlNode channel = doc["rss"]["channel"];

                

                foreach (XmlNode n in channel.ChildNodes)
                {
                    if (n.Name == "item")
                    {
                        RssItem rssItem = new RssItem
                        {
                            Description = n["description"].InnerText,
                            Title = n["title"].InnerText,
                            Date = DateTime.Parse(n["pubDate"].InnerText),
                            Link = n["link"].InnerText,
                        };

                        rssItems.Add(rssItem);
                    }
                }

                // store this item in the cache

                if (feed == null)
                {
                    feed = new CachedFeed();
                    feeds.Add(feed);
                }

                feed.Expiry = DateTime.UtcNow.AddHours(timeout);
                feed.RssItems = rssItems;
                feed.Uri = uri;                

                HttpContext.Current.Cache[CACHEID] = feeds;

                return rssItems;
            }
        }

        /// <summary>
        /// Returns an IEnumerable of RssItem based on a given uri, which
        /// can be an internet URL or a local filesystem/network path. The
        /// cache will be reset according to a default timeout value
        /// </summary>
        /// <param name="uri">URI of RSS Feed</param>
        /// <returns>IEnumerable of RssItem</returns>
        public IEnumerable<RssItem> Read(string uri)
        {
            return Read(uri, DEFAULT_CACHE_TIMEOUT);
        }
    }
}
