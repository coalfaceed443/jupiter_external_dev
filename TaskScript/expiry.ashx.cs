using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using CRM.Code.Models;
using System.Globalization;
using CRM.Code.Utils.Enumeration;

namespace CRM.TaskScript
{
    /// <summary>
    /// Summary description for expiry
    /// </summary>
    public class expiry : IHttpHandler
    {


        public void ProcessRequest(HttpContext context)
        {
            RunImport(0, 20000);
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
        private enum accountPos : int
        {
            Firstname, Surname, ConstituteID, Programme, TotalAmount, UpgradedOn, JoinedOn,
            HistoryRenewDate, HistoryExpireDate, MembershipID, TransactionType, PayMethod
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

                            var passes = person.CRM_AnnualPassPersons.Select(c => c.CRM_AnnualPass);

                            foreach (CRM_AnnualPass pass in passes)
                            {
                                if (pass.ExpiryDate.ToString("MM/dd/yyyy") == SwitchUsToUk(contentsSplit[(int)accountPos.HistoryExpireDate]).ToString("MM/dd/yyyy") && pass.CRM_AnnualPassCard.MembershipNumber.ToString() == contentsSplit[(int)accountPos.MembershipID])
                                {

                                    pass.AmountPaid = Convert.ToDecimal(contentsSplit[(int)accountPos.TotalAmount].Substring(1));

                                    try
                                    {
                                        pass.PaymentMethod = (byte)Enumeration.GetEnumValueByName<CRM.Code.Helpers.PaymentType.Types>(contentsSplit[(int)accountPos.PayMethod]);
                                    }
                                    catch
                                    {
                                        pass.PaymentMethod = (byte)CRM.Code.Helpers.PaymentType.Types.Unknown;
                                        HttpContext.Current.Response.Write(pass.ID + "<br/>");
                                    }

                                }
                            }

                            db.SubmitChanges();
                        }
                        catch { }
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