using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.IO;
using System.Text;
using CRM.Code.BasePages.Admin;
using System.Web.UI;

namespace CRM.Code.Managers
{
    public class NoticeManager
    {
        public static void SetMessage(string message, bool useJavascriptRedirect)
        {
            if (useJavascriptRedirect)
            {
                SetMessage(message, HttpContext.Current.Request.RawUrl, true);
            }
            else
            {
                SetMessage(message);
            }
        }
        public static void SetMessage(string message)
        {
            SetMessage(message, HttpContext.Current.Request.RawUrl);
        }

        public static void SetMessage(string message, string url)
        {
            HttpContext.Current.Session[Constants.DropDownNoticeSession] = message;
            HttpContext.Current.Response.Redirect(url);            
        }

        public static void SetMessage(string message, string url, bool useJavascriptRedirect)
        {
            if (useJavascriptRedirect)
            {
                Page page = ((AdminPage)HttpContext.Current.Handler);
                page.ClientScript.RegisterStartupScript(page.GetType(), "redirect", "window.location='" + url + "'", true);
            }
            else
            {
                HttpContext.Current.Session[Constants.DropDownNoticeSession] = message;
                HttpContext.Current.Response.Redirect(url);
            }
        }

        public static string GetMessage()
        {
            string message = "";

            if (HttpContext.Current.Session != null && HttpContext.Current.Session[Constants.DropDownNoticeSession] != null)
            {
                message = HttpContext.Current.Session[Constants.DropDownNoticeSession].ToString();
                HttpContext.Current.Session.Remove(Constants.DropDownNoticeSession);
            }

            return message;
        }
    }
}