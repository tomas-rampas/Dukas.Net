#nullable enable
using System;

namespace Bi5.Net.Models
{
    public class Tick : ITimedData
    {
        public Tick(DateTime timestamp)
        {
            Timestamp = timestamp;
        }

        public DateTime Timestamp { get; }
        public double Bid { get; set; }
        public float BidVolume { get; set; }
        public double Ask { get; set; }
        public float AskVolume { get; set; }

        public override string ToString()
        {
            return (this);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Tick tick) return false;
            return Math.Abs(tick.Ask - Ask) < 0.000001
                   && Math.Abs(tick.Bid - Bid) < 0.000001
                   && tick.Timestamp == Timestamp
                   && Math.Abs(tick.AskVolume - AskVolume) < 0.001
                   && Math.Abs(tick.BidVolume - BidVolume) < 0.001;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Timestamp);
        }

        /// <summary>
        /// Implicit conversion to string
        /// </summary>
        /// <param name="t">Tick to convert</param>
        /// <returns>CSV string</returns>
        public static implicit operator string(Tick t)
        {
            return $"{t.Timestamp:yyyy-MM-dd HH:mm:ss.fff},{t.Bid},{t.BidVolume},{t.Ask},{t.AskVolume}";
        }
    }
}