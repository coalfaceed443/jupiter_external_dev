using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;
using System.IO;

namespace CRM.TaskScript
{
    /// <summary>
    /// Summary description for ConstType
    /// </summary>
    public class ConstType : IHttpHandler
    {

        private enum accountPos : int
        {
            ConstID, Firstname, Surname, ConstDesc
        }

        public void ProcessRequest(HttpContext context)
        {
            RunImport(141, 10000);
        }

        protected int lineNumber = 0;
        public void RunImport(int startLine, int endLine)
        {
            MainDataContext db = new MainDataContext();

            HttpServerUtility Server = HttpContext.Current.Server;

            string path = "/TaskScript/ConstTypes.csv";
            using (StreamReader reader = new StreamReader(Server.MapPath(path)))
            {

                lineNumber = 0;

                while (!reader.EndOfStream)
                {

                    lineNumber++;

                    string contents = reader.ReadLine();

                    if (lineNumber >= startLine && lineNumber <= endLine)
                    {
                        string[] contentsSplit = contents.Split(',');
                        string errorMessage = "";


                        CRM_FormFieldItem ffItem = db.CRM_FormFieldItems.FirstOrDefault(f => f.CRM_FormFieldID == 1 && f.Label.ToLower() == contentsSplit[(int)accountPos.ConstDesc].ToLower());


                        if (ffItem == null)
                        {
                            ffItem = new CRM_FormFieldItem()
                            {
                                Label = contentsSplit[(int)accountPos.ConstDesc],
                                CRM_FormFieldID = 1,
                                OrderNo = Code.Utils.Ordering.Ordering.GetNextOrderID(db.CRM_FormFieldItems),
                                IsActive = true,
                                IsArchived = false
                            };

                            db.CRM_FormFieldItems.InsertOnSubmit(ffItem);
                            db.SubmitChanges();
                        }

                        CRM_Person person = db.CRM_Persons.SingleOrDefault(s => s.LegacyID.ToString() == contentsSplit[(int)accountPos.ConstID]);

                        if (person != null)
                        {

                            CRM_FormFieldAnswer answer = db.CRM_FormFieldAnswers.FirstOrDefault(f => f.TargetReference == person.Reference && f.CRM_FormFieldID == 1);


                            if (answer == null)
                            {

                                answer = new CRM_FormFieldAnswer()
                                {
                                    Answer = contentsSplit[(int)accountPos.ConstDesc],
                                    CRM_FormFieldID = ffItem.CRM_FormFieldID,
                                    TargetReference = person.Reference
                                };

                                db.CRM_FormFieldAnswers.InsertOnSubmit(answer);
                                db.SubmitChanges();
                            }
                            else
                            {
                                string[] items = answer.Answer.Split(new string[] { "<br/>" }, StringSplitOptions.None);

                                if (!items.Any(i => i == contentsSplit[(int)accountPos.ConstDesc]))
                                {
                                    answer.Answer += "<br/>" + contentsSplit[(int)accountPos.ConstDesc];
                                }

                                db.SubmitChanges();
                            }
                        }
                        else
                        {
                            HttpContext.Current.Response.Write("Person not in system - " + contentsSplit[(int)accountPos.ConstID] + "<br/>");
                        }


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