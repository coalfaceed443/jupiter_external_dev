using System;
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
using System.Diagnostics;
using CRM.Code.Managers;

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
        public ContextResult<Service.CRM_Venue[]> GetVenues(string authkey)
        {
            ServiceDataContext db = new ServiceDataContext();

            var venueList = new List<Service.CRM_Venue>();

            foreach (var source in db.CRM_Venues.ToArray())
            {
                venueList.Add(new Service.CRM_Venue()
                {
                   Name = source.Name,
                   ID = source.ID
                });
            }

            ContextResult<Service.CRM_Venue[]> result = new ContextResult<Service.CRM_Venue[]>() { ReturnObject = venueList.ToArray() };
            result.IsSuccess = IsAuthValid(authkey);

            SetResponseHeaders(result.IsSuccess);

            return result;
        }

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

            if (account.Password == null)
            {
                account.Password = "";
            }

            db.HoldingPens.InsertOnSubmit(account);
            db.SubmitChanges();

            return true;
        }

        private bool IsFriend(string email)
        {
            ServiceDataContext db = new ServiceDataContext();

            var personsMatchingEmail = db.CRM_Persons.Where(r => r.PrimaryEmail.ToLower().Trim() == email.ToLower().Trim());

            bool hasPass = false;

            foreach (Service.CRM_Person person in personsMatchingEmail)
            {
                bool HasPass = db.CRM_AnnualPasses.Any(r => r.StartDate >= UKTime.Now && r.ExpiryDate <= UKTime.Now && r.PrimaryContactReference == person.Reference);

                if (HasPass)
                    hasPass = true;
            }

            return hasPass;

        }

        [WebMethod]
        public ContextResult<List<string>> ActiveFriendEmails(string authKey)
        {

            bool valid = IsAuthValid(authKey);

            List<string> ActiveFriends = new List<string>();

            if (valid)
            {
                var now = UKTime.Now;

                using (ServiceDataContext db = new ServiceDataContext())
                {
                    foreach (Service.CRM_AnnualPass pass in db.CRM_AnnualPasses.Where(r => r.StartDate <= now && r.ExpiryDate >= now))
                    {
                        Service.CRM_Person person = db.CRM_Persons.FirstOrDefault(f => f.Reference == pass.PrimaryContactReference);

                        if (person != null && person.PrimaryEmail != "")
                            ActiveFriends.Add(person.PrimaryEmail.ToLower().Trim());
                    }
                }
            }

            ContextResult<List<string>> result = new ContextResult<List<string>>() { ReturnObject = ActiveFriends };
            result.IsSuccess = valid;

            SetResponseHeaders(result.IsSuccess);

            return result;
            
        }


        [WebMethod]
        public string SyncCalendar(int ExternalEventID, string Name, DateTime startDate, int attendees, int total, bool UpdateAttendance, int Adults, int children, int altChildren, int concessions, int student)
        {

            try
            {

                ServiceDataContext db = new ServiceDataContext();

                Service.CRM_Calendar calendarItem = db.CRM_Calendars.FirstOrDefault(f => f.ExternalEventID == ExternalEventID && startDate.Date == f.StartDateTime.Date);


                int CalendarTypeID = db.CRM_CalendarTypes.Single(s => s.FixedRef == (int)Code.Models.CRM_CalendarType.TypeList.Event).ID;

                bool addedNew = false;
                if (calendarItem == null)
                {
                    addedNew = true;

                    calendarItem = new Service.CRM_Calendar();
                    calendarItem.CancellationReason = "";
                    calendarItem.CreatedByAdminID = 1;
                    calendarItem.CRM_CalendarTypeID = CalendarTypeID;
                    calendarItem.DateCancelled = null;
                    calendarItem.DatePaid = null;
                    calendarItem.Status = (byte)Code.Models.CRM_Calendar.StatusTypes.Confirmed;
                    calendarItem.ExternalEventID = ExternalEventID;
                    calendarItem.InvoiceAddressID = null;
                    calendarItem.InvoiceFirstname = "";
                    calendarItem.InvoiceLastname = "";
                    calendarItem.InvoiceTitle = "";
                    calendarItem.IsCancelled = false;
                    calendarItem.IsInvoiced = false;
                    calendarItem.PONumber = "";
                    calendarItem.PriceAgreed = 0;
                    calendarItem.PriceType = 0;
                    calendarItem.PrimaryContactReference = "";
                    calendarItem.PrivacyStatus = (byte)Code.Models.CRM_Calendar.PrivacyTypes.Editable;
                    calendarItem.RequiresCatering = false;
                    calendarItem.TargetReference = "";


                    if (ExternalEventID == 91)
                    {
                        calendarItem.CRM_CalendarTypeID = db.CRM_CalendarTypes.Single(s => s.FixedRef == (int)Code.Models.CRM_CalendarType.TypeList.generic).ID;

                        calendarItem.StartDateTime = startDate.AddHours(9);
                        calendarItem.EndDateTime = startDate.AddHours(17);
                    }
                    else
                    {
                        if (startDate.Hour == 0)
                        {
                            calendarItem.StartDateTime = startDate.AddHours(9);
                            calendarItem.EndDateTime = startDate.AddHours(10);
                        }
                        else
                        {
                            calendarItem.StartDateTime = startDate;
                            calendarItem.EndDateTime = startDate.AddHours(1);
                        }
                    }

                    db.CRM_Calendars.InsertOnSubmit(calendarItem);
                }

                if (!UpdateAttendance|| addedNew)
                {
                    calendarItem.Taken = attendees;
                    calendarItem.Limit = total;
                }
                calendarItem.DisplayName = Name;

                db.SubmitChanges();


                if (UpdateAttendance)
                {

                    Service.CRM_AttendanceEvent attendanceEvent = db.CRM_AttendanceEvents.FirstOrDefault(f => f.ExternalEventID == ExternalEventID);


                    if (attendanceEvent == null)
                    {
                        attendanceEvent = new Service.CRM_AttendanceEvent()
                        {
                            ExternalEventID = ExternalEventID,
                            Name = Name
                        };

                        db.CRM_AttendanceEvents.InsertOnSubmit(attendanceEvent);
                        db.SubmitChanges();
                    }

                    List<string> ticketTypes = new List<string>()
                    {
                        "Adults",
                        "Children",
                        "Alt Children",
                        "Concessions",
                        "Student"
                    };

                    
                    Service.CRM_AttendanceLogGroup group = new Service.CRM_AttendanceLogGroup();

                    group.CRM_AttendanceEventID = attendanceEvent.ID;
                    group.AddedTimeStamp = startDate;
                    group.OriginType = (byte)CRM.Code.Models.CRM_AttendanceLogGroup.OriginTypes.WebsiteWebservice;
                    group.DateInserted = UKTime.Now;
                    db.CRM_AttendanceLogGroups.InsertOnSubmit(group);
                    db.SubmitChanges();

                    foreach (string ticketType in ticketTypes)
                    {
                        int attendanceNumber = 0;
                        switch (ticketType)
                        {
                            case "Adults":
                                attendanceNumber = Adults;
                                break;
                            case "Children":
                                attendanceNumber = children;
                                break;
                            case "Alt Children":
                                attendanceNumber = altChildren;
                                break;
                            case "Concessions":
                                attendanceNumber = concessions;
                                break;
                            case "Student":
                                attendanceNumber = student;
                                break;
                        }

                        

                        if (attendanceNumber > 0)
                        {

                            string logticketType = "Web Booking - " + ticketType;

                            Service.CRM_AttendancePersonType personType = db.CRM_AttendancePersonTypes.FirstOrDefault(f => f.Name == logticketType);

                            if (personType == null)
                            {
                                personType = new Service.CRM_AttendancePersonType();
                                personType.IsActive = true;
                                personType.IsArchived = false;
                                personType.Name = logticketType;
                                personType.OrderNo = 0;
                                db.CRM_AttendancePersonTypes.InsertOnSubmit(personType);
                            }

                            db.SubmitChanges();

                            if (attendanceEvent == null)
                            {
                                attendanceEvent = new Service.CRM_AttendanceEvent();
                                attendanceEvent.Name = Name;
                                attendanceEvent.ExternalEventID = ExternalEventID;
                                db.CRM_AttendanceEvents.InsertOnSubmit(attendanceEvent);
                            }

                            db.SubmitChanges();



                            Service.CRM_AttendanceLog log = new Service.CRM_AttendanceLog();
                            log.CRM_AttendancePersonTypeID = personType.ID;
                            log.Quantity = attendanceNumber;
                            log.CRM_CRM_AttendanceLogGroupID = group.ID;
                            db.CRM_AttendanceLogs.InsertOnSubmit(log);
                            db.SubmitChanges();
                        }
                    }

                }
                return "OK";
            }
            catch(Exception ex) {


                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();

                return "External Event ID " + ExternalEventID + " : " + ex.Message + ":" + line;
            }
        }

        /// <summary>
        /// Used for joint memberships
        /// </summary>
        /// <param name="key"></param>
        /// <param name="account"></param>
        /// <param name="interests"></param>
        /// <returns>ID - The ID of the submitted holding pen</returns>
        [WebMethod]
        public int SendJointAccount(string key, Service.HoldingPen account, InterestAnswer[] interests)
        {
            ServiceDataContext db = new ServiceDataContext();

            JavaScriptSerializer js = new JavaScriptSerializer();

            account.InterestObjects = js.Serialize(interests);

            if (account.Password == null)
            {
                account.Password = "";
            }

            db.HoldingPens.InsertOnSubmit(account);
            db.SubmitChanges();

            return account.ID;
        }

        /// <summary>
        /// Used to link up two holding pens which have been joint by a membership
        /// </summary>
        /// <param name="key"></param>
        /// <param name="account"></param>
        /// <param name="otherID">The 'other' holding pen to be joined to this account</param>
        [WebMethod]
        public void UpdateJointID(string key, Service.HoldingPen account, int otherID)
        {
            ServiceDataContext db = new ServiceDataContext();
            account.JointHoldingPenID = otherID;           
            db.SubmitChanges();
        }

        [WebMethod]
        public bool CheckUserLogin(string key, string email, string password)
        {
            if (!IsAuthValid(key))
            {
                return false;
            }

            ServiceDataContext db = new ServiceDataContext();

            email = email.ToLower();

            var result = db.CRM_Persons.ToList().FirstOrDefault(c => !c.IsArchived && c.PrimaryEmail.ToLower() == email && c.Password == password);

            return result != null;
        }

        [WebMethod]
        public ContextResult<Service.CRM_Person> GetUserDetails(string key, string email, string password)
        {
            ServiceDataContext db = new ServiceDataContext();

            email = email.ToLower();

            var person = db.CRM_Persons.ToList().FirstOrDefault(c => !c.IsArchived && c.PrimaryEmail.ToLower() == email && c.Password == password);

            var tempPerson = new Service.CRM_Person()
            {
                PrimaryEmail = person.PrimaryEmail,
                Title = person.Title,
                Firstname = person.Firstname,
                Lastname = person.Lastname,
                PrimaryAddressID = person.PrimaryAddressID,
            };

            ContextResult<Service.CRM_Person> result = new ContextResult<Service.CRM_Person>() { ReturnObject = tempPerson };
            result.IsSuccess = IsAuthValid(key);

            SetResponseHeaders(result.IsSuccess);

            return result;
        }

        [WebMethod]
        public bool HasActiveMembership(string key, string email)
        {
            if (!IsAuthValid(key))
            {
                return false;
            }

            ServiceDataContext db = new ServiceDataContext();

            var person = db.CRM_Persons.ToList().FirstOrDefault(c => !c.IsArchived && c.PrimaryEmail.ToLower() == email);

            if (person == null)
            {
                return false;
            }

            return person.CRM_AnnualPassPersons.Any(c => !c.IsArchived && !c.CRM_AnnualPass.IsArchived && c.CRM_AnnualPass.ExpiryDate >= DateTime.Today);
        }

        [WebMethod]
        public ContextResult<Service.CRM_Address> GetAddress(string key, int addressID)
        {
            ServiceDataContext db = new ServiceDataContext();

            var address = db.CRM_Addresses.ToList().FirstOrDefault(c => c.ID == addressID);

            ContextResult<Service.CRM_Address> result = new ContextResult<Service.CRM_Address>()
            {
                ReturnObject = new Service.CRM_Address()
                {
                    AddressLine1 = address.AddressLine1,
                    AddressLine2 = address.AddressLine3,
                    AddressLine3 = address.AddressLine3,
                    AddressLine4 = address.AddressLine4,
                    AddressLine5 = address.AddressLine5,
                    Town = address.Town,
                    County = address.County,
                    Postcode = address.Postcode
                }
            };
            result.IsSuccess = IsAuthValid(key);

            SetResponseHeaders(result.IsSuccess);

            return result;
        }

        [WebMethod]
        public bool SendPasswordReset(string key, string email)
        {
            if (!IsAuthValid(key))
            {
                return false;
            }

            ServiceDataContext db = new ServiceDataContext();

            email = email.ToLower();

            var result = db.CRM_Persons.ToList().FirstOrDefault(c => !c.IsArchived && c.PrimaryEmail.ToLower() == email);

            if (result != null)
            {
                // reset code //

                result.TempCode = Code.Models.CRM_Person.GeneratePassword();

                db.SubmitChanges();

                // send email //

                var emailManager = new EmailManager(result.PrimaryEmail);

                emailManager.SendPasswordReset(result.Firstname, result.PrimaryEmail, result.TempCode);
            }

            return true;
        }

        [WebMethod]
        public bool ResetCodeValid(string key, string email, string code)
        {
            if (!IsAuthValid(key))
            {
                return false;
            }

            ServiceDataContext db = new ServiceDataContext();

            email = email.ToLower();

            var result = db.CRM_Persons.ToList().FirstOrDefault(c => !c.IsArchived && c.PrimaryEmail.ToLower() == email && c.TempCode == code);

            if (result != null)
            {
                return true;
            }

            return false;
        }

        [WebMethod]
        public bool UpdatePassword(string key, string email, string password)
        {
            if (!IsAuthValid(key))
            {
                return false;
            }

            ServiceDataContext db = new ServiceDataContext();

            email = email.ToLower();

            var result = db.CRM_Persons.ToList().FirstOrDefault(c => !c.IsArchived && c.PrimaryEmail.ToLower() == email);

            if (result != null)
            {
                result.Password = password;
                db.SubmitChanges();
            }

            return true;
        }

        [WebMethod]
        public byte CheckUserExists(string key, string email)
        {
            if (!IsAuthValid(key))
            {
                return 0;
            }

            ServiceDataContext db = new ServiceDataContext();

            email = email.ToLower();

            var result = db.CRM_Persons.ToList().FirstOrDefault(c => !c.IsArchived && c.PrimaryEmail.ToLower() == email);

            if (result != null)
            {
                if (result.Password.Length == 0)
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }

            return 0;
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

