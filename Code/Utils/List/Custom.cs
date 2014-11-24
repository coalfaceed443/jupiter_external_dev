using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using CRM.Code.Models;

namespace CRM.Code.Utils.List
{
    public interface ICustomField
    {
        string Reference { get; }
    }



    public class Custom
    {
        public static object Eval(object sentObject, string propertyname)
        {
            try
            {
                return DataBinder.Eval(sentObject, propertyname);
            }
            catch
            {
                string answer = String.Empty;

                try
                {
                    using (MainDataContext db = new MainDataContext())
                    {
                        CRM_FormFieldAnswer Answer = db.CRM_FormFieldAnswers.FirstOrDefault(f => f.CRM_FormField.ID.ToString() == propertyname && f.TargetReference == ((ICustomField)sentObject).Reference);
                        if (Answer != null)
                            answer = Answer.Answer;

                    }
                }
                catch(InvalidCastException ex)
                {

                }

                return answer;
            }
        }
    }
}