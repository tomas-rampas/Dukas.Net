using System;
using static System.FormattableString;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Bi5.Net.Models;

public class Bar : ITimedData
{
    public int Ticks { get; set; }
    public DateTime Timestamp { get; init; }
    public double Open { get; init; }
    public double High { get; init; }
    public double Low { get; init; }
    public double Close { get; init; }
    public double Volume { get; init; }

    public override string ToString()
    {
        return this;
    }

    public static implicit operator string(Bar bar)
    {
        return Invariant(
            $"{bar.Timestamp:yyyy-MM-dd HH:mm:ss},{bar.Open:0.#####},{bar.High:0.#####},{bar.Low:0.#####},{bar.Close:0.#####},{bar.Volume:0.#####}"
        );
    }
}