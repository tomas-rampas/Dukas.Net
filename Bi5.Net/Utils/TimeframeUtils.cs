using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Bi5.Net.Models;

[assembly: InternalsVisibleTo("Bi5.Net.Tests")]

namespace Bi5.Net.Utils;

public delegate DateTime GetResampledDateTime(DateTime timestamp, int minorScale);

public static class TimeframeUtils
{
    private static readonly Dictionary<DateTimePart, GetResampledDateTime> DatePartDelegates =
        new()
        {
            {
                DateTimePart.Sec,
                (timestamp, minorScale) =>
                    new DateTime(timestamp.Year, timestamp.Month, timestamp.Day, timestamp.Hour,
                        timestamp.Minute, timestamp.Second / minorScale * minorScale, 0)
            },
            {
                DateTimePart.Min,
                (timestamp, minorScale) =>
                    new DateTime(timestamp.Year, timestamp.Month, timestamp.Day, timestamp.Hour,
                        timestamp.Minute / minorScale * minorScale, 0)
            },
            {
                DateTimePart.Hour,
                (timestamp, minorScale) =>
                    new DateTime(timestamp.Year, timestamp.Month, timestamp.Day,
                        timestamp.Hour / minorScale * minorScale, 0, 0)
            },
            {
                DateTimePart.Day,
                (timestamp, minorScale) =>
                    new DateTime(timestamp.Year, timestamp.Month, timestamp.Day / minorScale * minorScale,
                        0, 0, 0)
            },
            {
                DateTimePart.Month,
                (timestamp, minorScale) =>
                    new DateTime(timestamp.Year, timestamp.Month / minorScale * minorScale,
                        1, 0, 0, 0)
            },
            {
                DateTimePart.Year,
                (timestamp, minorScale) =>
                    new DateTime(timestamp.Year / minorScale * minorScale, 1, 1, 0, 0, 0)
            }
        };

    public static DateTime GetTimestampForCandle(DateTime timestamp, DateTimePart majorScale, uint minorScale)
    {
        return DatePartDelegates[majorScale](timestamp, (int)minorScale);
    }
}