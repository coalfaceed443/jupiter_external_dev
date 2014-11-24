using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using CRM.Code.Utils.RemotePost;

namespace CRM.Code.Utils.ECommerce
{
    /// <summary>
    /// Summary description for CurrencyConverter
    /// </summary>
    public class CurrencyConverter
    {
        public static string ConvertCurrency(string fromCurrencyCode, string toCurrencyCode, decimal amount)
        {
            string Expression = amount.ToString("F2") + fromCurrencyCode + "%3D%3F" + toCurrencyCode;
            string url = "http://www.google.com/ig/calculator?hl=en&q=" + Expression;

            char[] cChar = new char[3];
            string[] _params = new string[100];

            cChar[0] = ',';
            _params = ASyncPost.PostData(url, "", ASyncPost.PostTypes.Post).Split(cChar[0]);

            string ConvertedAmount = "";

            ConvertedAmount = _params[1];
            ConvertedAmount = ConvertedAmount.Replace("\"", "");
            ConvertedAmount = ConvertedAmount.Replace("rhs", "");
            ConvertedAmount = ConvertedAmount.Replace(":", "");
            ConvertedAmount = ConvertedAmount.Trim();
            ConvertedAmount = ConvertedAmount.Remove(ConvertedAmount.IndexOf(' '), ConvertedAmount.Length - ConvertedAmount.IndexOf(' '));

            return ConvertedAmount;
        }
    }
}