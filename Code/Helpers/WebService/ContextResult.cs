using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Website.Code.Helpers.WebService
{
    public class ContextResult<T> where T : class
    {
        private bool _isSuccess = false;
        public bool IsSuccess
        {
            get
            {
                return _isSuccess;
            }
            set
            {
                _isSuccess = value;
                if (value == false)
                {
                    Messages.Add("Authorisation failed");
                    ReturnObject = null;
                }
            }
        }
        public List<string> Messages { get; set; }
        public T ReturnObject { get; set; }

        public ContextResult()
        {
            IsSuccess = true;
            Messages = new List<string>();
        }
    }
}
