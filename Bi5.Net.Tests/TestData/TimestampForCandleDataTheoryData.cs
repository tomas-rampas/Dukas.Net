using System;
using Bi5.Net.Models;
using Xunit;

namespace Bi5.Net.Tests.TestData;

public class TimestampForCandleDataTheoryData : TheoryData<DateTime, DateTime, DateTimePart, uint>
{
    public TimestampForCandleDataTheoryData()
    {
        var today = new DateTime(2021, 1, 1, 10, 35, 14, 566);
        Add(new DateTime(today.Year, today.Month, today.Day, today.Hour, today.Minute,
                today.Second + 3, 0),
            today.AddSeconds(3), DateTimePart.Sec, 1);
        Add(new DateTime(today.Year, today.Month, today.Day, today.Hour, today.AddMinutes(-13).Minute,
                0, 0),
            today.AddMinutes(-13), DateTimePart.Min, 1);
        Add(new DateTime(today.Year, today.Month, today.Day, today.AddHours(-1).Hour, 0, 0, 0, 0),
            today.AddHours(-1), DateTimePart.Hour, 1);
        Add(new DateTime(today.Year, today.Month, today.AddDays(12).Day, 0, 0, 0, 0),
            today.AddDays(12), DateTimePart.Day, 1);
        Add(new DateTime(today.Year, today.Month, 1, 0, 0, 0, 0),
            today, DateTimePart.Month, 1);
        Add(new DateTime(today.AddYears(3).Year, 1, 1, 0, 0, 0, 0),
            today.AddYears(3), DateTimePart.Year, 1);
    }
}