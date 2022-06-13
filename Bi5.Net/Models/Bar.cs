using System;
using static System.FormattableString;

namespace Bi5.Net.Models
{
    public class Bar : ITimedData
    {
        public DateTime GroupDate { get; set; }
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
            return Invariant(
                $"{bar.Timestamp:yyyy-MM-dd HH:mm:ss},{bar.Open:0.#####},{bar.High:0.#####},{bar.Low:0.#####},{bar.Close:0.#####},{bar.Volume:0.#####}"
            );
        }
    }
}