using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Code.Helpers.WebService
{
    public class Response
    {
        public bool IsSuccessful { get; set; }

        private string _Message;
        public string Message
        {
            get
            {
                if (!String.IsNullOrEmpty(_Message))
                    return _Message;
                else if (Messages != null && Messages.Any())
                {
                    String.Join("<br/>", Messages.ToArray());
                }
                return "";
            }
            set { _Message = value; }
        }

        public List<string> Messages { get; set; }
        public Dictionary<string, string> Data { get; set; }
    }
}