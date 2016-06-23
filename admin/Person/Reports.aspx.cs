using CRM.Code;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Extensions;

namespace CRM.admin.Person
{
    public partial class Reports : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnExportExhibitionPostal.EventHandler = btnExportExhibitionPostal_Click;
            btnExportLearningContacts.EventHandler = btnExportLearningContacts_Click;
        }

        protected enum CustomFieldIDs
        {
            Friend = 1,
            PersonalFriend = 2,
            FreelanceTeacher = 3,
            Press = 4,
            Museums = 5,
            schools = 6,
            Supplier,
            Council,
            Galleries,
            Artist,
            Festivals,
            PotentialProblem,
            Government,
            Educational,
            Opinion,
            VIP,
            Staff,
            CourseAttendee,
            Volunteer,
            OnlineCustomer,
            Charity
        }

        protected List<int> ExhibitionPostals()
        {
            List<int> constituents = new List<int>()
            {
                (int)CustomFieldIDs.PersonalFriend,
                (int)CustomFieldIDs.Press,
                (int)CustomFieldIDs.Museums,
                (int)CustomFieldIDs.Council,
                (int)CustomFieldIDs.Galleries,
                (int)CustomFieldIDs.Artist,
                (int)CustomFieldIDs.Festivals,
                (int)CustomFieldIDs.Government,
                (int)CustomFieldIDs.Volunteer,

            };

            return constituents;
        }


        protected List<int> Learning()
        {
            List<int> constituents = new List<int>()
            {
                (int)CustomFieldIDs.CourseAttendee,
                (int)CustomFieldIDs.schools,
                (int)CustomFieldIDs.FreelanceTeacher,
                (int)CustomFieldIDs.Educational
            };

            return constituents;
        }

        protected void btnExportLearningContacts_Click(object sender, EventArgs e)
        {
            var constituents = Learning();

            var dataset = from p in db.CRM_Persons
                          where db.CRM_FormFieldResponses.Any(r =>
                          r.CRM_FormFieldItemID != null && constituents.Contains((int)r.CRM_FormFieldItemID) &&
                              r.TargetReference == p.Reference)
                              let constituentOutput = p.ConstituentTypeOutput(db, " ")
                          where !p.IsArchived
                          select new { 
                          
                              p.Title,
                              p.Firstname,
                              p.Lastname,
                              p.IsAnnualPassHolder,
                              p.PrimaryEmail,
                              p.CalculatedSalutation,
                              constituentOutput,
                              p.PrimaryAddressLine1,
                              p.PrimaryAddressLine2,
                              p.PrimaryAddressLine3,
                              p.PrimaryAddressLine4,
                              p.PrimaryAddressLine5,
                              p.PrimaryAddressTown,
                              p.PrimaryAddressCounty,
                              p.PrimaryAddressPostcode,
                             p.PrimaryAddressCountry,
                             p.NextExpiryDate,
                             p.IsDoNotEmail,
                             p.IsDoNotMail

                          };

            CSVExport.GenericExport(dataset.ToArray(), "learning-contacts");



        }

        protected void btnExportExhibitionPostal_Click(object sender, EventArgs e)
        {
            var constituents = ExhibitionPostals();

            var dataset = from p in(from p in db.CRM_Persons
                                     where !p.IsArchived
                          where db.CRM_FormFieldResponses.Any(r =>
                              r.CRM_FormFieldItemID != null &&
                              constituents.Contains((int)r.CRM_FormFieldItemID) &&
                              r.TargetReference == p.Reference)
                                     where !p.IsDoNotMail
                                select p
                               ).ToList().DistinctOnRelations()
                          where !p.IsAnnualPassHolder
                          let ConstituentType = p.ConstituentTypeOutput(db, ", ")
                          select new {
                          ConstituentType,
                          p.RelationshipSaltuation,
                          p.Title,
                          p.Firstname,
                          p.Lastname,
                          p.OrganisationRole,
                          p.OrganisationName,
                          p.PrimaryAddressLine1,
                          p.PrimaryAddressLine2,
                          p.PrimaryAddressLine3,
                          p.PrimaryAddressCounty,
                          p.PrimaryAddressPostcode,
                          p.PrimaryAddressCountry,
                          p.PrimaryEmail
                          
                          };



            CSVExport.GenericExport(dataset.ToArray(), "exhibition-postals");


        }

    }
}