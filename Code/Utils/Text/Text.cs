using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;

/// <summary>
/// Helpers for generating text
/// </summary>

namespace CRM.Code.Utils.Text
{
    public static class Text
    {
        public static string ToPrettyDay(DateTime date)
        {
            string formatted = date.ToString("dd");

            return formatted.StartsWith("0") ? formatted.Substring(1) : formatted;
        }

        public static string MailTo(string email)
        {
            return MailTo(email, email);
        }

        public static string MailTo(string email, string text)
        {
            return "<a href=\"mailto:" + email + "\">" + text + "</a>";
        }

        public static string ConvertUrlsToLinks(string url, string text)
        {
                return ("<a href=\"" +  url + "\">" + text + "</a>").Replace("href=\"www", "href=\"http://www");
        }



        public static string AlternativeText(string input, string alternative)
        {
            if (input.Length > 0)
            {
                return input;
            }
            else
            {
                return alternative;
            }
        }

        public static string ConvertYesNo(bool input)
        {
            if (input)
            {
                return "Yes";
            }
            else
            {
                return "No";
            }
        }

 	    public static string CreateNewPassword()
        {
            string characters = "BCDFGHJKLMNPQRSTVWXYZ";
            string vowels = "AEIOU";
            string numbers = "1234567890";
            string password = "";
            string prevChar = "";
            Random ran = new Random();
            for (int i = 0; i <= 8; i++)
            {

                int index = ran.Next(0, 21);
                int vowelindex = ran.Next(0, 5);
                int numberindex = ran.Next(0, 10);
                if (i > 6)
                {
                    prevChar = numbers[numberindex].ToString();
                }
                else if (vowels.Contains(prevChar))
                {
                    prevChar = characters[index].ToString();
                }
                else
                {
                    prevChar = vowels[vowelindex].ToString();
                }

                password += prevChar;
            }

            return password.ToLower();
        }

        public static string FirstSentence(string text)
        {
            string pattern = @"<h1>(.+?)</h1>";

            text = Regex.Replace(text, pattern, string.Empty);

            return StripHTML(text).Split('.')[0] + ".";
        }

        public static string LimitToWords(string text, int count)
        {
            if (String.IsNullOrEmpty(text))
                return "";

            char[] delimiters = { ' ', '\r', '\n' };

            string[] words = text.Split(
                delimiters,
                count,
                StringSplitOptions.RemoveEmptyEntries);

            if (words.Length == count)
                words[words.Length - 1] = "...";

            return String.Join(" ", words);
        }

        public static string Ordinal(int number)
        {
            string suffix = String.Empty;

            int ones = number % 10;
            int tens = (int)Math.Floor(number / 10M) % 10;

            if (tens == 1)
            {
                suffix = "th";
            }
            else
            {
                switch (ones)
                {
                    case 1:
                        suffix = "st";
                        break;

                    case 2:
                        suffix = "nd";
                        break;

                    case 3:
                        suffix = "rd";
                        break;

                    default:
                        suffix = "th";
                        break;
                }
            }
            return String.Format("{0}{1}", number, suffix);
        }

        public static string FormatTime(DateTime datetime)
        {
            string ampm;
            int houri = datetime.Hour;
            if (houri > 11)
            {
                ampm = "pm";
            }
            else
            {
                ampm = "am";
            }

            return datetime.ToString("h:mm") + ampm;
        }

        public static string CreateAMPM(string hour)
        {
            string ampm;
            int houri = Convert.ToInt32(hour);
            if (houri > 11)
            {
                ampm = "pm";
            }
            else
            {
                ampm = "am";
            }

            return ampm;
        }

        public static string CreateDate(DateTime Date)
        {
            return CreateDate(Date, false, true);
        }

        public static string CreateDate(DateTime Date, bool ShowDayName)
        {
            return CreateDate(Date, ShowDayName, true);
        }

        public static string CreateDate(DateTime Date, bool ShowDayName, bool ShowYear)
        {
            string fullDate;
            string dayWord;
            string dateEnd;

            dayWord = (ShowDayName) ? Date.ToString("dddd ") : "";

            int day = Convert.ToInt32(Date.ToString("dd"));
            dayWord += Ordinal(day);

            dateEnd = (ShowYear) ? Date.ToString("MMMM yyyy") : Date.ToString("MMMM");

            fullDate = dayWord + " " + dateEnd;

            return fullDate;
        }

        public static string CreateDateTime(DateTime Date)
        {
            string fullDate;
            string dayWord;

            int day = Convert.ToInt32(Date.ToString("dd"));
            dayWord = Ordinal(day);

            int hour = Convert.ToInt32(Date.ToString("HH"));
            string hours = Date.ToString("HH");
            string ampm = CreateAMPM(hours);

            fullDate = dayWord + " " + Date.ToString("MMMM yyyy");

            return fullDate + " : " + FormatTime(Date);
        }

        public static string PrettyDate(DateTime date)
        {
            string suffix = "th";

            if (date.Day < 10 || date.Day > 19)
                switch (date.Day % 10)
                {
                    case 1:
                        suffix = "st";
                        break;
                    case 2:
                        suffix = "nd";
                        break;
                    case 3:
                        suffix = "rd";
                        break;
                }

            return date.ToString("dddd MMMM d") + suffix +
                " " + date.Year.ToString();
        }

        public static string RemoveTags(string value, params string[] tags)
        {
            string stripped = value;
            string tagList = "(" + string.Join("|", tags) + ")";

            stripped = Regex.Replace(stripped,
                "<" + tagList + @"(/?>|(\s+[^>]*>))",
                String.Empty); // start tags            

            stripped = Regex.Replace(stripped,
                "</" + tagList + ">",
                String.Empty); // end tags

            return stripped;
        }

        public static string StripHTML(string htmlString)
        {

            string pattern = @"<(.|\n)*?>";

            return Regex.Replace(htmlString, pattern, string.Empty);

        }

        public static string TrimHTML(string htmlString, int length)
        {

            string output = StripHTML(htmlString);

            if (output.Length > length)
            {
                output = output.Substring(0, length);
                return output;
            }
            else
            {
                return output;
            }

        }

        public static string FileExtension(string filename)
        {
            int dotpos;
            string extension;

            dotpos = filename.LastIndexOf(".");

            extension = filename.Substring(dotpos + 1);

            return extension;
        }

        public static DateTime FormatInputDate(string input)
        {
            DateTime output = new DateTime(Convert.ToInt32(input.Substring(6, 4)), Convert.ToInt32(input.Substring(3, 2)), Convert.ToInt32(input.Substring(0, 2)));
            return output;
        }

        public static DateTime FormatInputDateTime(string input)
        {
            DateTime output = new DateTime(Convert.ToInt32(input.Substring(6, 4)), Convert.ToInt32(input.Substring(3, 2)), Convert.ToInt32(input.Substring(0, 2)), Convert.ToInt32(input.Substring(11, 2)), Convert.ToInt32(input.Substring(14, 2)), 0);
            return output;
        }

        /// <summary>
        /// Finds the first occurrence of the “begin” and “end” strings, then you can use result[1] to allow you to move ahead down the string to find the next value.
        /// </summary>
        /// <param name="strBegin">the string to start the search</param>
        /// <param name="strEnd">the string to stop the search</param>
        /// <param name="strSource">the string to search in</param>
        /// <param name="includeBegin">whether to return the begin in the result or not</param>
        /// <param name="includeEnd">whether to return the end in the result or not</param>
        /// <returns></returns>
        public static string[] GetStringInBetween(string strBegin, string strEnd, string strSource, bool includeBegin, bool includeEnd)
        {
            string[] result = { "", "" };

            int iIndexOfBegin = strSource.IndexOf(strBegin);

            if (iIndexOfBegin != -1)
            {

                // include the Begin string if desired

                if (includeBegin)
                    iIndexOfBegin -= strBegin.Length;

                strSource = strSource.Substring(iIndexOfBegin + strBegin.Length);

                int iEnd = strSource.IndexOf(strEnd);

                if (iEnd != -1)
                {
                    // include the End string if desired
                    if (includeEnd)
                        iEnd += strEnd.Length;

                    result[0] = strSource.Substring(0, iEnd);

                    // advance beyond this segment

                    if (iEnd + strEnd.Length < strSource.Length)
                        result[1] = strSource.Substring(iEnd + strEnd.Length);
                }
            }
            else
                // stay where we are
                result[1] = strSource;
            return result;
        }
    }
}