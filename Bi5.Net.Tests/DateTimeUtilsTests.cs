using System;
using Bi5.Net.Utils;
using NSubstitute;
using Xunit;

namespace Bi5.Net.Tests
{
    public class DateTimeUtilsTests
    {
        #region CalculateEffectiveDate Tests

        [Fact]
        public void CalculateEffectiveDate_WithWeekday_ReturnsStartOfDay()
        {
            // Arrange
            var testDate = new DateTime(2023, 5, 15, 14, 30, 45); // Monday with time component

            // Act
            var result = DateTimeUtils.CalculateEffectiveDate(testDate);

            // Assert
            var expected = new DateTime(2023, 5, 15, 0, 0, 0);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CalculateEffectiveDate_WithSunday_AndEndDateFalse_ReturnsStartOfSunday()
        {
            // Arrange
            var sunday = new DateTime(2023, 5, 14, 14, 30, 45); // Sunday
            Assert.Equal(DayOfWeek.Sunday, sunday.DayOfWeek);

            // Act
            var result = DateTimeUtils.CalculateEffectiveDate(sunday);

            // Assert
            var expected = new DateTime(2023, 5, 14, 0, 0, 0);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CalculateEffectiveDate_WithSunday_AndEndDateTrue_ReturnsStartOfMonday()
        {
            // Arrange
            var sunday = new DateTime(2023, 5, 14, 14, 30, 45); // Sunday
            Assert.Equal(DayOfWeek.Sunday, sunday.DayOfWeek);

            // Act
            var result = DateTimeUtils.CalculateEffectiveDate(sunday, true);

            // Assert
            var expected = new DateTime(2023, 5, 15, 0, 0, 0); // Monday
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CalculateEffectiveDate_WithSaturday_AndEndDateTrue_ReturnsStartOfSameDay()
        {
            // Arrange
            var saturday = new DateTime(2023, 5, 13, 14, 30, 45); // Saturday
            Assert.Equal(DayOfWeek.Saturday, saturday.DayOfWeek);

            // Act
            var result = DateTimeUtils.CalculateEffectiveDate(saturday, true);

            // Assert
            var expected = new DateTime(2023, 5, 13, 0, 0, 0);
            Assert.Equal(expected, result);
        }

        #endregion

        #region IsLastHour Tests

        [Fact]
        public void IsLastHour_WithCurrentUtcHourMinus1_ReturnsTrue()
        {
            // Arrange
            var currentUtcHour = DateTime.Now.ToUniversalTime().Round(TimeSpan.FromHours(1));
            var oneHourBefore = currentUtcHour.AddHours(-1);

            // Act
            var result = DateTimeUtils.IsLastHour(oneHourBefore, false);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsLastHour_WithRegularWeekdayAt23Hour_NotUsingMarketDate_ReturnsTrue()
        {
            // Arrange - Wednesday at 23:00
            var wednesday23 = new DateTime(2023, 5, 17, 23, 0, 0);
            Assert.Equal(DayOfWeek.Wednesday, wednesday23.DayOfWeek);

            // Act
            var result = DateTimeUtils.IsLastHour(wednesday23, false);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsLastHour_WithRegularWeekdayNotAt23Hour_NotUsingMarketDate_ReturnsFalse()
        {
            // Arrange - Wednesday at 22:00
            var wednesday22 = new DateTime(2023, 5, 17, 22, 0, 0);
            Assert.Equal(DayOfWeek.Wednesday, wednesday22.DayOfWeek);

            // Act
            var result = DateTimeUtils.IsLastHour(wednesday22, false);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(20,6,20,true)]  // During DST at 20:00, should be true
        [InlineData(10, 1,21,true)] // Not during DST at 21:00, should be true
        [InlineData(20,6,19,false)] // During DST at 19:00, should be false
        [InlineData(10,1,20,false)] // Not during DST at 20:00, should be false
        public void IsLastHour_WithMarketDate_RespectsTimeZoneSettings(int day, int month, int hour, bool expected)
        {
            // Arrange - create a test date with specified hour
            var testDate = new DateTime(2025, month, day, hour, 0, 0);
            
            // Act
            var result = DateTimeUtils.IsLastHour(testDate, true);
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IsLastHour_WithFridayAt20Hour_NotUsingMarketDate_ReturnsTrue()
        {
            // Arrange - Friday at 20:00
            var friday20 = new DateTime(2023, 5, 19, 20, 0, 0);
            Assert.Equal(DayOfWeek.Friday, friday20.DayOfWeek);
            
            // Act
            var result = DateTimeUtils.IsLastHour(friday20, false);
            
            // Assert
            Assert.True(result);
        }

        #endregion

        #region GetLastHour Tests

        [Theory]
        [InlineData(false, 23)] // Not using market date should always return 23
        [InlineData(true, 20)]  // Using market date during DST should return 20
        public void GetLastHour_ReturnsCorrectHourBasedOnSettings(bool useMarketDate, int expected)
        {
            // Arrange
            var dstDate = new DateTime(2023, 7, 15); // Summer, likely DST
            
            // Act
            var result = DateTimeUtils.GetLastHour(dstDate, useMarketDate);
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetLastHour_UsingMarketDate_NotDuringDst_Returns21()
        {
            // Arrange
            var nonDstDate = new DateTime(2023, 1, 15); // Winter, likely not DST
            
            // Act
            var result = DateTimeUtils.GetLastHour(nonDstDate, true);
            
            // Assert
            Assert.Equal(21, result);
        }

        #endregion

        #region Round Tests

        [Fact]
        public void Round_TimeSpan_WithDefaultRounding_RoundsCorrectly()
        {
            // Arrange
            var timeSpan = TimeSpan.FromMinutes(10.6); // 10 minutes and 36 seconds
            var roundingInterval = TimeSpan.FromMinutes(5);
            
            // Act
            var result = timeSpan.Round(roundingInterval);
            
            // Assert
            Assert.Equal(TimeSpan.FromMinutes(10), result);
        }

        [Fact]
        public void Round_TimeSpan_WithMidpointRoundingAwayFromZero_RoundsCorrectly()
        {
            // Arrange
            var timeSpan = TimeSpan.FromMinutes(7.5); // 7 minutes and 30 seconds
            var roundingInterval = TimeSpan.FromMinutes(5);
            
            // Act
            var result = timeSpan.Round(roundingInterval, MidpointRounding.AwayFromZero);
            
            // Assert
            Assert.Equal(TimeSpan.FromMinutes(10), result);
        }

        [Fact]
        public void Round_TimeSpan_WithMidpointRoundingToEven_RoundsCorrectly()
        {
            // Arrange
            var timeSpan = TimeSpan.FromMinutes(7.5); // 7 minutes and 30 seconds
            var roundingInterval = TimeSpan.FromMinutes(5);
            
            // Act
            var result = timeSpan.Round(roundingInterval, MidpointRounding.ToEven);
            
            // Assert
            Assert.Equal(TimeSpan.FromMinutes(10), result); // Rounds to nearest even multiple (5)
        }

        [Fact]
        public void Round_DateTime_RoundsCorrectly()
        {
            // Arrange
            var dateTime = new DateTime(2023, 5, 17, 14, 28, 0);
            var roundingInterval = TimeSpan.FromHours(1);
            
            // Act
            var result = dateTime.Round(roundingInterval);
            
            // Assert
            Assert.Equal(new DateTime(2023, 5, 17, 14, 0, 0), result);
        }

        [Theory]
        [InlineData(14, 10, 0, 14, 0, 0)]  // 14:10 -> 14:00
        [InlineData(14, 35, 0, 15, 0, 0)]  // 14:35 -> 15:00
        [InlineData(14, 30, 0, 14, 0, 0)]  // 14:30 -> 14:00 (midpoint to even)
        [InlineData(13, 30, 0, 14, 0, 0)]  // 13:30 -> 14:00 (midpoint to even)
        public void Round_DateTime_WithHourInterval_RoundsCorrectly(
            int hour, int minute, int second,
            int expectedHour, int expectedMinute, int expectedSecond)
        {
            // Arrange
            var dateTime = new DateTime(2023, 5, 17, hour, minute, second);
            var roundingInterval = TimeSpan.FromHours(1);
            
            // Act
            var result = dateTime.Round(roundingInterval);
            
            // Assert
            var expected = new DateTime(2023, 5, 17, expectedHour, expectedMinute, expectedSecond);
            Assert.Equal(expected, result);
        }

        #endregion
    }
}