﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Helper methods for dealing with UK time
/// </summary>
/// 

namespace CRM.Code.Utils.Time
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }
    }

    public class UKTime
    {
        /// <summary>
        /// ...the period beginning at one o'clock, Greenwich mean time, in the 
        /// morning of the last Sunday in March and ending at one o'clock, 
        /// Greenwich mean time, in the morning of the last Sunday in October.
        /// —The Summer Time Order 2002[1]
        /// </summary>    
        static public bool IsBritishSummerTime(DateTime date)
        {
            int year = date.Year;
            DateTime start = new DateTime(year, 3, 30);
            DateTime end = new DateTime(year, 10, 31);

            start = start.AddDays(-(int)start.DayOfWeek);
            end = end.AddDays(-(int)end.DayOfWeek);

            return (date > start && date < end);
        }

        /// <summary>
        /// Gets the current UK time, taking BST into account if applicable
        /// </summary>
        static public DateTime Now
        {
            get
            {
                DateTime now = DateTime.UtcNow;
                if (IsBritishSummerTime(now))
                    now = now.AddHours(1);

                return now;
            }
        }
    }
}
