using System;
using System.Diagnostics;

namespace Bi5.Net.Models
{
    public class Bar
    {
        public int Ticks { get; set; }
        public DateTime Timestamp { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Volume { get; set; }

        public override string ToString()
        {
            return (this);
        }

        public static implicit operator string(Bar bar)
        {
            return $"{bar.Timestamp:yyyy-MM-dd HH:mm:ss},{bar.Open},{bar.High},{bar.Low},{bar.Close},{bar.Volume}";
        }
    }
}