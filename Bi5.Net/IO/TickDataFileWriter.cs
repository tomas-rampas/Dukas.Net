using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bi5.Net.Models;
using static System.IO.Path;

namespace Bi5.Net.IO
{
    public class TickDataFileWriter : FileWriter<Tick>
    {
        private TickDataFileWriter()
        {
        }

        public TickDataFileWriter(LoaderConfig cfg) : base(cfg)
        {
        }

        [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
        [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
        protected override bool Write(string product, QuoteSide side, IEnumerable<Tick> data)
        {
            if (data == null || !data.Any()) throw new ArgumentException(null, nameof(data));
            var ticks = data as Tick[] ?? data.ToArray();
            var firstTickHour = ticks.First().Timestamp;
            var dataPath = Combine(FilePath, product, "Tick");
            Directory.CreateDirectory(dataPath);
            var filePath = Combine(dataPath, $"{firstTickHour:yyyyMMddHH}00.csv");
            IEnumerable<string> lines = ticks.Select(tick => tick.ToString());
            Task.Run(() => File.WriteAllLinesAsync(filePath, lines));
            _filePaths.Add(filePath);
            return true;
        }

        public async Task<IEnumerable<Tick>> ReadTickFromDisk(string product, QuoteSide side, DateTime date)
        {
            List<Tick> ticks = new List<Tick>();
            string filePath = ((IFileWriter)this).GetTickDataPath(product, side, date);
            if (filePath == "" || !File.Exists(filePath)) return null;
            string[] lines = await File.ReadAllLinesAsync(filePath);
            if (lines.Length < 1) return ticks;
            Array.ForEach(lines, (line) => { ticks.Add(line); });
            return ticks;
        }
    }
}