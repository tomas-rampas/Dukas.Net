﻿using System;

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

        public override bool Equals(object obj)
        {
            var tick = obj as Tick;
            if (tick == null) return false;
            return Math.Abs(tick.Ask - Ask) < 0.000001 
                   && Math.Abs(tick.Bid - Bid) < 0.000001
                   && tick.Timestamp == Timestamp 
                   && Math.Abs(tick.AskVolume - AskVolume) < 0.001
                   && Math.Abs(tick.BidVolume - BidVolume) < 0.001;
        }

        public static implicit operator string(Tick t)
        {
            return $"{t.Timestamp:dd.MM.yyyy hh:mm:ss.fff},{t.Bid},{t.BidVolume},{t.Ask},{t.AskVolume}";
        }
    }
}