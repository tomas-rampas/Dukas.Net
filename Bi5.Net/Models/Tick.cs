using System;

namespace Bi5.Net.Models
{
    public class Tick
    {
        public DateTime Timestamp { get; set; }
        public double Bid { get; set; }
        public float BidVolume { get; set; }
        public double Ask { get; set; }
        public float AskVolume { get; set; }

        public override string ToString()
        {
            return (this);
        }

        public static implicit operator string(Tick t)
        {
            return $"{t.Timestamp:dd.MM.yyyy hh:mm:ss.fff},{t.Bid}/{t.Ask},{t.BidVolume}/{t.AskVolume}";
        }
    }
}