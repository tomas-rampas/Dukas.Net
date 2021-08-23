using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Bi5.Net.Models;

[assembly: InternalsVisibleTo("Bi5.Net.Tests")]
namespace Bi5.Net.Utils
{
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
        /// <param name="decimals">How many digits is after decimal JPY pairs can have 3, Majoe pairs 4 or 5, and
        /// Index and Commodity CFDs can have 1 or 2</param>
        /// <returns>Array of Ticks translated from given byte array</returns>
        /// <exception cref="ArgumentException">Exception thrown when given byte array is not divisible
        /// by 20 (size of record) without remainder</exception>
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
                var tickTimestamp = new DateTime(date.Year,date.Month,date.Day, date.Hour, 0, 0)
                    .AddMilliseconds(milliseconds);

                var i1 = BitConverter.ToInt32(bytes[new Range(i+4, i + 8)].Bi5ToArray());
                var i2 = BitConverter.ToInt32(bytes[new Range(i+8, i + 12)].Bi5ToArray());
                var f1 = BitConverter.ToSingle(bytes[new Range(i+12, i + 16)].Bi5ToArray());
                var f2 = BitConverter.ToSingle(bytes[new Range(i+16, i + 20)].Bi5ToArray());
                var tick = new Tick(tickTimestamp)
                {
                    Bid = i2 / Math.Pow(10, decimals),
                    BidVolume = f2,
                    Ask = i1 / Math.Pow(10, decimals),
                    AskVolume = f1,
                };
                Console.WriteLine(tick);
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
        /// <returns>Enumerable of Bars</returns>
        internal static IEnumerable<Bar> Resample(this IEnumerable<Tick> ticks, DateTimePart majorScale, int minorScale)
        {
            var bars = ticks
                .GroupBy(tick => new { BarTime= TimeframeUtils.GetTimestampForCandle(tick.Timestamp, 
                    majorScale, minorScale)} 
                )
                .Select(grouping => new Bar
                    {
                        Ticks = grouping.Count(),
                        Timestamp = grouping.Key.BarTime,
                        Open = grouping.OrderBy(x=>x.Timestamp).First().Bid,
                        High = grouping.Max(x=>x.Bid),
                        Low = grouping.Min(x=>x.Bid),
                        Close = grouping.OrderBy(x=>x.Timestamp).Last().Bid,
                        Volume = Math.Round(grouping.Sum(x=>x.BidVolume), 3)
                    }
                ).ToList();

            return bars;
        }

        internal static byte[] Bi5ToArray(this IEnumerable<byte> bytes) => bytes.Reverse().ToArray();
        
        /// <summary>
        /// Concatenates two or more arrays into a single one.
        /// No linq here as we can work with huge tick data 
        /// </summary>
        public static T[] Concat<T>(params T[][] arrays)
        {
            // return (from array in arrays from arr in array select arr).ToArray();
 
            var result = new T[arrays.Sum(a => a.Length)];
            int offset = 0;
            for (int x = 0; x < arrays.Length; x++)
            {
                arrays[x].CopyTo(result, offset);
                offset += arrays[x].Length;
            }
            return result;
        }
    }
}