using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace CRM.Code.Utils.Error
{
    /// <summary>
    /// Summary description for ValidationError
    /// </summary>
    public class ValidationError : IValidator
    {
        string _message;

        public static void AddValidationError(string message)
        {
            Page page = HttpContext.Current.Handler as Page;
            ValidationError error = new ValidationError(message);
            page.Validators.Add(error);
        }

        private ValidationError(string message)
        {
            _message = message;
        }

        public string ErrorMessage
        {
            get { return _message; }
            set { }
        }

        public bool IsValid
        {
            get { return false; }
            set { }
        }

        public void Validate()
        { }
    }
}