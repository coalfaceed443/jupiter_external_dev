using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

/// <summary>
/// Summary description for Error
/// </summary>
namespace CRM.Code.Utils.Error
{
    public static class ErrorLog
    {
        public static void AddEntry(Exception e)
        {
            string title = ErrorMessage.GetHtmlTitle(e);
            string message = ErrorMessage.GetHtmlError(e);
            string timeStamp = DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss-ffff");

            // create error file //


            try
            {
                string filePath = HttpContext.Current.Server.MapPath(Constants.ErrorLogPath + timeStamp + ".html");

                FileStream fs = File.Open(filePath, FileMode.CreateNew, FileAccess.Write);

                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);

                sw.Write(message);

                sw.Close();
                fs.Close();

                // amend error log //

                string logFilePath = HttpContext.Current.Server.MapPath(Constants.ErrorLogPath + "error-log.txt");

                fs = File.Open(logFilePath, FileMode.Append, FileAccess.Write);

                sw = new StreamWriter(fs, System.Text.Encoding.UTF8);

                sw.WriteLine(timeStamp + " - " + title);

                sw.Close();
                fs.Close();
            }
            catch(System.IO.IOException ex)
            {
                // throw away ex
            }
        }
    }
}
