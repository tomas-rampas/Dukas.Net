//#nullable enable
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using static System.FormattableString;

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
        return this;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Tick tick)) return false;
        return Math.Abs(tick.Ask - Ask) < 0.000001
               && Math.Abs(tick.Bid - Bid) < 0.000001
               && tick.Timestamp == Timestamp
               && Math.Abs(tick.AskVolume - AskVolume) < 0.001
               && Math.Abs(tick.BidVolume - BidVolume) < 0.001;
    }

    public override int GetHashCode()
    {
        return Utils.HashCodeHelper.Combine(Timestamp);
    }

    /// <summary>
    /// Implicit conversion to string
    /// </summary>
    /// <param name="t">Tick to convert</param>
    /// <returns>CSV string</returns>
    public static implicit operator string(Tick t)
    {
        return Invariant(
            $"{t.Timestamp:yyyy-MM-dd HH:mm:ss.fff},{t.Bid:0.#####},{t.BidVolume:0.###},{t.Ask:0.#####},{t.AskVolume:0.###}");
    }

    /// <summary>
    /// Implicit conversion from string, which is expected to be CSV string object
    /// </summary>
    /// <param name="csvRecord">Comma separated string with tick data</param>
    /// <returns>Instance of Tick class</returns>
    [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH")]
    public static implicit operator Tick(string csvRecord)
    {
        if (string.IsNullOrWhiteSpace(csvRecord)) throw new ArgumentNullException(nameof(csvRecord));

        var csvValues = csvRecord.Trim().Split(new[] { ',' });

        if (csvValues == null || csvValues.Length < 1)
            throw new ArgumentException($"{nameof(csvRecord)} is wrongly formatted");

        if (!DateTime.TryParse(csvValues[0], out var timeStamp))
        {
            Debugger.Break();
        }

        try
        {
            return new Tick(timeStamp)
            {
                Bid = double.Parse(csvValues[1], CultureInfo.InvariantCulture),
                BidVolume = float.Parse(csvValues[2], CultureInfo.InvariantCulture),
                Ask = double.Parse(csvValues[3], CultureInfo.InvariantCulture),
                AskVolume = float.Parse(csvValues[4], CultureInfo.InvariantCulture)
            };
        }
        catch (Exception)
        {
            Debugger.Break();
            throw;
        }
    }
}}
