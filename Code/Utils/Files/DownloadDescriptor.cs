using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Code.Utils.Files
{
    /// <summary>
    /// Utility class for handling MimeTypes
    /// </summary>
    public class DownloadDescriptor
    {
        public string Mime { get; set; }
        public string Description { get; set; }
        public string Filename { get; set; }
        public string Extension { get; set; }

        /// <summary>
        /// Instantiate a DownloadDescriptor with Mime and Description based on filetype
        /// </summary>
        /// <param name="strFileName"></param>
        public DownloadDescriptor(string strFileName)
        {
            Filename = strFileName.Split('.')[0];
            Extension = System.IO.Path.GetExtension(strFileName).ToLower();

            switch (Extension)
            {
                case ".avi":
                    Mime = "video/avi";
                    Description = "Video";
                    break;
                case ".doc":
                case ".docx":
                    Mime = "application/msword";
                    Description = "Word Document";
                    break;
                case ".eps":
                    Mime = "application/eps";
                    Description = "EPS File";
                    break;
                case ".gif":
                    Mime = "image/gif";
                    Description = "Gif Image";
                    break;
                case ".gzip":
                    Mime = "application/x-gzip";
                    Description = "Zip";
                    break;
                case ".htm":
                    Mime = "text/html";
                    Description = "Text File";
                    break;
                case ".html":
                    Mime = "text/html";
                    Description = "HTML File";
                    break;
                case ".jpeg":
                case ".jpg":
                    Mime = "image/jpeg";
                    Description = "Jpeg File";
                    break;
                case ".m1v":
                case ".m2a":
                case ".m2v":
                case ".mp2":
                case ".mp3":
                case ".mpa":
                    Mime = "audio/mpeg";
                    Description = "Audio File";
                    break;

                case ".mpe":
                case ".mpeg":
                case ".mpg":
                    Mime = "video/mpeg";
                    Description = "Video File";
                    break;

                case ".mid":
                case ".midi":
                    Mime = "audio/midi";
                    Description = "MIDI Audio File";
                    break;

                case ".mov":
                    Mime = "video/quicktime";
                    Description = "Quicktime Video";
                    break;

                case ".pdf":
                    Mime = "application/pdf";
                    Description = "PDF";
                    break;

                case ".ppt":
                    Mime = "application/vnd.ms-powerpoint";
                    Description = "Powerpoint Presentation";
                    break;

                case ".qif":
                    Mime = "image/x-quicktime";
                    Description = "Quicktime Image";
                    break;

                case ".qt":
                    Mime = "image/x-quicktime";
                    Description = "Quicktime Video";
                    break;


                case ".txt":
                    Mime = "text/plain";
                    Description = "Plain Text File";
                    break;

                case ".xlw":
                case ".xlt":
                case ".xls":
                case ".xlsx":
                    Mime = "application/vnd.ms-excel";
                    Description = "Excel Spreadsheet";
                    break;

                case ".zip":
                    Mime = "application/zip";
                    Description = "ZIP file";
                    break;

                default:
                    Mime = "application/octet-stream";
                    Description = "Unknown";
                    break;
            }
        }

        /// <summary>
        /// Downloads the file using the fileName used when instantiating the object
        /// </summary>
        /// <param name="displayName"></param>
        public void DownloadFile()
        {
            DownloadFile(Filename);
        }

        /// <summary>
        /// Downloads the file using the fileName used when instantiating the object and
        /// displays the param displayName as an alias
        /// </summary>
        /// <param name="displayName"></param>
        public void DownloadFile(string displayName)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = Mime;
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=\"" + displayName + Extension + "\"");
            HttpContext.Current.Response.TransmitFile(HttpContext.Current.Server.MapPath(Filename + Extension));
            HttpContext.Current.Response.End();
        }
    }
}