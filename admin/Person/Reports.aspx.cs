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
using static CRM.Code.CSVExport;

namespace CRM.admin.Person
{
    public partial class Reports : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnExportExhibitionPostal.EventHandler = btnExportExhibitionPostal_Click;
            btnExportLearningContacts.EventHandler = btnExportLearningContacts_Click;
            btnExportByInterest.EventHandler = btnExportByInterest_Click;

            if (!Page.IsPostBack)
            {
                ddlInterest.DataSource = from p in db.CRM_FormFieldItems.Where(r => !r.IsArchived && r.IsActive && r.CRM_FormFieldID == 1)
                                         orderby p.OrderNo
                                         select p;
                ddlInterest.DataBind();
            }
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
                (int)CustomFieldIDs.Government

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



        protected void btnExportByInterest_Click(object sender, EventArgs e)
        {
            //var dataset = from p in db.CRM_Persons
            //              where !p.IsArchived
            //              let Responses = db.CRM_FormFieldResponses.Where(r => r.TargetReference == p.Reference && r.CRM_FormFieldID == 1)
            //              select new ExportByInterestHelper{
            //                  Person = p,
            //                  Responses = Responses
            //              };

            //List<CRM_FormFieldItem> items = db.CRM_FormFieldItems.Where(r => r.CRM_FormFieldID == 1).ToList(); // Constituents

            //CSVExport.ContactsByInterest(dataset.ToList(), items, Response);
            
            var dataset = from p in db.CRM_Persons
                          where db.CRM_FormFieldResponses.Any(r =>
                          r.CRM_FormFieldItemID != null && ddlInterest.SelectedValue == ((int)r.CRM_FormFieldItemID).ToString() &&
                              r.TargetReference == p.Reference)
                          let constituentOutput = p.ConstituentTypeOutput(db, " ")
                          where !p.IsArchived
                          select new
                          {

                              p.Title,
                              p.Firstname,
                              p.Lastname,
                              p.IsAnnualPassHolder,
                              p.PrimaryEmail,
                              p.CalculatedSalutation,
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
                              p.IsDoNotMail,
                              constituentOutput

                          };

            CSVExport.GenericExport(dataset.ToArray(), "byinterest-contacts");

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
                               ).ToList().DistinctOnRelations().DistinctOnNamePostcode()
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