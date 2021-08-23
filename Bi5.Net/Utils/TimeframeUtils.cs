using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Bi5.Net.Models;

[assembly: InternalsVisibleTo("Bi5.Net.Tests")]
namespace Bi5.Net.Utils
{
    public static class TimeframeUtils
    {
        private static readonly Dictionary<string, DateTimePart> Timeframes =
            new()
            {
                { "mSec", DateTimePart.MSec },
                { "Sec", DateTimePart.Sec },
                { "Min", DateTimePart.Min },
                { "Hour", DateTimePart.Hour },
                { "Day", DateTimePart.Day },
                { "Week", DateTimePart.Week },
                { "Month", DateTimePart.Month },
                { "Year", DateTimePart.Year },
            };
        
        public static DateTime GetTimestampForCandle(DateTime timestamp, DateTimePart majorScale, int minorScale)
        {
            // return new DateTime(timestamp.Year, timestamp.Month,
            //     timestamp.Day, timestamp.Hour, timestamp.Minute, (timestamp.Second / 15) * 15);
            return new DateTime(timestamp.Year, timestamp.Month,
                timestamp.Day, timestamp.Hour, (timestamp.Minute/1)*1, 0);
        }
    }
}