using System;
using System.Linq;
using Bi5.Net.Models;
using Bi5.Net.Utils;
using Xunit;

namespace Bi5.Net.Tests
{

public class ArrayUtilsTest
{
    [Fact]
    public void Checking_Wrong_Byte_Array_Test()
    {
        var bytes = new byte[] { 0 };
        Assert.Throws<ArgumentException>(() => bytes.ToTickArray(DateTime.Now, 0));
    }

    [Fact]
    public void Check_Conversion_Test()
    {
        var today = DateTime.Now;
        var currentDate = new DateTime(today.Year, today.Month, today.Day, 13, 32, 26);
        var startDate = new DateTime(today.Year, today.Month, today.Day, 13, 0, 0);
        var (bytes, originalTick) = CreateTestTick(currentDate, startDate);
        var ticks = bytes.ToTickArray(startDate, 5);

        var tickArray = ticks as Tick[] ?? ticks.ToArray();
        Assert.True(tickArray.Length == 1);
        var resultTick = tickArray.First();
        Assert.True(resultTick.Equals(originalTick));
    }

    private static (byte[], Tick) CreateTestTick(DateTime currentDate, DateTime startDate)
    {
        var milliseconds = (int)(currentDate - startDate).TotalMilliseconds;
        var timeBytes = BitConverter.GetBytes(milliseconds).Reverse();
        const int bid = 12345;
        var bidBytes = BitConverter.GetBytes(bid).Reverse();
        const int ask = 12346;
        var askBytes = BitConverter.GetBytes(ask).Reverse();
        const float bidVol = (float)1.15;
        var bidVolBytes = BitConverter.GetBytes(bidVol).Reverse();
        const float askVol = (float)2.30;
        var askVolBytes = BitConverter.GetBytes(askVol).Reverse();

        var timestamp = startDate.AddMilliseconds(milliseconds);

        var originalTick = new Tick(timestamp)
        {
            Ask = bid / Math.Pow(10, 5),
            AskVolume = bidVol,
            Bid = ask / Math.Pow(10, 5),
            BidVolume = askVol
        };

        return (
            timeBytes.Concat(bidBytes.Concat(askBytes.Concat(bidVolBytes.Concat(askVolBytes)))).ToArray(),
            originalTick
        );
    }

    [Fact]
    public void Convert_Bi5_Bytes_To_Array()
    {
        byte[] input = { 1, 2, 3, 4, 5 };
        var output = input.Bi5ToArray();
        Assert.Equal(input.Reverse().ToArray(), output);
    }
    
        #region Bi5ToArray Tests

        [Fact]
        public void Bi5ToArray_ShouldReverseByteOrder()
        {
            // Arrange
            byte[] original = { 1, 2, 3, 4 };
            
            // Act
            byte[] result = original.Bi5ToArray();
            
            // Assert
            Assert.Equal(4, result.Length);
            Assert.Equal(4, result[0]);
            Assert.Equal(3, result[1]);
            Assert.Equal(2, result[2]);
            Assert.Equal(1, result[3]);
        }

        [Fact]
        public void Bi5ToArray_WithEmptyArray_ShouldReturnEmptyArray()
        {
            // Arrange
            byte[] emptyArray = new byte[0];
            
            // Act
            byte[] result = emptyArray.Bi5ToArray();
            
            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Bi5ToArray_WithSingleByte_ShouldReturnSameByte()
        {
            // Arrange
            byte[] singleByte = { 42 };
            
            // Act
            byte[] result = singleByte.Bi5ToArray();
            
            // Assert
            Assert.Single(result);
            Assert.Equal(42, result[0]);
        }

        #endregion

        #region ToTickArray Tests

        [Fact]
        public void ToTickArray_WithInvalidSize_ShouldThrowArgumentException()
        {
            // Arrange
            byte[] invalidBytes = new byte[21]; // Not divisible by TickItemByteSize (20)
            DateTime testDate = new DateTime(2023, 1, 1, 10, 0, 0);
            
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => invalidBytes.ToTickArray(testDate, 5).ToArray());
            Assert.Equal("bytes", exception.ParamName);
            Assert.Contains("Wrong size of array", exception.Message);
        }

        [Fact]
        public void ToTickArray_WithEmptyArray_ShouldReturnEmptyCollection()
        {
            // Arrange
            byte[] emptyArray = new byte[0];
            DateTime testDate = new DateTime(2023, 1, 1, 10, 0, 0);
            
            // Act
            var result = emptyArray.ToTickArray(testDate, 5).ToArray();
            
            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ToTickArray_WithValidSingleRecord_ShouldReturnSingleTick()
        {
            // Arrange
            var testDate = new DateTime(2023, 1, 1, 10, 0, 0);
            byte[] bytes = new byte[20]; // One complete record
            
            // Prepare test data - milliseconds (60000 = 1 minute)
            BitConverter.GetBytes(60000).Reverse().ToArray().CopyTo(bytes, 0);
            
            // Ask price (12346 / 10^5 = 0.12346)
            BitConverter.GetBytes(12346).Reverse().ToArray().CopyTo(bytes, 4);
            
            // Bid price (12345 / 10^5 = 0.12345)
            BitConverter.GetBytes(12345).Reverse().ToArray().CopyTo(bytes, 8);
            
            // Ask volume (2.5f)
            BitConverter.GetBytes(2.5f).Reverse().ToArray().CopyTo(bytes, 12);
            
            // Bid volume (1.5f)
            BitConverter.GetBytes(1.5f).Reverse().ToArray().CopyTo(bytes, 16);
            
            // Act
            var result = bytes.ToTickArray(testDate, 5).ToArray();
            
            // Assert
            Assert.Single(result);
            var tick = result[0];
            Assert.Equal(testDate.AddMinutes(1), tick.Timestamp);
            Assert.Equal(0.12345, tick.Bid, 5);
            Assert.Equal(0.12346, tick.Ask, 5);
            Assert.Equal(1.5f, tick.BidVolume);
            Assert.Equal(2.5f, tick.AskVolume);
        }

        [Fact]
        public void ToTickArray_WithMultipleRecords_ShouldReturnMultipleTicks()
        {
            // Arrange
            var testDate = new DateTime(2023, 1, 1, 10, 0, 0);
            byte[] bytes = new byte[40]; // Two complete records
            
            // Record 1
            BitConverter.GetBytes(60000).Reverse().ToArray().CopyTo(bytes, 0); // 1 minute
            BitConverter.GetBytes(12346).Reverse().ToArray().CopyTo(bytes, 4); // Ask 
            BitConverter.GetBytes(12345).Reverse().ToArray().CopyTo(bytes, 8); // Bid
            BitConverter.GetBytes(2.5f).Reverse().ToArray().CopyTo(bytes, 12); // Ask volume
            BitConverter.GetBytes(1.5f).Reverse().ToArray().CopyTo(bytes, 16); // Bid volume
            
            // Record 2
            BitConverter.GetBytes(120000).Reverse().ToArray().CopyTo(bytes, 20); // 2 minutes
            BitConverter.GetBytes(12356).Reverse().ToArray().CopyTo(bytes, 24); // Ask
            BitConverter.GetBytes(12355).Reverse().ToArray().CopyTo(bytes, 28); // Bid
            BitConverter.GetBytes(3.5f).Reverse().ToArray().CopyTo(bytes, 32); // Ask volume
            BitConverter.GetBytes(2.5f).Reverse().ToArray().CopyTo(bytes, 36); // Bid volume
            
            // Act
            var result = bytes.ToTickArray(testDate, 5).ToArray();
            
            // Assert
            Assert.Equal(2, result.Length);
            
            // Check first tick
            Assert.Equal(testDate.AddMinutes(1), result[0].Timestamp);
            Assert.Equal(0.12345, result[0].Bid, 5);
            Assert.Equal(0.12346, result[0].Ask, 5);
            Assert.Equal(1.5f, result[0].BidVolume);
            Assert.Equal(2.5f, result[0].AskVolume);
            
            // Check second tick
            Assert.Equal(testDate.AddMinutes(2), result[1].Timestamp);
            Assert.Equal(0.12355, result[1].Bid, 5);
            Assert.Equal(0.12356, result[1].Ask, 5);
            Assert.Equal(2.5f, result[1].BidVolume);
            Assert.Equal(3.5f, result[1].AskVolume);
        }

        [Fact]
        public void ToTickArray_WithDifferentDecimals_ShouldScalePricesCorrectly()
        {
            // Arrange
            var testDate = new DateTime(2023, 1, 1, 10, 0, 0);
            byte[] bytes = new byte[20];
            
            BitConverter.GetBytes(60000).Reverse().ToArray().CopyTo(bytes, 0);
            BitConverter.GetBytes(1235).Reverse().ToArray().CopyTo(bytes, 4); // Ask
            BitConverter.GetBytes(1234).Reverse().ToArray().CopyTo(bytes, 8); // Bid
            BitConverter.GetBytes(2.5f).Reverse().ToArray().CopyTo(bytes, 12);
            BitConverter.GetBytes(1.5f).Reverse().ToArray().CopyTo(bytes, 16);
            
            // Act
            // Using 3 decimals instead of 5
            var result = bytes.ToTickArray(testDate, 3).ToArray();
            
            // Assert
            Assert.Single(result);
            Assert.Equal(1.234, result[0].Bid, 3);
            Assert.Equal(1.235, result[0].Ask, 3);
        }

        #endregion

        #region Resample Tests

        [Fact]
        public void Resample_WithEmptyArray_ShouldReturnNull()
        {
            // Arrange
            Tick[] emptyArray = new Tick[0];
            
            // Act
            var result = emptyArray.Resample(DateTimePart.Minute, 1);
            
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Resample_WithNullTicks_ShouldReturnNull()
        {
            // Arrange
            Tick[] ticksWithNull = new Tick[] { null, null };
            
            // Act
            var result = ticksWithNull.Resample(DateTimePart.Minute, 1);
            
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Resample_WithMixedValidAndNullTicks_ShouldFilterNulls()
        {
            // Arrange
            var baseTime = new DateTime(2023, 1, 1, 10, 0, 0);
            Tick[] ticks = new Tick[]
            {
                null,
                new Tick(baseTime) { Bid = 1.0, Ask = 1.1, BidVolume = 100, AskVolume = 110 },
                null,
                new Tick(baseTime.AddSeconds(30)) { Bid = 1.2, Ask = 1.3, BidVolume = 120, AskVolume = 130 }
            };
            
            // Act
            var result = ticks.Resample(DateTimePart.Minute, 1).ToArray();
            
            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(2, result[0].Ticks); // Should have two valid ticks
        }

        [Fact]
        public void Resample_WithValidTicks_ShouldCreateBars()
        {
            // Arrange
            var baseTime = new DateTime(2023, 1, 1, 10, 0, 0);
            Tick[] ticks = new Tick[]
            {
                new Tick(baseTime) { Bid = 1.0, Ask = 1.1, BidVolume = 100, AskVolume = 110 },
                new Tick(baseTime.AddSeconds(15)) { Bid = 1.2, Ask = 1.3, BidVolume = 120, AskVolume = 130 },
                new Tick(baseTime.AddSeconds(45)) { Bid = 0.9, Ask = 1.0, BidVolume = 90, AskVolume = 100 },
                // New minute
                new Tick(baseTime.AddMinutes(1)) { Bid = 1.1, Ask = 1.2, BidVolume = 110, AskVolume = 120 },
                new Tick(baseTime.AddMinutes(1).AddSeconds(30)) { Bid = 1.3, Ask = 1.4, BidVolume = 130, AskVolume = 140 }
            };
            
            // Act
            var result = ticks.Resample(DateTimePart.Minute, 1).ToArray();
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Length); // Two bars (one for each minute)
            
            // First bar (10:00)
            Assert.Equal(baseTime, result[0].Timestamp);
            Assert.Equal(3, result[0].Ticks);
            Assert.Equal(1.0, result[0].Open);
            Assert.Equal(1.2, result[0].High);
            Assert.Equal(0.9, result[0].Low);
            Assert.Equal(0.9, result[0].Close);
            Assert.Equal(310, result[0].Volume);
            
            // Second bar (10:01)
            Assert.Equal(baseTime.AddMinutes(1), result[1].Timestamp);
            Assert.Equal(2, result[1].Ticks);
            Assert.Equal(1.1, result[1].Open);
            Assert.Equal(1.3, result[1].High);
            Assert.Equal(1.1, result[1].Low);
            Assert.Equal(1.3, result[1].Close);
            Assert.Equal(240, result[1].Volume);
        }

        [Fact]
        public void Resample_WithAskSide_ShouldCreateBarsUsingAskPrices()
        {
            // Arrange
            var baseTime = new DateTime(2023, 1, 1, 10, 0, 0);
            Tick[] ticks = new Tick[]
            {
                new Tick(baseTime) { Bid = 1.0, Ask = 1.1, BidVolume = 100, AskVolume = 110 },
                new Tick(baseTime.AddSeconds(30)) { Bid = 1.2, Ask = 1.3, BidVolume = 120, AskVolume = 130 }
            };
            
            // Act
            var result = ticks.Resample(DateTimePart.Minute, 1, QuoteSide.Ask).ToArray();
            
            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(1.1, result[0].Open);
            Assert.Equal(1.3, result[0].High);
            Assert.Equal(1.1, result[0].Low);
            Assert.Equal(1.3, result[0].Close);
            Assert.Equal(240, result[0].Volume); // 110 + 130
        }

        [Fact]
        public void Resample_WithEnumerableInput_ShouldCreateBars()
        {
            // Arrange
            var baseTime = new DateTime(2023, 1, 1, 10, 0, 0);
            var ticks = new Tick[]
            {
                new Tick(baseTime) { Bid = 1.0, Ask = 1.1, BidVolume = 100, AskVolume = 110 },
                new Tick(baseTime.AddSeconds(30)) { Bid = 1.2, Ask = 1.3, BidVolume = 120, AskVolume = 130 }
            }.AsEnumerable(); // Convert to IEnumerable<Tick> to test that code path
            
            // Act
            var result = ticks.Resample(DateTimePart.Minute, 1).ToArray();
            
            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public void Resample_WithDifferentTimeframes_ShouldGroupTicksCorrectly()
        {
            // Arrange
            var baseTime = new DateTime(2023, 1, 1, 10, 0, 0);
            Tick[] ticks = new Tick[]
            {
                // Minutes 0-9
                new Tick(baseTime) { Bid = 1.0, Ask = 1.1, BidVolume = 100, AskVolume = 110 },
                new Tick(baseTime.AddMinutes(5)) { Bid = 1.2, Ask = 1.3, BidVolume = 120, AskVolume = 130 },
                // Minutes 10-19
                new Tick(baseTime.AddMinutes(12)) { Bid = 1.1, Ask = 1.2, BidVolume = 110, AskVolume = 120 },
                new Tick(baseTime.AddMinutes(18)) { Bid = 1.3, Ask = 1.4, BidVolume = 130, AskVolume = 140 }
            };
            
            // Act - group by 10 minute intervals
            var result = ticks.Resample(DateTimePart.Minute, 10).ToArray();
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Length);
            
            // First 10-minute bar (10:00-10:09)
            Assert.Equal(new DateTime(2023, 1, 1, 10, 0, 0), result[0].Timestamp);
            Assert.Equal(2, result[0].Ticks);
            
            // Second 10-minute bar (10:10-10:19)
            Assert.Equal(new DateTime(2023, 1, 1, 10, 10, 0), result[1].Timestamp);
            Assert.Equal(2, result[1].Ticks);
        }

        [Fact]
        public void Resample_WithHourlyTimeframe_ShouldCreateHourlyBars()
        {
            // Arrange
            var baseTime = new DateTime(2023, 1, 1, 10, 0, 0);
            Tick[] ticks = new Tick[]
            {
                new Tick(baseTime) { Bid = 1.0, Ask = 1.1, BidVolume = 100, AskVolume = 110 },
                new Tick(baseTime.AddMinutes(30)) { Bid = 1.2, Ask = 1.3, BidVolume = 120, AskVolume = 130 },
                new Tick(baseTime.AddHours(1)) { Bid = 1.1, Ask = 1.2, BidVolume = 110, AskVolume = 120 },
                new Tick(baseTime.AddHours(1).AddMinutes(30)) { Bid = 1.3, Ask = 1.4, BidVolume = 130, AskVolume = 140 }
            };
            
            // Act
            var result = ticks.Resample(DateTimePart.Hour, 1).ToArray();
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Length);
            Assert.Equal(baseTime, result[0].Timestamp);
            Assert.Equal(baseTime.AddHours(1), result[1].Timestamp);
        }

        [Fact]
        public void Resample_WithUnsortedTicks_ShouldSortAndCreateCorrectBars()
        {
            // Arrange
            var baseTime = new DateTime(2023, 1, 1, 10, 0, 0);
            Tick[] ticks = new Tick[]
            {
                // Purposely out of order
                new Tick(baseTime.AddSeconds(45)) { Bid = 0.9, Ask = 1.0, BidVolume = 90, AskVolume = 100 },
                new Tick(baseTime) { Bid = 1.0, Ask = 1.1, BidVolume = 100, AskVolume = 110 },
                new Tick(baseTime.AddSeconds(15)) { Bid = 1.2, Ask = 1.3, BidVolume = 120, AskVolume = 130 },
            };
            
            // Act
            var result = ticks.Resample(DateTimePart.Minute, 1).ToArray();
            
            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(1.0, result[0].Open); // Should be from the earliest tick (baseTime)
            Assert.Equal(0.9, result[0].Close); // Should be from the latest tick (baseTime+45sec)
        }
        
        #endregion
    
}}
