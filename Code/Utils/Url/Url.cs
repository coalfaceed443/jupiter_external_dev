using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;

/// <summary>
/// Helpers for generating text
/// </summary>

namespace CRM.Code.Utils.Url
{
    public static class Url
    {
        public static string CreateFriendlyUrl(string input)
        {
            return CreateFriendlyUrl(input, false);
        }

        public static string CreateFriendlyUrl(string input, bool removeForwardSlashes)
        {

            StringBuilder output = new StringBuilder(input.ToLower());

            output.Replace("  ", "-");
            output.Replace(" ", "-");
            output.Replace(".", "");
            output.Replace("_", "-");
            output.Replace("&", "and");
            output.Replace("#", "");
            output.Replace("$", "");
            output.Replace("+", "");
            output.Replace("*", "");
            output.Replace(",", "");
            output.Replace(",", "");
            output.Replace(":", "");
            output.Replace(@"\", "");
            output.Replace("=", "");
            output.Replace("@", "");
            output.Replace("?", "");
            output.Replace("\"", "");
            output.Replace("'", "");
            output.Replace("<", "");
            output.Replace(">", "");
            output.Replace("%", "");
            output.Replace("[", "");
            output.Replace("]", "");
            output.Replace("|", "");
            output.Replace("{", "");
            output.Replace("}", "");
            output.Replace("(", "");
            output.Replace(")", "");
            output.Replace("!", "");
            output.Replace("£", "");
            
            if (removeForwardSlashes)
            {
                output.Replace("/", "-");
            }

            output.Replace("---", "-");
            output.Replace("--", "-");

            string output2 = output.ToString();

            if (output2.Length > 0)
            {

                if (output2.LastIndexOf('-') == (output2.Length - 1) && output2.LastIndexOf('-') != 0)
                {
                    output2 = output2.Substring(0, (output2.Length - 1));
                }

            }

            return output2;

        }

        public static string CheckHttp(string url)
        {
            string output = url;

            if (url.Length > 5)
            {
                if (url.ToLower().Substring(0, 5) != "http:")
                {
                    output = "http://" + url;
                }
            }

            return output;
        }

        public static void GeneratePaging(DataPager pager, ListView lvItems, string format)
        {
            GeneratePaging(pager, null, lvItems, format);
        }

        public static void GeneratePaging(DataPager pager, DataPager pager2, ListView lvItems, string format)
        {
            lvItems.DataBind();

            int count = pager.TotalRowCount;
            int pageSize = pager.PageSize;
            int pagesCount = count / pageSize + (count % pageSize == 0 ? 0 : 1);
            int pageSelected = (pager.StartRowIndex / pageSize) + 1;

            pager.Controls.Clear();

            for (int i = 1; i <= pagesCount; ++i)
            {
                if (pageSelected != i)
                {
                    HyperLink link = new HyperLink();
                    link.NavigateUrl = format.Replace("@ID@", i.ToString());
                    link.Text = "<span class=\"pager-link\">" + i.ToString() + "</span>";
                    pager.Controls.Add(link);
                }
                else
                {
                    Literal lit = new Literal();
                    lit.Text = "<span class=\"pager-current\">" + i.ToString() + "</span>";
                    pager.Controls.Add(lit);
                }

                Literal spaceb = new Literal();
                spaceb.Text = " ";
                pager.Controls.Add(spaceb);

            }

            if (pager2 != null)
            {
                count = pager2.TotalRowCount;
                pageSize = pager2.PageSize;
                pagesCount = count / pageSize + (count % pageSize == 0 ? 0 : 1);
                pageSelected = (pager.StartRowIndex / pageSize) + 1;

                pager2.Controls.Clear();

                for (int i = 1; i <= pagesCount; ++i)
                {
                    if (pageSelected != i)
                    {
                        HyperLink link = new HyperLink();
                        link.NavigateUrl = format.Replace("@ID@", i.ToString());
                        link.Text = "<span class=\"pager2-current\">" + i.ToString() + "</span>";
                        pager2.Controls.Add(link);
                    }
                    else
                    {
                        Literal lit = new Literal();
                        lit.Text = "<span class=\"pager2-link\">" + i.ToString() + "</span>";
                        pager2.Controls.Add(lit);
                    }

                    Literal spaceb = new Literal();
                    spaceb.Text = " ";
                    pager2.Controls.Add(spaceb);

                }
            }
        }

        public static int GetInt(string request, string redirectURL)
        {
            int value = 0;

            if (!Int32.TryParse(request, out value))
                HttpContext.Current.Response.Redirect(redirectURL);

            return value;
        }

        public static double GetDouble(string request, string redirectURL)
        {
            double value = 0;

            if (!Double.TryParse(request, out value))
                HttpContext.Current.Response.Redirect(redirectURL);

            return value;
        }

        public static string GetString(string request, string redirectURL)
        {
            string value = "";

            try
            {
                value = request.ToString();
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Redirect(redirectURL);
            }

            return value;
        }

        public static string GetEmbededVideoUrl(string url)
        {
            if (url.ToLower().Contains("youtube") || url.ToLower().Contains("youtu.be"))
                return "http://www.youtube.com/embed/" + GetVideoReference(url);
            else if (url.ToLower().Contains("vimeo"))
                return "http://player.vimeo.com/video/" + GetVideoReference(url) + "?title=0&amp;byline=0&amp;portrait=0";

            return url;
        }

        public static string GetEmbededVideoThumb(string url, string style)
        {
            if (url.ToLower().Contains("youtube") || url.ToLower().Contains("youtu.be"))
                return "<img src=\"http://i4.ytimg.com/vi/" + GetVideoReference(url) + "/0.jpg\" alt=\"\" style=\"" + style + "\" />";
            else if (url.ToLower().Contains("vimeo"))
            {
                string id = GetVideoReference(url);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://vimeo.com/api/v2/video/" + id + ".json?callback=?");
                request.Timeout = 10000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader sr = new StreamReader(response.GetResponseStream());
                    string json = sr.ReadToEnd();
                    json = json.Substring(json.ToLower().IndexOf("\"thumbnail_large\":\"") + 19);
                    json = json.Substring(0, json.IndexOf("\""));

                    return "<img src=\"" + json.Replace(@"\", "") + "\" alt=\"\" style=\"" + style + "\" />";
                }
            }

            return "<img src=\"/_assets/images/admin/novideo.jpg\" alt=\"No image\" />";
        }

        private static string GetVideoReference(string url)
        {
            string output = url;

            if (url.ToLower().Contains("youtube.com") || url.ToLower().Contains("youtu.be"))
            {
                if (url.ToLower().Contains("youtube.com"))
                {
                    if (url.ToLower().Contains("embed"))
                    {
                        if (url.ToLower().Contains("<iframe"))
                            url = url.Substring(url.ToLower().LastIndexOf("embed/") + 6);
                        else
                            url = url.Substring(url.ToLower().IndexOf("/v/") + 3);

                        if (url.Contains("?"))
                            url = url.Substring(0, url.IndexOf("?"));
                        else if (url.Contains("\""))
                            url = url.Substring(0, url.IndexOf("\""));

                        output = url;

                    }
                    else if (url.ToLower().Contains("v="))
                    {
                        url = url.Substring(url.ToLower().LastIndexOf("v=") + 2);

                        if (url.Contains("&"))
                            url = url.Substring(0, url.IndexOf("&"));

                        output = url;
                    }
                }
                else
                {
                    output = url.Substring(url.LastIndexOf("/") + 1);
                }
            }
            if (url.ToLower().Contains("vimeo"))
            {
                if (url.ToLower().Contains("<iframe"))
                {
                    url = url.Substring(url.ToLower().IndexOf("/video/") + 7);

                    if (url.Contains("/"))
                        url = url.Substring(0, url.IndexOf("\""));
                    if (url.Contains("&"))
                        url = url.Substring(0, url.IndexOf("&"));
                    if (url.Contains("?"))
                        url = url.Substring(0, url.IndexOf("?"));

                    output = url;

                }
                else if (url.ToLower().Contains("<object"))
                {
                    url = url.Substring(url.ToLower().IndexOf("clip_id=") + 8);

                    if (url.Contains("&"))
                        url = url.Substring(0, url.IndexOf("&"));
                    if (url.Contains("?"))
                        url = url.Substring(0, url.IndexOf("?"));

                    output = url;
                }
                else
                {
                    output = url.Substring(url.LastIndexOf("/") + 1);
                }
            }

            return output;
        }
    }
}