using System;
using System.Collections.Generic;
using Bi5.Net.Models;

namespace Bi5.Net.IO
{

public static class WriterFactory
{
    internal static IFileWriter CreateWriter(IEnumerable<ITimedData> timedData, LoaderConfig loaderConfig)
    {
        if (timedData is IEnumerable<Tick>)
        {
            return new TickDataFileWriter(loaderConfig);
        }
        else if (timedData is IEnumerable<Bar>)
        {
            return new OhlcvFileWriter(loaderConfig);
        }
        else
        {
            throw new ArgumentException("Unknown Timed Data", nameof(timedData));
        }
    }
}}
