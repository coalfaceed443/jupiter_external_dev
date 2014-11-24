using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Utils.Enumeration;

namespace CRM.Code.Utils.Rss
{
    /// <summary>
    /// A single RSS article entry class
    /// </summary>
    public class RssItem
    {
        public enum FeedTypes : byte
        {
            [StringValue("Facebook")]
            Facebook = 0,
            [StringValue("Twitter")]
            Twitter = 1,
            [StringValue("Generic Rss")]
            GenericRss = 2
        }

        private string[] stringSeparators = new string[] { "<br/><br/>" };

        public FeedTypes FeedType { get; set; }

        public string TypeStringValue
        {
            get
            {
                return StringEnum.GetStringValue(FeedType);
            }
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get { return Utils.Text.Text.RemoveTags(Description, "img").Split(stringSeparators, StringSplitOptions.None)[0]; } }
        public DateTime Date { get; set; }
        public string Link { get; set; }
        private string _AuthorImage = "/_assets/media/blogauthor/1.jpg";

        public RssItem()
        {

        }

        public RssItem(string title, string description, DateTime date, string link, FeedTypes feedType)
        {
            Title = title.Trim();
            Description = description.Trim();
            Date = date;
            Link = link.Trim();
            FeedType = feedType;
        }
        
        public RssItem(string title, string description, DateTime date, string link, FeedTypes feedType, string imageURL)
        {
            Title = title.Trim();
            Description = description.Trim();
            Date = date;
            Link = link.Trim();
            FeedType = feedType;
            _AuthorImage = imageURL;
        }

        public string AuthorImageURL
        {
            get
            {
                return _AuthorImage;
            }
        }

        public override string ToString()
        {
            return "T:\"" + Title + "\"<br/>D: " + Utils.Text.Text.RemoveTags(Description, "img").Split(stringSeparators, StringSplitOptions.None)[0]
                + "<br/>Date: " + Date.ToString("dd/MM/yyyy") + "<br/>L: " + Link;
        }
    }
}

