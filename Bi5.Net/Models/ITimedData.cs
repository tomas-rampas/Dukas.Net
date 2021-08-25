using System;

namespace Bi5.Net.Models
{
    /// <summary>
    /// Common base interface for tick and OHLCV data  
    /// </summary>
    public interface ITimedData
    {
        public DateTime Timestamp { get; }
    }
}