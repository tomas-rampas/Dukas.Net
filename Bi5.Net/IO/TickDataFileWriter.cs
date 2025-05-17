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

    protected override void Write(string product, QuoteSide side, IEnumerable<Tick> data)
    {
        // Check for null data first to avoid unnecessary operations
        if (data == null)
            throw new ArgumentException(null, nameof(data));

        // Cast once to array if needed
        Tick[] tickArray;
        if (data is Tick[] arr)
        {
            tickArray = arr;
        }
        else
        {
            tickArray = data.ToArray();
        }

        // Check if array is empty
        if (tickArray.Length == 0)
            throw new ArgumentException("Data collection is empty", nameof(data));

        // Get first tick's hour
        var firstTick = tickArray[0];
        var firstTickHour = firstTick.Timestamp;

        // Create path only once
        var dataPath = Combine(FilePath, product, "Tick");
        Directory.CreateDirectory(dataPath);

        var filePath = Combine(dataPath, $"{firstTickHour:yyyyMMddHH}00.csv");

        // Pre-allocate list for lines
        var lineCount = tickArray.Length;
        var lines = new string[lineCount];

        // Materialize lines in memory with a for loop instead of LINQ
        for (int i = 0; i < lineCount; i++)
        {
            lines[i] = tickArray[i].ToString();
        }

        // Use Task.Run with awaiter to prevent hanging tasks
        Task.Run(() => File.WriteAllLinesAsync(filePath, lines))
            .ConfigureAwait(false);

        // Add file path to collection
        FilePaths.Add(filePath);
    }
    public async Task<IEnumerable<Tick>> ReadTickFromDisk(string product, DateTime date)
    {
        var ticks = new List<Tick>();
        var filePath = ((IFileWriter)this).GetTickDataPath(product, date);
        if (filePath == "" || !File.Exists(filePath)) return null;
        var lines = await File.ReadAllLinesAsync(filePath);
        if (lines.Length < 1) return ticks;
        Array.ForEach(lines, line => { ticks.Add(line); });
        return ticks;
    }
}