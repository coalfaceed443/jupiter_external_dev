﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using CRM.Code.Models;
using System.Xml.Serialization;
using Service;
using Website.Code.Helpers.WebService;
using System.Web.Script.Serialization;
using CRM.Code.Utils.Enumeration;
using System.Runtime.Serialization;
using CRM.Code.Utils.Time;

namespace CRM.WebService
{
    /// <summary>
    /// Data submission and retrieval for CMS/External syncs.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CRMSync : System.Web.Services.WebService
    {

        private const string AuthKey = "yud9usa98xasdw2A8sy7d98as87da9s8ds";


        [WebMethod]
        public ContextResult<Service.Country[]> GetCountries(string authkey)
        {
            ServiceDataContext db = new ServiceDataContext();

            ContextResult<Service.Country[]> result = new ContextResult<Service.Country[]>() { ReturnObject = db.Countries.ToArray() };
            result.IsSuccess = IsAuthValid(authkey);

            SetResponseHeaders(result.IsSuccess);

            return result;
        }

        public class InterestAnswer
        {
            public bool OptIn { get; set; }
            public int FormFieldID { get; set; }
        }

        [WebMethod]
        public CRM.Code.Models.HoldingPen.Origins ExposeOrigins()
        {
            return Code.Models.HoldingPen.Origins.websitecheckout;
        }

        [WebMethod]
        public bool SendAccount(string key, Service.HoldingPen account, InterestAnswer[] interests)
        {
            ServiceDataContext db = new ServiceDataContext();

            JavaScriptSerializer js = new JavaScriptSerializer();

            account.InterestObjects = js.Serialize(interests);

            db.HoldingPens.InsertOnSubmit(account);
            db.SubmitChanges();

            return true;
        }


        [WebMethod]
        public bool CheckMemberNo(string key, string memberNo)
        {
            ServiceDataContext db = new ServiceDataContext();

            return db.CRM_AnnualPassCards.Any(c => c.MembershipNumber.ToString() == memberNo
                && c.CRM_AnnualPasses.Any(ap => ap.StartDate <= UKTime.Now
                    && ap.ExpiryDate >= UKTime.Now && !ap.IsArchived));
        }


        [WebMethod]
        public ContextResult<CRM_Title[]> GetTitles(string authkey)
        {
            MainDataContext db = new MainDataContext();
            var items = db.CRM_Titles.ToArray().OrderBy(c => c.DisplayName).ToArray();
            ContextResult<CRM_Title[]> result = new ContextResult<CRM_Title[]>() { ReturnObject = items };
            result.IsSuccess = IsAuthValid(authkey);

            SetResponseHeaders(result.IsSuccess);

            return result;
        }



        [WebMethod]
        public ContextResult<Service.CRM_RelationCode[]> GetRelationshipCodes(string authkey)
        {
            ServiceDataContext db = new ServiceDataContext();
            var items = db.CRM_RelationCodes.ToArray().OrderBy(c => c.Name).ToArray();
            ContextResult<Service.CRM_RelationCode[]> result = new ContextResult<Service.CRM_RelationCode[]>() { ReturnObject = items };
            result.IsSuccess = IsAuthValid(authkey);

            SetResponseHeaders(result.IsSuccess);

            return result;
        }

        [WebMethod]
        public ContextResult<Service.CRM_AnnualPassType[]> GetMemberships(string authkey)
        {
            ServiceDataContext db = new ServiceDataContext();
            var items = db.CRM_AnnualPassTypes.Where(r => r.IsWebsite && !r.IsArchived).ToArray().OrderBy(c => c.Name).ToArray();
            ContextResult<Service.CRM_AnnualPassType[]> result = new ContextResult<Service.CRM_AnnualPassType[]>() { ReturnObject = items };
            result.IsSuccess = IsAuthValid(authkey);

            SetResponseHeaders(result.IsSuccess);

            return result;
        }

        private int FieldsToShowID = 24; // interest 

        [WebMethod]
        public ContextResult<Service.CRM_FormFieldItem[]> GetInterests(string authkey)
        {

            ServiceDataContext db = new ServiceDataContext();

            Service.CRM_FormFieldItem[] items = db.CRM_FormFieldItems.Where(f => f.CRM_FormFieldID == 24).OrderBy(o => o.OrderNo).ToArray();


            ContextResult<Service.CRM_FormFieldItem[]> result = new ContextResult<Service.CRM_FormFieldItem[]>() { ReturnObject = items };

            result.IsSuccess = IsAuthValid(authkey);


            SetResponseHeaders(result.IsSuccess);

            return result;

        }

        [WebMethod]
        public ContextResult<InterestAnswer[]> GetCurrentInterests(string authkey, int WebsiteAccountID)
        {

            ServiceDataContext db = new ServiceDataContext();

            Service.CRM_Person personAccount = db.CRM_Persons.FirstOrDefault(s => s.WebsiteAccountID == WebsiteAccountID);

            InterestAnswer[] items = (from a in db.CRM_FormFieldAnswers
                                      where a.CRM_FormFieldID == 24 && a.TargetReference == personAccount.Reference
                                      select new InterestAnswer { FormFieldID = a.CRM_FormFieldID, OptIn = a.Answer == "Yes" }).ToArray();

            ContextResult<InterestAnswer[]> result = new ContextResult<InterestAnswer[]>() { ReturnObject = items };
            result.IsSuccess = IsAuthValid(authkey);

            SetResponseHeaders(result.IsSuccess);

            return result;
        }

        [WebMethod]
        public ContextResult<Service.CRM_Person> GetPersonRecord(string authkey, int WebsiteAccountID)
        {

            ServiceDataContext db = new ServiceDataContext();

            Service.CRM_Person personAccount = db.CRM_Persons.FirstOrDefault(s => s.WebsiteAccountID == WebsiteAccountID);

            ContextResult<Service.CRM_Person> result = new ContextResult<Service.CRM_Person>() { ReturnObject = personAccount };
            result.IsSuccess = IsAuthValid(authkey);

            SetResponseHeaders(result.IsSuccess);

            return result;
        }

        private bool IsAuthValid(string key)
        {
            return key == AuthKey;
        }


        private void SetResponseHeaders(bool success)
        {
            HttpContext.Current.Response.StatusCode = success ? 200 : 500;
        }


    }
}
