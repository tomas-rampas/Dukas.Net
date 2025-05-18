using System;
using System.Collections.Generic;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Bi5.Net.Models
{

public class LoaderConfig
{
    public LoaderConfig(DateTime startDate, DateTime endDate, IEnumerable<string> products,
        DateTimePart timeFrameMajorScale, uint timeFrameMinorScale, string outputFolder, QuoteSide quoteSide,
        FileScale fileScale, bool writeHeader, bool gzipResult, byte threads = 4)
    {
        StartDate = startDate;
        EndDate = endDate;
        if (endDate.TimeOfDay == new TimeSpan(0, 0, 0))
        {
            EndDate = endDate.AddSeconds(23 * 60 * 60 + 59 * 60 + 59);
        }

        Products = products;
        TimeFrameMajorScale = timeFrameMajorScale;
        TimeFrameMinorScale = timeFrameMinorScale;
        OutputFolder = outputFolder;
        QuoteSide = quoteSide;
        FileScale = fileScale;
        WriteHeader = writeHeader;
        GzipResult = gzipResult;
        Threads = threads;
        UseMarketDate = false;
    }

    public bool UseMarketDate { get; }

    public DateTime StartDate { get; }
    public DateTime EndDate { get; }

    /// <summary>
    /// Consult Dukascopy web site for available list of products
    /// </summary>
    public IEnumerable<string> Products { get; }

    public byte Threads { get; }
    public DateTimePart TimeFrameMajorScale { get; }
    public uint TimeFrameMinorScale { get; }
    public string OutputFolder { get; }
    public QuoteSide QuoteSide { get; }
    public FileScale FileScale { get; }
    public bool WriteHeader { get; }
    public bool GzipResult { get; }
}}
