using CRM.Code;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM.admin.Person
{
    public partial class Reports : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnExportExhibitionPostal.EventHandler = btnExportExhibitionPostal_Click;
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

        protected void btnExportExhibitionPostal_Click(object sender, EventArgs e)
        {
            var constituents = ExhibitionPostals();

            var dataset = from p in (from p in db.CRM_Persons
                          where db.CRM_FormFieldResponses.Any(r =>
                              r.CRM_FormFieldItemID != null &&
                              constituents.Contains((int)r.CRM_FormFieldItemID) &&
                              r.TargetReference == p.Reference)
                                     where !p.IsDoNotMail
                                select p
                               ).ToList()
                          where !p.IsAnnualPassHolder
                          select new { 
                          
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