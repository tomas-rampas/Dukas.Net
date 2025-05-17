using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using Bi5.Net.Models;

[assembly: InternalsVisibleTo("Bi5.Net.Tests")]

namespace Bi5.Net.Utils;

/// <summary>
/// Byte Array extension methods.
/// Extends arrays to make tick data manipulation little bit easier. 
/// </summary>
internal static class ArrayExtensions
{
    /// <summary>
    /// 5 x 4 bytes
    /// It's size of one Tick record in bi5 file
    /// The structure of bi5 record is following:
    /// 1st 4 bytes -> Time part of the timestamp
    /// 2nd 4 bytes -> Bid
    /// 3rd 4 bytes -> Ask
    /// 4th 4 bytes -> Bid Volume
    /// 5th 4 bytes -> Ask Volume
    /// </summary>
    private const int TickItemByteSize = 20;

    /// <summary>
    /// Converts bytes to Ticks
    /// </summary>
    /// <param name="bytes">Array of bytes to convert</param>
    /// <param name="date">Bi5 does not contain whole timestamp,
    /// so day, month and year is taken from this param when Tick timestamp is being constructed</param>
    /// <param name="decimals">How many digits is after decimal JPY pairs can have 3, other pairs 4 or 5, and
    /// Index and Commodity CFDs can have 1 or 2</param>
    /// <returns>Array of Ticks translated from given byte array</returns>
    /// <exception cref="ArgumentException">Exception thrown when given byte array is not divisible
    /// by 20 (size of record) without remainder</exception>
    [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
    public static IEnumerable<Tick> ToTickArray(this byte[] bytes, DateTime date, int decimals)
    {
        if (bytes.Length % TickItemByteSize > 0)
        {
            throw new ArgumentException("Wrong size of array", nameof(bytes));
        }

        var records = (uint)(bytes.Length / TickItemByteSize);
        var ticks = new Tick[records];

        var k = 0;
        for (var i = 0; i < bytes.Length; i += TickItemByteSize, k++)
        {
            var milliseconds = BitConverter.ToInt32(bytes[new Range(new Index(i), new Index(i + 4))].Bi5ToArray());
            var tickTimestamp = new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0)
                .AddMilliseconds(milliseconds);

            var i1 = BitConverter.ToInt32(bytes[new Range(i + 4, i + 8)].Bi5ToArray());
            var i2 = BitConverter.ToInt32(bytes[new Range(i + 8, i + 12)].Bi5ToArray());
            var f1 = BitConverter.ToSingle(bytes[new Range(i + 12, i + 16)].Bi5ToArray());
            var f2 = BitConverter.ToSingle(bytes[new Range(i + 16, i + 20)].Bi5ToArray());
            var tick = new Tick(tickTimestamp)
            {
                Bid = i2 / Math.Pow(10, decimals),
                BidVolume = f2,
                Ask = i1 / Math.Pow(10, decimals),
                AskVolume = f1
            };
            //Debug.WriteLine(tick);
            ticks[k] = tick;
        }

        return ticks;
    }

    /// <summary>
    /// Resamples Tick data to given time frame bars
    /// </summary>
    /// <param name="ticks">List of Ticks</param>
    /// <param name="majorScale">Major scale</param>
    /// <param name="minorScale">Minor scale</param>
    /// <param name="side">Quote side</param>
    /// <returns>Enumerable of Bars</returns>
    internal static IEnumerable<Bar> Resample(this IEnumerable<Tick> ticks, DateTimePart majorScale
        , uint minorScale, QuoteSide side = QuoteSide.Bid)
    {
        // Validate input and convert to array if not already
        if (ticks is not Tick[] tickArray)
        {
            tickArray = ticks.ToArray();
        }

        if (tickArray.Length == 0)
        {
            return null;
        }

        // Filter out null ticks in-place without additional allocations
        int validCount = 0;
        for (int i = 0; i < tickArray.Length; i++)
        {
            if (tickArray[i] != null)
            {
                if (i != validCount)
                {
                    tickArray[validCount] = tickArray[i];
                }

                validCount++;
            }
        }

        if (validCount == 0)
        {
            return null;
        }

        // Create a dictionary to avoid anonymous type allocations
        var barGroups = new Dictionary<DateTime, List<Tick>>();

        // Group ticks by bar time
        for (int i = 0; i < validCount; i++)
        {
            var tick = tickArray[i];
            var barTime = TimeframeUtils.GetTimestampForCandle(tick.Timestamp, majorScale, minorScale);

            if (!barGroups.TryGetValue(barTime, out var tickList))
            {
                tickList = new List<Tick>();
                barGroups.Add(barTime, tickList);
            }

            tickList.Add(tick);
        }

        // Create bars array with known size to avoid resizing
        var bars = new Bar[barGroups.Count];
        int barIndex = 0;

        foreach (var group in barGroups)
        {
            var barTime = group.Key;
            var tickList = group.Value;

            // Sort once for the group
            tickList.Sort((a, b) => a.Timestamp.CompareTo(b.Timestamp));

            double high = double.MinValue;
            double low = double.MaxValue;
            double volume = 0;

            Tick firstTick = tickList[0];
            Tick lastTick = tickList[tickList.Count - 1];

            // Calculate high, low, and volume in a single pass
            for (int i = 0; i < tickList.Count; i++)
            {
                Tick tick = tickList[i];
                double price = side == QuoteSide.Bid ? tick.Bid : tick.Ask;

                if (price > high) high = price;
                if (price < low) low = price;

                volume += side == QuoteSide.Bid ? tick.BidVolume : tick.AskVolume;
            }

            bars[barIndex++] = new Bar
            {
                Ticks = tickList.Count,
                Timestamp = barTime,
                Open = side == QuoteSide.Bid ? firstTick.Bid : firstTick.Ask,
                High = high,
                Low = low,
                Close = side == QuoteSide.Bid ? lastTick.Bid : lastTick.Ask,
                Volume = Math.Round(volume, 3)
            };
        }

        return bars;
    }

    internal static byte[] Bi5ToArray(this IEnumerable<byte> bytes) => bytes.Reverse().ToArray();
}