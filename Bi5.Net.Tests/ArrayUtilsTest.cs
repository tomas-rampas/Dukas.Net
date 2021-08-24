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
            var bytes = new byte[] {0};
            Assert.Throws<ArgumentException>(()=> bytes.ToTickArray(DateTime.Now, 0));
        }

        [Fact]
        public void Check_Conversion_Test()
        {
            var today  = DateTime.Now;
            var currentDate = new DateTime(today.Year, today.Month, today.Day, 13, 32, 26);
            var startDate = new DateTime(today.Year, today.Month, today.Day, 13, 0, 0);
            var (bytes, originalTick) = CreateTestTick(currentDate, startDate);
            var ticks = bytes.ToTickArray(startDate, 5);

            var tickArray = ticks as Tick[] ?? ticks.ToArray();
            Assert.True(tickArray.Length == 1);
            Tick resultTick = tickArray.First();
            Assert.True(resultTick.Equals(originalTick));

        }
        
        private static (byte[], Tick) CreateTestTick(DateTime currentDate, DateTime startDate)
        {
            var milliseconds  = (int)(currentDate - startDate).TotalMilliseconds;
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
                Ask = bid/Math.Pow(10, 5),
                AskVolume = bidVol,
                Bid = ask/Math.Pow(10, 5),
                BidVolume = askVol,
            };
            
            return (
                timeBytes.Concat(bidBytes.Concat(askBytes.Concat(bidVolBytes.Concat(askVolBytes)))).ToArray(),
                originalTick
            );
        }

        [Fact]
        public void Convert_Bi5_Bytes_To_Array()
        {
            byte[] input = new byte[] { 1, 2, 3, 4, 5 };
            byte[] output = input.Bi5ToArray();
            Assert.Equal(input.Reverse().ToArray(), output);
        }

        [Fact]
        public void Arrays_Concat_Test()
        {
            Tick[] ticks1 = new[] { 
                new Tick(DateTime.Now)
                {
                    Bid = 1, Ask = 1, AskVolume = (float)2.5, BidVolume = (float)3.1
                } ,
                new Tick(DateTime.Now.AddMinutes(-1))
                {
                Bid = 2, Ask = 2, AskVolume = (float)2.5, BidVolume = (float)3.1
                } 
            };
            Tick[] ticks2 = new[] { 
                new Tick(DateTime.Now.AddHours(1))
                {
                    Bid = 3, Ask = 3, AskVolume = (float)2.5, BidVolume = (float)3.1
                } ,
                new Tick(DateTime.Now.AddHours(-1))
                {
                    Bid = 4, Ask = 4, AskVolume = (float)2.5, BidVolume = (float)3.1
                } 
            };

            var ticks3 = ArrayExtensions.Concat(new [] {ticks1, ticks2});
            Assert.NotNull(ticks3);
            Assert.NotEmpty(ticks3);
            Assert.True(ticks3.Count() == ticks1.Length + ticks2.Length);
        }
    }
}