using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;
using System.Web.UI.WebControls;
using System.IO;

namespace CRM.Code.Utils.Files
{
    public static class Files
    {
        /// <summary>
        /// Creates the path to a given filename
        /// </summary>
        /// <param name="mapPath">Whether the path requires the function 'MapPath'</param>
        /// <param name="clearCache">Appends a query string to ensure the latest file is returned</param>
        /// <param name="filePath">The path to the file</param>
        /// <returns>string: The complete path to the file</returns>
        public static string GetFilePath(bool mapPath, bool clearCache, string filePath)
        {
            string suffix = "";

            if (clearCache)
            {
                suffix = "?" + DateTime.Now.Ticks.ToString();
            }

            if (!mapPath)
            {
                return filePath + suffix;
            }
            else
            {
                return HttpContext.Current.Server.MapPath(filePath + suffix);
            }
        }

        /// <summary>
        /// Gets the file extension for the given file name
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        /// <returns>string: The extension of the given file</returns>
        public static string GetFileExtension(string filePath)
        {
            return Path.GetExtension(filePath).ToLower();
        }

        /// <summary>
        /// Checks to see if a file exists, automatically performs a mappath on the string input
        /// </summary>
        /// <param name="filePath">The path to the file (non MapPath)</param>
        /// <returns>Bool: Whether the file exists or not</returns>
        public static bool IsFileExist(string filePath)
        {
            return File.Exists(HttpContext.Current.Server.MapPath(filePath));
        }

        /// <summary>
        /// Checks to see if the directory exists, if not creates it
        /// </summary>
        /// <param name="directory">The (non MapPath'd) directory string</param>
        /// <returns>string: The directory path passed in</returns>
        public static string CheckDirectory(string directory)
        {
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(directory)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(directory));
            }

            return directory;
        }
    }
}