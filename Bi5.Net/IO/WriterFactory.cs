using System;
using System.Collections.Generic;
using Bi5.Net.Models;

namespace Bi5.Net.IO
{
    public static class WriterFactory
    {
        internal static IFileWriter CreateWriter(IEnumerable<ITimedData> timedData, LoaderConfig loaderConfig)
        {
            return timedData switch
            {
                IEnumerable<Tick> enumerable => new TickDataFileWriter(loaderConfig),
                IEnumerable<Bar> enumerable => new OhlcvFileWriter(loaderConfig),
                _ => throw new ArgumentException("Unknown Timed Data", nameof(timedData))
            };
        }
    }
}