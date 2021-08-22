using System;
using System.Collections.Generic;

namespace Bi5.Net.Models
{
    public class LoaderConfig
    {
        private LoaderConfig() {}

        public LoaderConfig(DateTime startDate, DateTime? endDate, IEnumerable<string> products, 
            DateTimePart timeFrameMajorScale, byte timeFrameMinorScale, byte threads = 4)
        {
            StartDate = startDate;
            EndDate = endDate ?? DateTime.Now;
            Products = products;
            TimeFrameMajorScale = timeFrameMajorScale;
            TimeFrameMinorScale = timeFrameMinorScale;
            Threads = threads;
        }
        
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        
        /// <summary>
        /// Consult Dukascopy web site for available list of products
        /// </summary>
        public IEnumerable<string> Products { get; }
        public byte Threads { get;  }
        public DateTimePart TimeFrameMajorScale { get; }
        public byte TimeFrameMinorScale { get; }
    }
}