using System;
using System.Collections.Generic;

namespace Bi5.Net.Models
{
    public class LoaderConfig
    {
        private LoaderConfig() {}

        public LoaderConfig(DateTime startDate, DateTime endDate, IEnumerable<string> products,
            DateTimePart timeFrameMajorScale, uint timeFrameMinorScale, string outputFolder, QuoteSide quoteSide,
            byte threads = 4)
        {
            StartDate = startDate;
            EndDate = endDate;
            Products = products;
            TimeFrameMajorScale = timeFrameMajorScale;
            TimeFrameMinorScale = timeFrameMinorScale;
            OutputFolder = outputFolder;
            QuoteSide = quoteSide;
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
        public uint TimeFrameMinorScale { get; }
        public string OutputFolder { get;  }
        public QuoteSide QuoteSide { get; }
    }
}