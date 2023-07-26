using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bi5.Net.Models;
using static System.IO.Path;

namespace Bi5.Net.IO;

public class TickDataFileWriter : FileWriter<Tick>
{
    public TickDataFileWriter(LoaderConfig cfg) : base(cfg)
    {
    }

    [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
    [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
    protected override bool Write(string product, QuoteSide side, IEnumerable<Tick> data)
    {
        var enumerableData = data as Tick[] ?? data.ToArray();
        if (data == null || !enumerableData.Any()) throw new ArgumentException(null, nameof(data));
        var ticks = data as Tick[] ?? enumerableData;
        var firstTickHour = ticks.First().Timestamp;
        var dataPath = Combine(FilePath, product, "Tick");
        Directory.CreateDirectory(dataPath);
        var filePath = Combine(dataPath, $"{firstTickHour:yyyyMMddHH}00.csv");
        var lines = ticks.Select(tick => tick.ToString());
        Task.Run(() => File.WriteAllLinesAsync(filePath, lines));
        FilePaths.Add(filePath);
        return true;
    }

    public async Task<IEnumerable<Tick>> ReadTickFromDisk(string product, QuoteSide side, DateTime date)
    {
        var ticks = new List<Tick>();
        var filePath = ((IFileWriter)this).GetTickDataPath(product, side, date);
        if (filePath == "" || !File.Exists(filePath)) return null;
        var lines = await File.ReadAllLinesAsync(filePath);
        if (lines.Length < 1) return ticks;
        Array.ForEach(lines, line => { ticks.Add(line); });
        return ticks;
    }
}