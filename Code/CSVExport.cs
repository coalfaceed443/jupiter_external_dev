﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using CRM.Code.Helpers;
using CRM.Code.Models;
using System.Web.UI;
using CRM.Code.Utils.List;
using System.Reflection;
namespace CRM.Code
{
    public class CSVExport
    {

            /// <summary>
            /// Parses a list of objects and returns a csv file with all the correct header information and property values. 
            /// </summary>
            /// <param name="dList">The list to format</param>
            /// <param name="FileName">The name of the csv file (a timestamp is added)</param>
            public static void GenericExport(object[] dList, string FileName)
            {
                if (!dList.Any())
                { return; }

                var input = dList.First();

                Type dType = input.GetType();

                string HeaderText = "";

                //loop through the properties and format the header of the csv file
                //Any item will do, they all have the same properties.
                int limit = dType.GetProperties().Count();
                int count = 0;
                foreach (var prop in dType.GetProperties())
                {
                    HeaderText += prop.Name;

                    //If the first letters of a csv is 'ID', excel assumes you're trying to open an SYLK file.
                    //To avoid formatting errors, enclose an ID in quotes (which formats normally)
                    if (HeaderText.ToLower() == "id")
                    { HeaderText = "\"" + HeaderText + "\""; }

                    count++;

                    if (count != limit)
                    { HeaderText += ", "; }

                }

                var sbOutput = new StringBuilder();

                sbOutput.Append(HeaderText + "\r\n");

                //Go through each item in list
                foreach (object item in dList)
                {
                    Type itmType = item.GetType();

                    //Go through each property of each item
                    foreach (PropertyInfo pi in itmType.GetProperties())
                    {
                        object propValue = pi.GetValue(item, null);

                        var piType = pi.PropertyType;
                        if (propValue == null)
                        {
                            sbOutput.Append(FormatLine("NULL"));
                        }
                        else
                        {
                            //Format certain values certain ways.
                            if (piType == (typeof(DateTime)))
                            {
                                sbOutput.Append(FormatLine(((DateTime)propValue).ToString("dd.MM.yy")));
                            }
                            else if (piType == (typeof(decimal)))
                            {
                                sbOutput.Append(FormatLine(((decimal)propValue).ToString("N2")));
                            }
                            else
                            {
                                sbOutput.Append(FormatLine(propValue.ToString()));
                            }
                        }
                    }

                    sbOutput.Append("\r\n");
                }
                
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentEncoding = Encoding.GetEncoding("Windows-1252");
                HttpContext.Current.Response.ContentType = "text/csv";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + FileName + DateTime.UtcNow.ToString("dd-MM-yyyy") + ".csv");
                HttpContext.Current.Response.Write(sbOutput.ToString());
                HttpContext.Current.Response.End();

            }

        public class ExportByInterestHelper
        {
            public CRM_Person Person { get; set; }
            public IQueryable<CRM_FormFieldResponse> Responses { get; set; }
        }

        internal static void ContactsByInterest(List<ExportByInterestHelper> Helpers, List<CRM_FormFieldItem> interests, HttpResponse Response)
        {
            string columns = "Title, Firstname, Lastname, IsAnnualPassHolder, PrimaryEmail, CalculatedSalutation, PrimaryAddressLine1, PrimaryAddressLine2, PrimaryAddressLine3, PrimaryAddressLine4, PrimaryAddressLine5, PrimaryAddressTown, PrimaryAddressCounty, PrimaryAddressPostcode, PrimaryAddressCountry, NextExpiryDate, IsDoNotEmail, IsDoNotMail, IsDeceased, ";

               string filename = "contacts_by_interest";

             Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment; filename=\"" + filename + "-" + DateTime.UtcNow.ToString("dd-MM-yyyy") + ".csv\"");
            Response.ContentEncoding = Encoding.GetEncoding("Windows-1252");


            foreach (CRM_FormFieldItem item in interests)
            {
                columns += item.DisplayName.Replace(",", "") + ",";
            }

            HttpContext.Current.Response.Write(columns);
            HttpContext.Current.Response.Write(Environment.NewLine);


            StringBuilder sbItems = new StringBuilder();

            foreach (ExportByInterestHelper helper in Helpers)
            {
                CRM_Person p = helper.Person;

                AddComma(p.Title, sbItems);
                AddComma(p.Firstname, sbItems);
                AddComma(p.Lastname, sbItems);
                AddComma(p.IsAnnualPassHolder ? "TRUE" : "FALSE", sbItems);
                AddComma(p.PrimaryEmail, sbItems);
                AddComma(p.CalculatedSalutation, sbItems);
                AddComma(p.PrimaryAddressLine1, sbItems);
                AddComma(p.PrimaryAddressLine2, sbItems);
                AddComma(p.PrimaryAddressLine3, sbItems);
                AddComma(p.PrimaryAddressLine4, sbItems);
                AddComma(p.PrimaryAddressLine5, sbItems);
                AddComma(p.PrimaryAddressTown, sbItems);
                AddComma(p.PrimaryAddressCounty, sbItems);
                AddComma(p.PrimaryAddressPostcode, sbItems);
                AddComma(p.PrimaryAddressCountry, sbItems);
                AddComma(p.NextExpiryDate, sbItems);
                AddComma(p.IsDoNotEmail ? "TRUE" : "FALSE", sbItems);
                AddComma(p.IsDoNotMail ? "TRUE" : "FALSE", sbItems);
                AddComma(p.IsDeceased ? "TRUE" : "FALSE", sbItems);

                foreach (CRM_FormFieldItem interest in interests)
                {
                    if (helper.Responses.Any(c => c.TargetReference == p.Reference && c.CRM_FormFieldItemID == interest.ID))
                    {
                        AddComma("TRUE", sbItems);
                    }
                    else
                    {
                        AddComma("FALSE", sbItems);
                    }
                }

                Response.Write(sbItems.ToString());
                Response.Write(Environment.NewLine);
                sbItems = new StringBuilder();
            }


            Response.End();

        }

        internal static void ActiveFriendsByConstituent(List<FriendReportHelper> members, HttpResponse Response)
        {

            string columnNames = "Do not Email, Do not Post, Email, Relation Salutation, Title, Firstname, Surname, Address 1, Address 2, Address 3, Address 4, Address 5, Town, County, Postcode, Country,"
                + "Is Friend,Is Personal Friend, Expiry Date, Pass Type";

            string filename = "ActiveFriendsByConstituent";

            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment; filename=\"" + filename + "-" + DateTime.UtcNow.ToString("dd-MM-yyyy") + ".csv\"");
            Response.ContentEncoding = Encoding.GetEncoding("Windows-1252");

            HttpContext.Current.Response.Write(columnNames);
            HttpContext.Current.Response.Write(Environment.NewLine);

            StringBuilder sbItems = new StringBuilder();

            foreach (FriendReportHelper friend in members)
            {
                AddComma(friend.CRM_Person.IsDoNotEmail ? "TRUE" : "FALSE", sbItems);
                AddComma(friend.CRM_Person.IsDoNotMail ? "TRUE" : "FALSE", sbItems);                
                AddComma(friend.CRM_Person.PrimaryEmail, sbItems);

                AddComma(friend.CRM_Person.RelationshipSaltuation, sbItems);
                AddComma(friend.CRM_Person.Title, sbItems);
                AddComma(friend.CRM_Person.Firstname, sbItems);
                AddComma(friend.CRM_Person.Lastname, sbItems);
                AddComma(friend.CRM_Person.RelationshipAddress1, sbItems);
                AddComma(friend.CRM_Person.RelationshipAddress2, sbItems);
                AddComma(friend.CRM_Person.RelationshipAddress3, sbItems);
                AddComma(friend.CRM_Person.RelationshipAddress4, sbItems);
                AddComma(friend.CRM_Person.RelationshipAddress5, sbItems);
                AddComma(friend.CRM_Person.RelationshipTown, sbItems);
                AddComma(friend.CRM_Person.RelationshipCounty, sbItems);
                AddComma(friend.CRM_Person.RelationshipPostcode, sbItems);
                AddComma(friend.CRM_Person.RelationshipCountry, sbItems);
                AddComma(friend.IsFriend ? "TRUE" : "FALSE", sbItems);
                AddComma(friend.IsPersonalFriend ? "TRUE" : "FALSE", sbItems);

                if (friend.CRM_AnnualPass != null)
                {
                    AddComma(friend.CRM_AnnualPass.ExpiryDate.ToString("dd/MM/yyyy"), sbItems);
                    AddComma(friend.CRM_AnnualPass.TypeOfPass, sbItems);
                }
                else
                {
                    AddComma("NO PASS", sbItems);
                    AddComma("NO PASS", sbItems);
                }
                
                Response.Write(sbItems.ToString());
                Response.Write(Environment.NewLine);
                sbItems = new StringBuilder();
            }

            Response.End();
            

        }

        public static string FormatLine(string intput)
            {
                return "\"" + intput.Replace(",", "").Replace(System.Environment.NewLine, "").Replace("\t", "").Replace("\n", "").Replace("\r", "") + "\"" + ",";
            }
        

        public static void GenericExport(List<_DataTableColumn> Headers, IEnumerable<ListData> Data, HttpResponse response, string filename)
        {
            string columnNames = "";

            Headers = Headers.OrderBy(h => h.OrderNo).ToList();
            Data = Data.OrderBy(d => d.OrderNo);

            for (int i=0; i <= Headers.Count() - 1;i++)
            {
                columnNames += Headers[i]._DataFieldFriendly + (i + 1 == Headers.Count() ? "" : ",");
            }
       
            response.Clear();
            response.ContentType = "text/csv";
            response.AddHeader("content-disposition", "attachment; filename=\"" + filename + "-" + DateTime.UtcNow.ToString("dd-MM-yyyy") + ".csv\"");
            response.ContentEncoding = Encoding.GetEncoding("Windows-1252");

            HttpContext.Current.Response.Write(columnNames);
            HttpContext.Current.Response.Write(Environment.NewLine);

             StringBuilder sbItems = new StringBuilder();
             foreach (ListData item in Data)
             {
                 sbItems = new StringBuilder();
                 foreach (_DataTableColumn col in Headers)
                 {
                     string output = Custom.Eval(item.DataItem, col._DataFieldName).ToString();

                     if (col._DataFieldFriendly == "View")
                     {
                         output = "=HYPERLINK(\"" + Constants.DomainName + "" + DataBinder.Eval((item.DataItem), "DetailsURL") + "\", \"VIEW\")";
                     }
                     
                     AddComma(output, sbItems);
                 }

                 response.Write(sbItems.ToString());
                 response.Write(Environment.NewLine);
             }

             response.End();
        }

        public static void MemberAudit(IEnumerable<CRM_AnnualPass> passes, HttpResponse Response)
        {
 

            string columnNames = "Signup Date, Expiry Date, Membership Type,Name,Number,Amount Paid,Payment Method,Email";

            string filename = "audit";

            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment; filename=\"" + filename + "-" + DateTime.UtcNow.ToString("dd-MM-yyyy") + ".csv\"");
            Response.ContentEncoding = Encoding.GetEncoding("Windows-1252");

            HttpContext.Current.Response.Write(columnNames);
            HttpContext.Current.Response.Write(Environment.NewLine);


            StringBuilder sbItems = new StringBuilder();

            foreach (Code.Models.CRM_AnnualPass pass in passes)
            {

                CRM_AnnualPassPerson owner = pass.CRM_AnnualPassPersons.FirstOrDefault(r => r.CRM_Person.Reference == pass.PrimaryContactReference);

                AddComma(pass.StartDate.ToString("dd/MM/yyyy HH:mm"), sbItems);
                AddComma(pass.ExpiryDate.ToString("dd/MM/yyyy HH:mm"), sbItems);
                AddComma(pass.TypeOfPass, sbItems);
                AddComma(owner == null ? "Unknown" : owner.CRM_Person.Fullname, sbItems);
                AddComma(pass.CRM_AnnualPassCard.MembershipNumber.ToString(), sbItems);
                AddComma(pass.AmountPaid.ToString("N2"), sbItems);
                AddComma(pass.PaymentMethodOutput, sbItems);

                if (owner != null && owner.CRM_Person != null)
                    AddComma(owner.CRM_Person.PrimaryEmail, sbItems);
                else
                {
                    AddComma("", sbItems);
                }


                Response.Write(sbItems.ToString());
                Response.Write(Environment.NewLine);
                sbItems = new StringBuilder();
            }

            Response.End();
        }

        private static void AddComma(string value, StringBuilder stringBuilder)
        {
            bool mustQuote = (value.Contains(",") || value.Contains("\"") || value.Contains("\r") || value.Contains("\n"));

            StringBuilder sb = new StringBuilder();

            if (mustQuote)
            {

                sb.Append("\"");
                foreach (char nextChar in value)
                {
                    sb.Append(nextChar);
                    if (nextChar == '"')
                        sb.Append("\"");
                }
                sb.Append("\"");

            }
            else
            {
                sb = sb.Append(value);
            }

            stringBuilder.Append(sb.ToString());
            stringBuilder.Append(",");
        }

    }


}