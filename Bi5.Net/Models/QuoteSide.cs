namespace Bi5.Net.Models
{

/// <summary>
/// Defines sides of the price quote
/// Data consumer can asked for Bid side, Ask side, or Both
/// If Both is requested 2 files going to be created  for every product. One for Bids, and one for Asks. 
/// </summary>
public enum QuoteSide
{
    /// <summary>
    /// LHS aka Bid
    /// </summary>
    Bid,

    /// <summary>
    /// RHS aka Ask
    /// </summary>
    Ask,

    /// <summary>
    /// Both
    /// </summary>
    Both
}}
