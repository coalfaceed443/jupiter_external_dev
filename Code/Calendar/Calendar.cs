using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;
using CRM.Code.Utils.Enumeration;

namespace CRM.Code.Calendar
{
    public class Calendar
    {
        private MainDataContext db;

        public Calendar(MainDataContext db)
        {
            this.db = db;
        }

        public List<DateTime> GetDaysInMonth(int month, int year)
        {
            DateTime startOfMonth = new DateTime(year, month, 1);

            // hack - why does DateTime not have a copy constructor??        
            DateTime walkDay = new DateTime(startOfMonth.Ticks);

            // if it's already a monday, we'll start return days from 
            // exactly a week before the month begins
            if (walkDay.DayOfWeek == DayOfWeek.Monday)
            {
                walkDay = walkDay.AddDays(-7);
            }
            // otherwise find the nearest Monday by walking backwards
            else while (walkDay.DayOfWeek != DayOfWeek.Monday)
                {
                    walkDay = walkDay.AddDays(-1);
                }

            List<DateTime> dayList = new List<DateTime>();

            // now simply add days starting from the monday we found closest
            // to the beginning of the month to our dayList
            for (int i = 0; i < 7 * 6; i++)
            {
                dayList.Add(walkDay);
                walkDay = walkDay.AddDays(1);
            }

            return dayList;
        }
    }
}
