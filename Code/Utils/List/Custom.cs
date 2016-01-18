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

                        string answers = "";

                        foreach (CRM_FormFieldResponse response in db.CRM_FormFieldResponses.Where(r => r.CRM_FormFieldID.ToString() == propertyname && r.TargetReference == ((ICustomField)sentObject).Reference))
                        {
                            if (response.CRM_FormFieldItemID != null)
                            {
                                answers += response.CRM_FormFieldItem.Label + "<br/>";
                            }
                            else if (!String.IsNullOrEmpty(response.Answer))
                                answers += response.Answer + "<br/>";
                        }

                        answer = answers;
                        

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