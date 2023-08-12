namespace Bi5.Net.Models;

/// <summary>
/// Time Frame Major scale.
/// Together with minor scale parameter it defines scaling factor.
/// This allows consumer to define any possible timeframe like 12 milliseconds, 19 hours,  etc.
/// Meaningless time frames are possible 😋.
/// </summary>
public enum DateTimePart
{
    /// <summary>
    /// Second
    /// </summary>
    Sec,

    /// <summary>
    /// Minute
    /// </summary>
    Min,

    /// <summary>
    /// Hour
    /// </summary>
    Hour,

    /// <summary>
    /// Day
    /// </summary>
    Day,

    /// <summary>
    /// Week. Currently not supported.
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    Week,

    /// <summary>
    /// Month
    /// </summary>
    Month,

    /// <summary>
    /// Year
    /// </summary>
    Year
}