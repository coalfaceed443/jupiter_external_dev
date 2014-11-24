using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using createsend_dotnet;
using System.Configuration;

/// <summary>
/// Summary description for CampaignMonitor
/// </summary>

namespace Website.Code.Utils.CampaignMonitor
{
    public class CampaignMonitor
    {
        /*  Christmas 180f7565eedb1292900f8cf21477787f
            Full Database 7cdbfd876c9df12b57ad61f8faa26cee
            Gallery Database 1fc0cdb4233ab51c12ff810978ef9ddf
            Kim Baker fa36f5f907e203c9192da26be8c461bd
            KK Database 91f600629586735b5db3dcb2f7319759
            OA/OU Web Database 92762371ffb7499c1ff50461f5a4a934
            OI 8750929284ec7aa691b9c5e430ac2b44
            Peter Blake 3ae949655c0cdfe8cf869866549869cf
            VIP List 78779704778af23be91f58a77408c082
            Website - Events 4c299b1be538a060a8526a3240ea57d7
            Website - New Art 698c37ea857afefd10f8abfb602d7616
            Website - Sales 600d1f77ff67d95e3d0c614929400c22
        */

        public static string SalesListID = "600d1f77ff67d95e3d0c614929400c22";
        public static string NewArtListID = "698c37ea857afefd10f8abfb602d7616";
        public static string EventsListID = "4c299b1be538a060a8526a3240ea57d7";

        public string CLIENT_ID { get; set; }
        public string API_KEY = Constants.CampaignMonitorAPIKey;
        public static string FROM_NAME = "OPUS Fine Art";
        public static string FROM_EMAIL = "info@opus-art.com";
        public static string CONFIRM_EMAIL = "info@opus-art.com";

        public CampaignMonitor()
        {
            createsend_dotnet.CreateSendOptions.ApiKey = API_KEY;
            CLIENT_ID = "cb729670756493fc8c3b40bd7a399adb";
        }

        private List<SubscriberDetail> GetFullListFromList(PagedCollection<SubscriberDetail> firstPage, List list)
        {
            List<SubscriberDetail> allSubscribers = new List<SubscriberDetail>();

            allSubscribers.AddRange(firstPage.Results);

            if (firstPage.NumberOfPages > 1)
                for (int pageNumber = 2; pageNumber <= firstPage.NumberOfPages; pageNumber++)
                {
                    PagedCollection<SubscriberDetail> subsequentPage = list.Active(new DateTime(1900, 1, 1), pageNumber, 50, "Email", "ASC");
                    allSubscribers.AddRange(subsequentPage.Results);
                }

            return allSubscribers;
        }

        public List<SubscriberDetail> GetActiveSubscribers(string ListID)
        {
            List list = new List(ListID);
          
            try
            {

                //get the first page, with an old date to signify entire list
                PagedCollection<SubscriberDetail> firstPage = list.Active(new DateTime(1900, 1, 1), 1, 50, "Email", "ASC");
                return GetFullListFromList(firstPage, list);
            }
            catch (CreatesendException ex)
            {
                ErrorResult error = (ErrorResult)ex.Data["ErrorResult"];
                HttpContext.Current.Response.Write(error.Code);
                HttpContext.Current.Response.Write(error.Message);
            }
            catch (Exception ex)
            {
                //handle some other failure
            }

            return null;
        }

        /// <summary>
        /// Checks the unsubscribed list for any matches to an email address
        /// </summary>
        /// <param name="email">email to check for</param>
         /// <param name="list">list to check within</param>
        /// <returns>boolean re the email is unsubscribed</returns>
        public bool IsUnsubscribed(string email, string listid)
        {
            List<SubscriberDetail> unsubscriberList = GetUnsubscriberList(listid);

            var subscriber = from p in unsubscriberList
                             where p.EmailAddress == email
                             select p;

            return subscriber.Any();
        }

        public bool IsSubscribed(string email, string listid)
        {
            List<SubscriberDetail> subscriberList = GetActiveSubscribers(listid);

            var subscriber = from p in subscriberList
                             where p.EmailAddress == email
                             select p;

            return subscriber.Any();
        }

        /// <summary>
        /// 
        /// </summary>
        public List<SubscriberDetail> GetUnsubscriberList(string listid)
        {
            List list = new List(listid);
            PagedCollection<SubscriberDetail> firstPage = list.Unsubscribed(new DateTime(1900, 1, 1), 1, 50, "Email", "ASC");
            List<SubscriberDetail> unsubscribers = GetFullListFromList(firstPage, list);
            return unsubscribers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool Unsubscribe(string email, string listID)
        {
            Subscriber subscriber = new Subscriber(listID);
            bool result = false;

            try
            {
                result = subscriber.Unsubscribe(email);
            }
            catch (CreatesendException ex)
            {
                ErrorResult error = (ErrorResult)ex.Data["ErrorResult"];
            }

            return result;
        }


        public static bool Unsubscribe(out string message, string email, string listID)
        {
            createsend_dotnet.CreateSendOptions.ApiKey = Constants.CampaignMonitorAPIKey;
            message = "";

            Subscriber subscriber = new Subscriber(listID);
            bool result = false;

            try
            {
                result = subscriber.Unsubscribe(email);
            }
            catch (CreatesendException ex)
            {
                ErrorResult error = (ErrorResult)ex.Data["ErrorResult"];
                message = error.Code + " || " + error.Message;
            }

            return result;
        }

        /// <summary>
        /// Adds a user to a list
        /// </summary>
        /// <param name="email"></param>
        /// <param name="name"></param>
        /// <param name="resubscribe">Adds the user back into the list if they have already been unsubscribed before</param>
        /// <returns>Success Status</returns>
        public static bool Subscribe(out string message, string email, string name, string listID, bool resubscribe)
        {
            createsend_dotnet.CreateSendOptions.ApiKey = Constants.CampaignMonitorAPIKey;

            message = "";
            bool isSuccess = false;
            Subscriber subscriber = new Subscriber(listID);
            string result = "";

            try
            {
                string newSubscriberID = subscriber.Add(email, name, null, resubscribe);
                isSuccess = true;
            }
            catch (CreatesendException ex)
            {
                ErrorResult error = (ErrorResult)ex.Data["ErrorResult"];
                result = error.Code + " || " + error.Message;
            }
            catch (Exception ex)
            {
                result = "-1 || An internal error occured";
            }

            message = result;
            
            return isSuccess;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BasicList> GetLists()
        {
            Client client = new Client(CLIENT_ID);
            return client.Lists();
        }

        /// <summary>
        /// Retreives basic summary information for a campaign, including the web version of the newsletter, recipients, opens etc.
        /// </summary>
        /// <param name="campaignID">Newsletter/Campaign ID</param>
        public void GetNewsletterSummary(string campaignID)
        {
            Campaign campaign = new Campaign(campaignID);
            campaign.Summary();
        }

        public IEnumerable<CampaignDetail> GetCampaigns()
        {
            Client client = new Client(CLIENT_ID);
            return client.Campaigns();
        }

        public string CreateList(string name, string unsubscribeURL)
        {
            CampaignMonitorAPIWrapper.Result<string> result = CampaignMonitorAPIWrapper.List.Create(API_KEY, CLIENT_ID, name, unsubscribeURL, false, "");
            return result.ReturnObject;
        }
        /*
        public string CreateCampaign(Newsletter newsletter, List<string> lists)
        {
            List<CampaignMonitorAPIWrapper.ListSegment> segments = new List<CampaignMonitorAPIWrapper.ListSegment>();

            CampaignMonitorAPIWrapper.Result<string> result = CampaignMonitorAPIWrapper.Campaign.Create(API_KEY, CLIENT_ID, newsletter.Subject + "_" + newsletter.ID + "_" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm"),
                newsletter.Subject, FROM_NAME, FROM_EMAIL, FROM_EMAIL, Constants.DomainName + newsletter.NewsletterHtmlURL, Constants.DomainName + newsletter.NewsletterTxtURL, lists, segments);

            if (!String.IsNullOrEmpty(result.ReturnObject))
                return result.ReturnObject;
            else
            {
                return result.Message;
            }
        }

        public string SendCampaign(Newsletter newsletter)
        {
            CampaignMonitorAPIWrapper.Result<string> result = CampaignMonitorAPIWrapper.Campaign.Send(API_KEY, newsletter.CampaignID, FROM_EMAIL, UKTime.Now.AddMinutes(1));
            return result.ReturnObject;
        }*/
    }
}