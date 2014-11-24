using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using CRM.Code.Helpers;
using CRM.Code.Models;
using System.Web.UI;
using CRM.Code.Utils.List;
namespace CRM.Code
{
    public class CSVExport
    {

        public static void GenericExport(List<_DataTableColumn> Headers, IEnumerable<ListData> Data, HttpResponse response, string filename)
        {
            string columnNames = "";

            Headers = Headers.OrderBy(h => h.OrderNo).ToList();
            Data = Data.OrderBy(d => d.OrderNo);

            for (int i=0; i <= Headers.Count() - 1;i++)
            {
                columnNames += Headers[i]._DataFieldFriendly + (i + 1 == Headers.Count() ? "" : ",");
            }
       
            response.Clear();
            response.ContentType = "text/csv";
            response.AddHeader("content-disposition", "attachment; filename=\"" + filename + "-" + DateTime.UtcNow.ToString("dd-MM-yyyy") + ".csv\"");

            HttpContext.Current.Response.Write(columnNames);
            HttpContext.Current.Response.Write(Environment.NewLine);

             StringBuilder sbItems = new StringBuilder();
             foreach (ListData item in Data)
             {
                 sbItems = new StringBuilder();
                 foreach (_DataTableColumn col in Headers)
                 {
                     string output = Custom.Eval(item.DataItem, col._DataFieldName).ToString();

                     if (col._DataFieldFriendly == "View")
                     {
                         output = "=HYPERLINK(\"" + Constants.DomainName + "" + DataBinder.Eval((item.DataItem), "DetailsURL") + "\", \"VIEW\")";
                     }
                     
                     AddComma(output, sbItems);
                 }

                 response.Write(sbItems.ToString());
                 response.Write(Environment.NewLine);
             }

             response.End();
        }

        private static void AddComma(string value, StringBuilder stringBuilder)
        {
            bool mustQuote = (value.Contains(",") || value.Contains("\"") || value.Contains("\r") || value.Contains("\n"));

            StringBuilder sb = new StringBuilder();

            if (mustQuote)
            {

                sb.Append("\"");
                foreach (char nextChar in value)
                {
                    sb.Append(nextChar);
                    if (nextChar == '"')
                        sb.Append("\"");
                }
                sb.Append("\"");

            }
            else
            {
                sb = sb.Append(value);
            }

            stringBuilder.Append(sb.ToString());
            stringBuilder.Append(",");
        }

    }


}