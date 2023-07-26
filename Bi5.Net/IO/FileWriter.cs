using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Bi5.Net.Models;

namespace Bi5.Net.IO;

public abstract class FileWriter<T> : IFileWriter
    where T : ITimedData
{
    protected readonly FileScale FileScale;
    protected readonly string FilePath;
    protected readonly string TimeFrame;

    // ReSharper disable once CollectionNeverQueried.Global
    protected readonly List<string> FilePaths;

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

    protected abstract bool Write(string product, QuoteSide side, IEnumerable<T> data);

    bool IFileWriter.Write(string product, QuoteSide side, IEnumerable data)
    {
        return Write(product, side, (IEnumerable<T>)data);
    }

    string IFileWriter.GetTickDataPath(string product, QuoteSide side, DateTime tickHour)
    {
        return Path.Combine(FilePath, product, "Tick", $"{tickHour:yyyyMMddHH}00.csv");
    }

    public string GetTickDataFolder(string product)
    {
        return Path.Combine(FilePath, product, "Tick");
    }
}