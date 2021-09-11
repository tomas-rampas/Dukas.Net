﻿using System;

namespace Bi5.Net.Utils
{
    internal static class DateTimeUtils
    {
        /// <summary>
        /// Calculates start of the day   
        /// </summary>
        /// <param name="dateTime">Given Timestamp</param>
        /// <param name="endDate">Is it end datetime</param>
        /// <returns>DateTime shifted accordingly</returns>
        internal static DateTime CalculateEffectiveDate(DateTime dateTime, bool endDate = false)
        {
            var startDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
            
            switch (endDate)
            {
                case true when dateTime.DayOfWeek == DayOfWeek.Sunday:
                    startDate = startDate.AddDays(-1);
                    break;
                case true:
                    startDate = startDate.AddDays(1);
                    break;
            }
            
            return startDate.IsDaylightSavingTime() ? startDate.AddHours(-2).AddSeconds(-1) : startDate.AddHours(-1).AddSeconds(-1);
        }

        internal static bool IsLastHour(DateTime dateTime, bool useMarketDate)
        {
            
            if (!useMarketDate && dateTime.DayOfWeek != DayOfWeek.Friday) return dateTime.Hour == 23;
            
            if (dateTime.IsDaylightSavingTime() || (!useMarketDate && dateTime.DayOfWeek == DayOfWeek.Friday))
            {
                return dateTime.Hour == 20;
            } 
            return dateTime.Hour == 21;
        }
        
        internal static int GetLastHour(DateTime dateTime, bool useMarketDate) 
            => !useMarketDate ? 23 :  dateTime.IsDaylightSavingTime() ? 20 : 21;
    }
}