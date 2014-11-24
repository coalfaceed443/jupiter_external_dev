using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;
using System.IO;
using System.Globalization;
using CRM.Code.Utils.Enumeration;

namespace CRM.TaskScript
{
    /// <summary>
    /// Summary description for MemberHistory
    /// </summary>
    public class MemberHistory : IHttpHandler
    {

        private enum accountPos : int
        {
            Firstname,Surname,ConstituteID,Programme,TotalAmount,UpgradedOn,JoinedOn,
            HistoryRenewDate,HistoryExpireDate,MembershipID,TransactionType,PayMethod
        }

        public void ProcessRequest(HttpContext context)
        {
            RunImport(2, 2000);
        }

        public DateTime SwitchUsToUk(string StartDate)
        {
            if (StartDate.Length == 9)
            {
                StartDate = "0" + StartDate;
            }

            DateTime dt = DateTime.ParseExact(StartDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            return dt;
        }

        protected int lineNumber = 0;
        public void RunImport(int startLine, int endLine)
        {
            MainDataContext db = new MainDataContext();

            HttpServerUtility Server = HttpContext.Current.Server;

            string path = "/TaskScript/payment_history.csv";
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

                        try
                        {
                            CRM_Person person = db.CRM_Persons.SingleOrDefault(s => s.LegacyID.ToString() == contentsSplit[(int)accountPos.ConstituteID]);

                            if (person != null)
                            {
                                DateTime startDate = SwitchUsToUk(contentsSplit[(int)accountPos.JoinedOn]);

                                if (!String.IsNullOrEmpty(contentsSplit[(int)accountPos.HistoryRenewDate]))
                                {
                                    startDate = SwitchUsToUk(contentsSplit[(int)accountPos.HistoryRenewDate]);
                                }

                                CRM_AnnualPass pass = db.CRM_AnnualPasses.ToArray().FirstOrDefault(f =>
                                    f.CRM_AnnualPassCard.MembershipNumber.ToString() == contentsSplit[(int)accountPos.MembershipID] &&
                                    f.CRM_AnnualPassPersons.Any(c => c.CRM_PersonID == person.ID) &&
                                    f.StartDate.ToString("dd/MM/yyyy") == startDate.ToString("dd/MM/yyyy"));


                                if (pass == null)
                                {

                                    CRM_AnnualPassCard card = db.CRM_AnnualPassCards.FirstOrDefault(s => s.MembershipNumber.ToString() == contentsSplit[(int)accountPos.MembershipID]);

                                    if (card == null)
                                    {
                                        card = new CRM_AnnualPassCard()
                                        {
                                            MembershipNumber = Convert.ToInt32(contentsSplit[(int)accountPos.MembershipID])
                                        };

                                        db.CRM_AnnualPassCards.InsertOnSubmit(card);
                                        db.SubmitChanges();
                                    }

                                    pass = new CRM_AnnualPass()
                                    {
                                        AmountPaid = 0,
                                        CRM_AnnualPassCardID = card.ID,
                                        CRM_OfferID = null,
                                        DiscountApplied = "",
                                        IsArchived = false,
                                        IsPending = false,
                                        PrimaryContactReference = person.Reference
                                    };

                                    db.CRM_AnnualPasses.InsertOnSubmit(pass);
                                }


                                pass.StartDate = startDate;

                                if (!String.IsNullOrEmpty(contentsSplit[(int)accountPos.HistoryExpireDate]))
                                {
                                    pass.ExpiryDate = SwitchUsToUk(contentsSplit[(int)accountPos.HistoryExpireDate]);
                                }
                                else
                                {
                                    pass.ExpiryDate = pass.StartDate.AddYears(1);
                                }


                                try
                                {
                                    pass.PaymentMethod = (byte)Enumeration.GetEnumValueByName<CRM.Code.Helpers.PaymentType.Types>(contentsSplit[(int)accountPos.PayMethod]);
                                }
                                catch
                                {
                                    pass.PaymentMethod = (byte)CRM.Code.Helpers.PaymentType.Types.Unknown;
                                }

                                if (!String.IsNullOrEmpty(contentsSplit[(int)accountPos.TotalAmount]))
                                {
                                    pass.AmountPaid = Convert.ToDecimal(contentsSplit[(int)accountPos.TotalAmount].Substring(1));
                                }
                                else
                                {
                                    pass.AmountPaid = 0M;
                                }


                                CRM_AnnualPassType type = db.CRM_AnnualPassTypes.FirstOrDefault(f => f.DefaultPrice == pass.AmountPaid);

                                if (type == null)
                                {
                                    pass.CRM_AnnualPassTypeID = db.CRM_AnnualPassTypes.First(f => f.ID == 15).ID;
                                }
                                else
                                {
                                    pass.CRM_AnnualPassTypeID = type.ID;
                                }

                                db.SubmitChanges();

                                if (!pass.CRM_AnnualPassPersons.Any(c => c.CRM_PersonID == person.ID))
                                {
                                    CRM_AnnualPassPerson passperson = new CRM_AnnualPassPerson()
                                    {
                                        CRM_PersonID = person.ID,
                                        IsArchived = false,
                                        CRM_AnnualPassID = pass.ID
                                    };

                                    db.CRM_AnnualPassPersons.InsertOnSubmit(passperson);
                                    db.SubmitChanges();
                                }

                            }
                            else
                            {
                                HttpContext.Current.Response.Write(contentsSplit[(int)accountPos.ConstituteID] + " person not found");
                            }

                        }
                        catch(Exception ex)
                        {
                            HttpContext.Current.Response.Write(ex.Message);
                            HttpContext.Current.Response.Write(lineNumber);
                            HttpContext.Current.Response.End();
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