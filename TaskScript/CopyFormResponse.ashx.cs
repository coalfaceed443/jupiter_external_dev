using CRM.Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.TaskScript
{
    /// <summary>
    /// Summary description for CopyFormResponse
    /// </summary>
    public class CopyFormResponse : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            MainDataContext db = new MainDataContext();

            foreach (CRM_FormFieldAnswer formFieldAnswer in db.CRM_FormFieldAnswers)
            {
                string[] individualAnswerSet = formFieldAnswer.Answer.Split(new string[] { "<br/>" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string individualAnswer in individualAnswerSet)
                {
                    CRM_FormField field = formFieldAnswer.CRM_FormField;
                    CRM_FormFieldItem item = field.CRM_FormFieldItems.FirstOrDefault(f => f.Label == individualAnswer);


                    CRM_FormFieldResponse formFieldResponse = new CRM_FormFieldResponse();
                    formFieldResponse.TargetReference = formFieldAnswer.TargetReference;
                    formFieldResponse.CRM_FormFieldID = field.ID;

                    if (field.Type == (byte)CRM_FormField.Types.SingleLineTextBox || field.Type == (byte)CRM_FormField.Types.MultiLineTextBox || field.Type == (byte)CRM_FormField.Types.SingleCheckBox)
                    {
                        formFieldResponse.Answer = individualAnswer;
                        formFieldResponse.CRM_FormFieldItemID = null;
                        db.CRM_FormFieldResponses.InsertOnSubmit(formFieldResponse);
                        db.SubmitChanges();

                        if (individualAnswerSet.Count() > 1)
                        {
                            HttpContext.Current.Response.Write("multiple answers for single item" + individualAnswerSet.Count() + " on " + formFieldAnswer.ID);
                        }



                    }
                    else
                    {
                        if (item == null)
                        {
                            // this answer doesn't exist as an option, possibly because of the issue we are resolving here (changing parent item doesn't update responses since just a mash of strings).

                            item = new CRM_FormFieldItem();
                            item.IsArchived = true;
                            item.IsActive = false;
                            item.Label = individualAnswer;
                            item.OrderNo = CRM.Code.Utils.Ordering.Ordering.GetNextOrderID(db.CRM_FormFieldItems);
                            item.CRM_FormFieldID = field.ID;
                            db.CRM_FormFieldItems.InsertOnSubmit(item);
                            db.SubmitChanges();
                        }


                        formFieldResponse.Answer = "";
                        formFieldResponse.CRM_FormFieldItemID = item.ID;
                        db.CRM_FormFieldResponses.InsertOnSubmit(formFieldResponse);
                        db.SubmitChanges();

                    }
                    

                }

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