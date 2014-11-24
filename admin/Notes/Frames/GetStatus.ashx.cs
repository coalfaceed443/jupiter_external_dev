using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using CRM.Code.Managers;
using CRM.Code.BasePages.Admin;

namespace CRM.admin.Notes.Frames
{
    /// <summary>
    /// Summary description for GetStatus
    /// </summary>
    public class GetStatus : IHttpHandler 
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.AddHeader("Content-type", "text/json");

            NoteManager NoteManager = new NoteManager();
            bool IsRead = NoteManager.IsRead(Convert.ToInt32(HttpContext.Current.Request.QueryString["id"]));
            context.Response.Write(JsonConvert.SerializeObject(new ReadStatus(IsRead)));
        }


        public class ReadStatus
        {
            public bool IsRead { get; set; }
            public string Message = "";
            public ReadStatus(bool isRead)
            {
                IsRead = isRead;
                Message = IsRead ? "Mark as Unread" : "Mark as Read";
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

}