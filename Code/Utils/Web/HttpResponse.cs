using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Web
{
    public static class HttpResponseExtension
    {
        /// <summary>
        /// Redirects the request to a specified URL and returning the supplied code in the response.
        /// If a 3xx code is entered a 'Location' header is added but if 4xx is added a Server.Transfer is used.
        /// </summary>
        /// <param name="redirectURL">The URL to redirect to. If using 400 codes use the aspx URL and not a friendly URL</param>
        /// <param name="statusCode">The code to return in the response</param>
        public static void Redirect(this HttpResponse source, string redirectURL, int statusCode)
        {
            source.Clear();
            source.Status = "404 Not Found";

            switch (statusCode)
            {
                /** 300's **/
                case 300:
                    source.Status = "300 Multiple Choices";
                    break;
                case 301:
                    source.Status = "301 Moved Permanently";
                    break;
                case 302:
                    source.Status = "302 Found";
                    break;
                case 303:
                    source.Status = "303 See Other";
                    break;
                case 304:
                    source.Status = "304 Not Modified";
                    break;
                case 305:
                    source.Status = "305 Use Proxy";
                    break;
                case 306:
                    source.Status = "306 Switch Proxy";
                    break;
                case 307:
                    source.Status = "307 Temporary Redirect";
                    break;
                case 308:
                    source.Status = "308 Resume Incomplete";
                    break;

                /** 400's **/
                case 400:
                    source.Status = "400 Bad Request";
                    break;
                case 401:
                    source.Status = "401 Unauthorized";
                    break;
                case 402:
                    source.Status = "402 Payment Required";
                    break;
                case 403:
                    source.Status = "403 Forbidden";
                    break;
                case 404:
                    source.Status = "404 Not Found";
                    break;
                case 405:
                    source.Status = "405 Method Not Allowed";
                    break;
                case 406:
                    source.Status = "406 Not Acceptable";
                    break;
                case 407:
                    source.Status = "407 Proxy Authentication Required";
                    break;
                case 408:
                    source.Status = "408 Request Timeout";
                    break;
                case 409:
                    source.Status = "409 Conflict";
                    break;
                case 410:
                    source.Status = "410 Gone";
                    break;
                case 411:
                    source.Status = "411 Length Required";
                    break;
                case 412:
                    source.Status = "412 Precondition Failed";
                    break;
                case 413:
                    source.Status = "413 Request Entity Too Large";
                    break;
                case 414:
                    source.Status = "414 Request-URI Too Long";
                    break;
                case 415:
                    source.Status = "415 Unsupported Media Type";
                    break;
                case 416:
                    source.Status = "416 Requested Range Not Satisfiable";
                    break;
                case 417:
                    source.Status = "417 Expectation Failed";
                    break;
                case 418:
                    source.Status = "418 I'm a teapot";
                    break;
                case 420:
                    source.Status = "420 Enhance Your Calm";
                    break;
                case 422:
                    source.Status = "422 Unprocessable Entity";
                    break;
                case 423:
                    source.Status = "423 Locked";
                    break;
                case 424:
                    source.Status = "424 Failed Dependency";
                    break;
                case 425:
                    source.Status = "425 Unordered Collection";
                    break;
                case 426:
                    source.Status = "426 Upgrade Required";
                    break;
                case 428:
                    source.Status = "428 Precondition Required";
                    break;
                case 429:
                    source.Status = "429 Too Many Requests";
                    break;
                case 431:
                    source.Status = "431 Request Header Fields Too Large";
                    break;
                case 444:
                    source.Status = "444 No Response";
                    break;
                case 449:
                    source.Status = "449 Retry With";
                    break;
                case 450:
                    source.Status = "450 Blocked by Windows Parental Controls";
                    break;
                case 499:
                    source.Status = "499 Client Closed Request";
                    break;
            }

            source.StatusCode = statusCode;

            //If we have a 300 status code then add the header as that will redirect
            if (statusCode >= 300 && statusCode < 400)
                source.AddHeader("Location", redirectURL);
            //But if we are in 400 codes then do a server transfer which will return 4xx to the browser but show the page if it is a real user
            else if (statusCode >= 400 && statusCode < 500)
                HttpContext.Current.Server.Transfer(redirectURL);
            source.End();
        }
    }
}