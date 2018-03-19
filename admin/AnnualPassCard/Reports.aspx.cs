using CRM.Code;
using CRM.Code.BasePages.Admin;
using CRM.Code.Helpers;
using CRM.Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Extensions;

namespace CRM.admin.AnnualPassCard
{

    public partial class Reports : AdminPage
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            btnExportAudit.EventHandler = btnExportAudit_Click;

            btnActiveFriends.EventHandler = btnActiveFriends_Click;
        }

        protected void btnActiveFriends_Click(object sender, EventArgs e)
        {
            int Friend = 1;
            int PersonalFriend = 2;

            int days = 30;

            Int32.TryParse(txtDays.Text, out days);

            var members = (from p in db.CRM_AnnualPasses
                          let FormFieldResponses = db.CRM_FormFieldResponses.Where(r => r.TargetReference == p.PrimaryContactReference)
                          let IsFriendByConstituent = (FormFieldResponses.Where(r => r.CRM_FormFieldItemID == Friend))
                          let IsPersonalFriend = (FormFieldResponses.Where(r => r.CRM_FormFieldItemID == PersonalFriend))
                          let Person = db.CRM_Persons.First(r => r.Reference == p.PrimaryContactReference)
                          where p.ExpiryDate >= DateTime.Now.Date.AddDays(-days)
                          where p.StartDate <= DateTime.Now.Date
                          where Person.IsDeceased == false
                          where Person.IsDoNotMail == false
                          where Person.IsArchived == false

                          select new CRM.Code.Helpers.FriendReportHelper() {
                              CRM_AnnualPass = p,
                              IsFriend = IsFriendByConstituent.Any(),
                              IsPersonalFriend = IsPersonalFriend.Any(),
                              CRM_Person = Person
                          }).ToList();


            members = members.ToList();
            var PersonalFriendsWithoutPasses = (from p in db.CRM_Persons
                                               let FormFieldResponses = db.CRM_FormFieldResponses.Where(r => r.TargetReference == p.Reference)
                                               let IsFriendByConstituent = (FormFieldResponses.Where(r => r.CRM_FormFieldItemID == Friend))
                                               let IsPersonalFriend = (FormFieldResponses.Where(r => r.CRM_FormFieldItemID == PersonalFriend))
                                               where p.IsDoNotMail == false
                                                where p.IsDeceased == false
                                                where IsPersonalFriend.Any()
                                                where p.IsArchived == false
                                                select new CRM.Code.Helpers.FriendReportHelper()
                                               {
                                                   CRM_AnnualPass = null,
                                                   IsFriend = IsFriendByConstituent.Any(),
                                                   IsPersonalFriend = IsPersonalFriend.Any(),
                                                   CRM_Person = p
                                               }).ToList();


            PersonalFriendsWithoutPasses = (from p in PersonalFriendsWithoutPasses
                                           where !members.Any(m => m.CRM_Person.Reference == p.CRM_Person.Reference)
                                           select p).ToList();

            members = members.Concat(PersonalFriendsWithoutPasses).Where(r => r.CRM_Person.Address1 != "").ToList();


            if (chkExhibition.Checked)
            {
                var constituents = ExhibitionPostals();

                var dataset = from p in (from p in db.CRM_Persons
                                         let FormFieldResponses = db.CRM_FormFieldResponses.Where(r => r.TargetReference == p.Reference)
                                         let IsFriendByConstituent = (FormFieldResponses.Where(r => r.CRM_FormFieldItemID == Friend))
                                         let IsPersonalFriend = (FormFieldResponses.Where(r => r.CRM_FormFieldItemID == PersonalFriend))
                                         where !p.IsArchived
                                         where db.CRM_FormFieldResponses.Any(r =>
                                             r.CRM_FormFieldItemID != null &&
                                             constituents.Contains((int)r.CRM_FormFieldItemID) &&
                                             r.TargetReference == p.Reference)
                                         where !p.IsDoNotMail
                                         select new CRM.Code.Helpers.FriendReportHelper()
                                         {
                                             CRM_AnnualPass = null,
                                             IsFriend = IsFriendByConstituent.Any(),
                                             IsPersonalFriend = IsPersonalFriend.Any(),
                                             CRM_Person = p
                                         }).ToList()
                    select p;


                members = members.Concat(dataset).ToList();

            }




            IEnumerable<IGrouping<int?, FriendReportHelper>> groupset = members.GroupBy(g => g.CRM_Person.RelationshipID);

            var recordsWithRelation = groupset.Where(c => c.Key != null).Select(c => c.First());
            var recordsWithoutRelation = groupset.Where(c => c.Key == null).SelectMany(c => c);
            members = recordsWithRelation.Concat(recordsWithoutRelation).ToList();
            
            
            IEnumerable<IGrouping<string, FriendReportHelper>> addressGroupset = members.GroupBy(g => g.CRM_Person.CRM_Address.FormattedAddress);

            var recordsWithAdress = addressGroupset.Where(c => c.Key != null).Select(c => c.First());
            var recordsWithoutAddress = addressGroupset.Where(c => c.Key == null).SelectMany(c => c);
            members = recordsWithAdress.Concat(recordsWithoutAddress).ToList();

            IEnumerable<IGrouping<string, FriendReportHelper>> fullnamePostcode = members.GroupBy(g => g.CRM_Person.CRM_Address.Postcode + g.CRM_Person.Fullname);

            members = fullnamePostcode.Select(c => c.First()).ToList();


            CSVExport.ActiveFriendsByConstituent(members, Response);


        }

        protected void btnExportAudit_Click(object sender, EventArgs e)
        {
            
            var startDate = new DateTime(DateTime.Now.Year - 1, 01, 01);
            var endDate = new DateTime(DateTime.Now.Year -1, 12, 31);

            var members = from p in db.CRM_AnnualPasses
                where p.StartDate >= startDate
                where p.StartDate <= endDate
                where !p.IsArchived
                select p;

            CSVExport.MemberAudit(members, Response);


        }

    }
}