﻿using CRM.Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Scheduled
{
    /// <summary>
    /// Summary description for RebuildTempMembers
    /// </summary>
    public class RebuildTempMembers : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            MainDataContext db = new MainDataContext();

            db.Cardpressos.DeleteAllOnSubmit(db.Cardpressos);
            db.SubmitChanges();

            db.ExecuteCommand("DBCC CHECKIDENT('Cardpresso', RESEED, 0);");
            db.SubmitChanges();

            var activeMemberships = from p in (from p in db.CRM_AnnualPasses
                                    where p.ExpiryDate >= DateTime.Now.Date
                                    where !p.IsArchived
                                    select p).ToList()
                                    where p.IsCurrent
                                    let CRM_Person = db.CRM_Persons.SingleOrDefault(s => s.Reference == p.PrimaryContactReference)
                                    where p.CRM_AnnualPassType.IsWebsite
                                    where CRM_Person != null
                                    select new Cardpresso()
                                    {
                                        Barcode = p.CRM_AnnualPassCard.MembershipNumber.ToString(),
                                        Expiry = p.ExpiryDate,
                                        Membership = p.CRM_AnnualPassType.Name,
                                        Name = CRM_Person.RelationshipSaltuation
                                    };

            db.Cardpressos.InsertAllOnSubmit(activeMemberships);
            db.SubmitChanges();

            context.Response.Write("DONE - " + activeMemberships.Count() + " records");
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