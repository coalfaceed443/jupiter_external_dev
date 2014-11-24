using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;

/// <summary>
/// Summary description for ErrorMessage
/// </summary>
namespace CRM.Code.Utils.Error
{
    public class ErrorMessage
    {
        static public string GetHtmlTitle(Exception ex)
        {
            return CleanHtml(ex.Message);
        }

        static public string GetHtmlVariableInfo()
        {
            // Heading Template
            const string heading = "<table border=\"0\" width=\"100%\" cellpadding=\"1\" cellspacing=\"0\"><tr><td bgcolor=\"#666666\" colspan=\"2\"><font face=\"Arial\" color=\"white\"><b>&nbsp;<!--HEADER--></b></font></td></tr></table>";

            // Error Message Header
            string html = String.Format("<font face=\"Arial\" size=\"4\" color=\"red\">Variables have been posted</font><br><br>");

            // QueryString Collection
            if (HttpContext.Current.Request.QueryString.Count > 0)
            {
                html += "<BR><BR>" + heading.Replace("<!--HEADER-->", "QueryString Collection");
                html += CollectionToHtmlTable(HttpContext.Current.Request.QueryString);
            }

            // Form Collection
            if (HttpContext.Current.Request.Form.Count > 0)
            {
                html += "<BR><BR>" + heading.Replace("<!--HEADER-->", "Form Collection");
                html += CollectionToHtmlTable(HttpContext.Current.Request.Form);
            }

            // Cookies Collection
            if (HttpContext.Current.Request.Cookies.Count > 0)
            {
                html += "<BR><BR>" + heading.Replace("<!--HEADER-->", "Cookies Collection");
                html += CollectionToHtmlTable(HttpContext.Current.Request.Cookies);
            }

            // Session Variables
            if (HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session.Count > 0)
                {
                    html += "<BR><BR>" + heading.Replace("<!--HEADER-->", "Session Variables");
                    html += CollectionToHtmlTable(HttpContext.Current.Session);
                }
            }

            // Server Variables
            if (HttpContext.Current.Request.ServerVariables.Count > 0)
            {
                html += "<BR><BR>" + heading.Replace("<!--HEADER-->", "Server Variables");
                html += CollectionToHtmlTable(HttpContext.Current.Request.ServerVariables);
            }

            return html;
        }

        static public string GetHtmlError(Exception ex)
        {
            // Heading Template
            const string heading = "<table border=\"0\" width=\"100%\" cellpadding=\"1\" cellspacing=\"0\"><tr><td bgcolor=\"#666666\" colspan=\"2\"><font face=\"Arial\" color=\"white\"><b>&nbsp;<!--HEADER--></b></font></td></tr></table>";

            // Error Message Header
            string html = String.Format("<font face=\"Arial\" size=\"4\" color=\"red\">An error occurred at {0}<br><font size=\"3\">&nbsp;&nbsp;&nbsp;&nbsp;{1}</font></font><br><br>", HttpContext.Current.Request.Url.ToString(), CleanHtml(ex.Message));

            // Populate Error Information Collection
            while (ex != null)
            {

                // Populate Error Information Collection
                NameValueCollection errorInfo = new NameValueCollection();
                errorInfo.Add("Source", ex.Source);
                errorInfo.Add("Exception Type", ex.GetType().ToString());
                errorInfo.Add("Message", ex.Message);
                errorInfo.Add("StackTrace", ex.StackTrace);
                errorInfo.Add("TargetSite", ex.TargetSite.ToString());

                // Error Information
                html += heading.Replace("<!--HEADER-->", "Error Information");
                html += CollectionToHtmlTable(errorInfo);

                ex = ex.InnerException;
            }

            // QueryString Collection
            if (HttpContext.Current.Request.QueryString.Count > 0)
            {
                html += "<BR><BR>" + heading.Replace("<!--HEADER-->", "QueryString Collection");
                html += CollectionToHtmlTable(HttpContext.Current.Request.QueryString);
            }

            // Form Collection
            if (HttpContext.Current.Request.Form.Count > 0)
            {
                html += "<BR><BR>" + heading.Replace("<!--HEADER-->", "Form Collection");
                html += CollectionToHtmlTable(HttpContext.Current.Request.Form);
            }

            // Cookies Collection
            if (HttpContext.Current.Request.Cookies.Count > 0)
            {
                html += "<BR><BR>" + heading.Replace("<!--HEADER-->", "Cookies Collection");
                html += CollectionToHtmlTable(HttpContext.Current.Request.Cookies);
            }

            // Session Variables
            if (HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session.Count > 0)
                {
                    html += "<BR><BR>" + heading.Replace("<!--HEADER-->", "Session Variables");
                    html += CollectionToHtmlTable(HttpContext.Current.Session);
                }
            }

            // Server Variables
            if (HttpContext.Current.Request.ServerVariables.Count > 0)
            {
                html += "<BR><BR>" + heading.Replace("<!--HEADER-->", "Server Variables");
                html += CollectionToHtmlTable(HttpContext.Current.Request.ServerVariables);
            }

            return html;
        }

        static private string CollectionToHtmlTable(NameValueCollection collection)
        {
            // <TD>...</TD> Template
            const string TD = "<td><font face=\"Arial\" size=\"2\"><!--VALUE--></font></td>";

            // Table Header
            string html = "\n<table width=\"100%\" bgcolor=\"#d1d9e4\" cellspacing=\"1\" cellpadding=\"3\">\n"
                + "  <tr bgcolor=\"#d0d0d0\">" + TD.Replace("<!--VALUE-->", "&nbsp;<b>Name</b>")
                + "  " + TD.Replace("<!--VALUE-->", "&nbsp;<b>Value</b>") + "</tr>\n";

            // No Body? -> N/A
            if (collection.Count == 0)
            {
                collection = new NameValueCollection();
                collection.Add("N/A", "");
            }

            // Table Body
            for (int i = 0; i < collection.Count; i++)
            {
                html += "<tr valign=\"top\" bgcolor=\"" + ((i % 2 == 0) ? "white" : "#f4f4f4") + "\">"
                    + TD.Replace("<!--VALUE-->", collection.Keys[i]) + "\n"
                    + TD.Replace("<!--VALUE-->", CleanHtml(collection[i])) + "</tr>\n";
            }

            // Table Footer
            return html + "</table>";
        }

        static private string CollectionToHtmlTable(HttpCookieCollection collection)
        {
            // Overload for HttpCookieCollection collection.
            // Converts HttpCookieCollection to NameValueCollection
            NameValueCollection NVC = new NameValueCollection();
            foreach (string item in collection) NVC.Add(item, collection[item].Value);
            return CollectionToHtmlTable(NVC);
        }

        static private string CollectionToHtmlTable(System.Web.SessionState.HttpSessionState collection)
        {
            // Overload for HttpSessionState collection.
            // Converts HttpSessionState to NameValueCollection
            NameValueCollection NVC = new NameValueCollection();
            foreach (string item in collection)
	    {
	        string output = "";
	        if (collection[item] != null)
	            output = collection[item].ToString();
	        NVC.Add(item, output);
            }
            return CollectionToHtmlTable(NVC);
        }

        static private string CleanHtml(string Html)
        {
            // Cleans the string for HTML friendly display
            return (Html.Length == 0) ? "" : Html.Replace("&", "&amp;").Replace("<", "&lt;").Replace("\r\n", "<BR>").Replace(" ", "&nbsp;");
        }
    }
}