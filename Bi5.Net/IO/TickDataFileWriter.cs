using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        protected override bool Write(string product, QuoteSide side, IEnumerable<Tick> data)
        {
            if (data == null || !data.Any()) throw new ArgumentException(null, nameof(data));
            var ticks = data as Tick[] ?? data.ToArray();
            var firstTickHour = ticks.First().Timestamp;
            var dataPath = Combine(FilePath, product, "Tick");
            Directory.CreateDirectory(dataPath);
            var filePath = Combine(dataPath, $"{firstTickHour:yyyyMMddHH}00.csv");
            IEnumerable<string> lines = ticks.Select(tick => tick.ToString());
            File.WriteAllLines(filePath, lines);
            _filePaths.Add(filePath);
            return true;
        }
    }
}