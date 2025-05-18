using System;
using Bi5.Net.Models;
using Bi5.Net.Tests.TestData;
using Bi5.Net.Utils;
using Xunit;

namespace Bi5.Net.Tests
{

public class TimeframeUtilsTests
{
    [Theory]
    [ClassData(typeof(TimestampForCandleDataTheoryData))]
    public void GetTimestampForCandle_Test(DateTime expectedResult, DateTime testData, DateTimePart timePart,
        uint tfScale)
    {
        var result = TimeframeUtils.GetTimestampForCandle(testData, timePart, tfScale);
        Assert.True(expectedResult == result);
    }
}}
