using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Web.Caching;
using System.Net.Sockets;

namespace CRM.Code.Utils.Rss
{
    public class Flickr
    {
        public string ThumbImage { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }


        /// <summary>
        /// Flickr object containing useful attributes for a flickr feed.  Feeds are cached with a lifetime of 3 hours.
        /// </summary>
        /// <param name="thumbimage">The thumbnail from Flickr (75x75)</param>
        /// <param name="description">Nullable Description</param>
        /// <param name="content">The original image</param>
        /// <param name="link">Link to the image as page</param>
        /// <param name="title">Link to the image title</param>
        public Flickr(string thumbimage, string description, string content, string link, string title)
        {
            ThumbImage = thumbimage;
            Description = description;
            Content = content;
            Link = link;
            Title = title;
        }

        /// <summary>
        /// Returns a Flickr object to display a list of flickr items in the feed, in list form available for use in LINQ
        /// </summary>
        /// <param name="rssfeedURL">The full URL of the flickr RSS feed</param>
        /// <returns>List of Flickr Objects</returns>
        public static List<Flickr> GetFlickrItems(string rssfeedURL)
        {
            try
            {
                XDocument feedXml = XDocument.Load(rssfeedURL);
                XNamespace media = "http://search.yahoo.com/mrss/";

                List<Flickr> flickrItems = new List<Flickr>();

                Cache cache = HttpContext.Current.Cache;
                string cacheKey = "flickr:" + rssfeedURL;

                if (cache[cacheKey] == null)
                {


                    flickrItems = (from item in feedXml.Descendants("item")
                                   where item.Element(media + "description") != null
                                   select new Flickr(

                                       item.Element(media + "thumbnail").Attribute("url").Value,
                                       item.Element(media + "description").Value,
                                       item.Element(media + "content").Attribute("url").Value,
                                       item.Element("link").Value,
                                       item.Element("title").Value
                                       )).ToList();

                    cache.Insert(cacheKey, flickrItems, null, DateTime.Today.AddHours(3), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    flickrItems = (List<Flickr>)HttpContext.Current.Cache[cacheKey];
                }


                return flickrItems;
            }
            catch (SocketException)
            {
                return new List<Flickr>();
            }
        }
    }


}