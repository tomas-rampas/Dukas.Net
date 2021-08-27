using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Bi5.Net.Models;

namespace Bi5.Net.IO
{
    public class WriterFactory
    {
        internal static IFileWriter CreateFactory(IEnumerable<ITimedData> timedData)
        {
            if (timedData is IEnumerable<Tick>) return new TickDataFileWriter();
            if (timedData is IEnumerable<Bar>) return new OhlcvFileWriter();
            throw new ArgumentException("Unknown Timed Data", nameof(timedData));
        }
    }
}