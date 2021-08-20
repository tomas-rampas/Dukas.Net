using System;

namespace Bi5.Net.Models
{
    public class Bar
    {
        public int Minute { get; set; }
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
            return $"{bar.Minute} ({bar.Ticks} Ticks) : " +
                   $"{bar.Timestamp} O={bar.Open}, H={bar.High}, L={bar.Low}, C={bar.Close}, V={bar.Volume}";
        }
    }
}