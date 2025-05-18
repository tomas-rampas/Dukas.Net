using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Bi5.Net.Models;


[assembly: InternalsVisibleTo("Bi5.Net.Tests")]

namespace Bi5.Net.IO
{

public abstract class FileWriter<T> : IFileWriter
    where T : ITimedData
{
    internal readonly FileScale FileScale;
    internal readonly string FilePath;
    internal readonly string TimeFrame;
    
    internal readonly List<string> FilePaths;

    protected FileWriter(LoaderConfig cfg)
    {
        FileScale = cfg.FileScale;
        FilePath = cfg.OutputFolder;
        var timeFrameMajorScale = cfg.TimeFrameMajorScale;
        var timeFrameMinorScale = cfg.TimeFrameMinorScale;
        TimeFrame = $"{timeFrameMajorScale}{timeFrameMinorScale}";
        FilePaths = new List<string>();
        Compress = cfg.GzipResult;
    }

    protected bool Compress { get; }

    protected abstract void Write(string product, QuoteSide side, IEnumerable<T> data);

    void IFileWriter.Write(string product, QuoteSide side, IEnumerable data)
    {
        Write(product, side, (IEnumerable<T>)data);
    }

    string IFileWriter.GetTickDataPath(string product, DateTime tickHour)
    {
        return Path.Combine(FilePath, product, "Tick", $"{tickHour:yyyyMMddHH}00.csv");
    }
}}
