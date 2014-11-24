using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;
using CRM.Code.Calendar;
using CRM.Code.Interfaces;
using CRM.Code.BasePages.Admin;

namespace CRM.Code.Managers
{
    public class CalendarManager
    {
        private MainDataContext db;

        public CalendarManager()
        {
            db = new MainDataContext();
        }

        public int IndexOfEventAtTime(CRM_Calendar ItemToCheck, DateTime StartDateTime, DateTime EndDateTime)
        {
            return ItemsDuringThisTime(StartDateTime).IndexOf(ItemToCheck);
        }

        public List<CRM_Calendar> ItemsDuringThisTime(DateTime StartDateTime)
        {
            var AllEventsInThisTime = (from c in db.CRM_Calendars
                                       where c.StartDateTime <= StartDateTime
                                       where c.EndDateTime >= StartDateTime
                                       orderby c.ID
                                       select c).ToList();

            return AllEventsInThisTime;
        }

        public class Clash
        {
            public IClash ItemOne { get; set; }
            public IClash ItemTwo { get; set; }

            public Clash(object v1, object v2)
            {
                ItemOne = (IClash)v1;
                ItemTwo = (IClash)v2;

                string colour = CreateRandomColour(this.GetHashCode());
                ItemOne.ClashColourID = colour;
                ItemTwo.ClashColourID = colour;
            }
        }

        public static string CreateRandomColour(int seed)
        {
            var random = new Random(seed);
            var colour = String.Format("#{0:X6}", random.Next(0x1000000));
            return colour;
        }

        public static IEnumerable<Clash> GetOverlappedTimes<T>(IEnumerable<T> list, Func<T, bool> filter, Func<T, DateTime> start, Func<T, DateTime> end)
        {
            // Selects all records that match filter() on left side and returns all records on right side that overlap.
            var overlap = from t1 in list
                          where filter(t1)
                          from t2 in list
                          where !object.Equals(t1, t2) // Don’t match the same object on right side.
                          let in1 = start(t1)
                          let out1 = end(t1)
                          let in2 = start(t2)
                          let out2 = end(t2)
                          where in1 <= out2 && out1 >= in2
                          select new Clash(t1, t2);
            return overlap;
        }

        public IEnumerable<CRM_Calendar> EventsPerDay(DateTime day, bool HideExternal, bool HideInternal, bool HideNonTagged, int type, int? admincalendar)
        {

            var baseSet = (from c in db.CRM_Calendars
                           where c.StartDateTime >= day.Date && c.StartDateTime < day.Date.AddHours(24).AddSeconds(-1)
                           orderby c.ID
                           select c).ToArray();

            var unparsed = baseSet;

            baseSet = QueryBasetSet(baseSet, HideExternal, HideInternal, HideNonTagged, type, admincalendar, out unparsed);

            return baseSet;
        }

        public CRM_Calendar[] QueryBasetSet(CRM_Calendar[] baseSet, bool HideExternal, bool HideInternal, bool HideNonTagged, int type, int? adminCalendar, out CRM_Calendar[] unparsed)
        {
            int currentAdmin = ((AdminPage)HttpContext.Current.Handler).AdminUser.ID;

            if (adminCalendar != null)
            {
                baseSet = (from b in baseSet
                           where b.IsInvolvedWith((int)adminCalendar, currentAdmin)
                           select b
                               ).ToArray();
            }
            else
            {
                baseSet = (from b in baseSet
                           where b.PrivacyStatus != (byte)CRM_Calendar.PrivacyTypes.Private || (b.CreatedByAdminID == currentAdmin && b.PrivacyStatus == (byte)CRM_Calendar.PrivacyTypes.Private)
                           select b).ToArray();
            }

            unparsed = baseSet;

            if (HideExternal)
            {
                baseSet = (from b in baseSet
                           where b.CRM_CalendarVenues.All(v => v.CRM_Venue.IsInternal)
                           select b).ToArray();
            }

            if (HideInternal)
            {
                baseSet = (from b in baseSet
                           where b.CRM_CalendarVenues.All(v => !v.CRM_Venue.IsInternal)
                           select b).ToArray();
            }


            if (type != -1)
            {
                baseSet = (from b in baseSet
                           where b.CRM_CalendarType.FixedRef == type
                           select b).ToArray();

            }


            if (HideNonTagged)
            {
                baseSet = (from b in baseSet
                           where b.CRM_CalendarAdmins.Any(c => c.AdminID == currentAdmin)
                           select b).ToArray();
            }

            return baseSet;
        }

        public IEnumerable<CalendarSlot>FetchSlotsForTime(DateTime StartDateTime, bool HideExternal, bool HideInternal, bool HideNonTagged, int type, int? adminCalendar)
        {
            var baseSet = (from c in db.CRM_Calendars
                    where c.StartDateTime >= StartDateTime && c.StartDateTime < StartDateTime.AddHours(1)
                    orderby c.ID
                    select c).ToArray();

            var unparsed = baseSet;

            baseSet = QueryBasetSet(baseSet, HideExternal, HideInternal, HideNonTagged, type, adminCalendar, out unparsed);
            
            var slotsAtThisStart = (from c in baseSet
                    select new CalendarSlot(c.StartDateTime.ToString("dd/MM/yyyy HH:mm"))
                    {
                        CalendarItem = c,
                        InsideFilter = baseSet.Contains(c)
                    }).ToList();


            
            int otherItemsInThisSlot = ItemsDuringThisTime(StartDateTime).Count();

            List<CalendarSlot> additionalSlots = new List<CalendarSlot>();

            int populatedSlots = slotsAtThisStart.Count();

            CalendarSlot lastslot = new CalendarSlot(StartDateTime.ToString("dd/MM/yyyy HH:mm"), true)
            {
                CalendarItem = null
            };
            additionalSlots.Add(lastslot);

            while (populatedSlots <= otherItemsInThisSlot)
            {
                
                CalendarSlot slot = new CalendarSlot(StartDateTime.ToString("dd/MM/yyyy HH:mm"))
                {
                    CalendarItem = null,
                    Visible = false
                };
                additionalSlots.Add(slot);

                populatedSlots++;
            }



            additionalSlots.AddRange(slotsAtThisStart);

            return additionalSlots.AsEnumerable();
            
        }
    }
}