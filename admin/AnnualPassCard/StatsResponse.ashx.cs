using CRM.Code.Managers;
using CRM.Code.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.admin.AnnualPassCard
{
    /// <summary>
    /// Summary description for StatsResponse
    /// </summary>
    public class StatsResponse : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            HttpRequest Request = HttpContext.Current.Request;

            string type = Request["type"];

            AnnualPassManager passManager = new AnnualPassManager();

            string output = "";
            string date = Request["date"];
            DateTime formedDate = CRM.Code.Utils.Text.Text.FormatInputDate(date);


            switch (type)
            {
                case "bydate":
                    {
                        int activeOnDate = passManager.GetMembersOnDate(formedDate);
                        int joinedDate = passManager.GetMembersOnJoinDate(formedDate);

                        output = JsonConvert.SerializeObject(new { ActiveOnDate = activeOnDate, JoinedOnDate = joinedDate });

                        break;
                    }
            }

            context.Response.Write(output);
            context.Response.End();
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