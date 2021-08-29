using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bi5.Net.IO;
using Bi5.Net.Models;
using Bi5.Net.Net;
using Bi5.Net.Utils;

namespace Bi5.Net
{
    public class Loader
    {
        private string _dataUrl = "https://datafeed.dukascopy.com/datafeed/{0}/{1:0000}/{2:00}/{3:00}/{4:00}h_ticks.bi5";

        private readonly LoaderConfig _cfg;
        
        protected Loader() {}

        public Loader(LoaderConfig cfg)
        {
            _cfg = cfg;
        }

        /// <summary>
        /// Fetch data and write it to the fiven directory
        /// </summary>
        /// <returns>true if success; false otherwise</returns>
        public async Task<bool> GetAndFlush()
        {
#if DEBUG
            var watch = Stopwatch.StartNew();
#endif
            //TODO optimize this, it have to run in parallel but without overloading Dukascopy servers
            foreach (var product in _cfg.Products)
            {
                var timedData = await Get(product);
                var fileWriter = WriterFactory.CreateWriter(timedData, _cfg);
                fileWriter.Write(product, timedData);
#if DEBUG
                watch.Stop();
                Console.WriteLine($"Fetch Data Time Taken : {watch.ElapsedMilliseconds} ms.");
                //Array.ForEach(timedData.ToArray(), Console.WriteLine);
#endif
            }
            return true;
        }

        public async Task<IEnumerable<ITimedData>> Get(string product)
        {
            IEnumerable<ITimedData> result = ArraySegment<ITimedData>.Empty;
            
            var webFactory = new WebFactory();
            result = await Get(product, webFactory);
            
            // ReSharper disable once PossibleMultipleEnumeration
            return result;
        }

        private async Task<IEnumerable<ITimedData>> Get(string product, WebFactory webFactory)
        {
            IEnumerable<ITimedData> result = ArraySegment<ITimedData>.Empty;
            Console.WriteLine($"Loading {product} from {_cfg.StartDate:yyyy-MM-dd HH:mm:ss} to " +
                              $"{_cfg.EndDate:yyyy-MM-dd HH:mm:ss}");

            var tickData = Array.Empty<Tick>();
            var totalHours = (_cfg.EndDate - _cfg.StartDate).TotalHours;
            var totalHoursAligned = (int)totalHours + Convert.ToInt32(_cfg.EndDate.Ticks % _cfg.StartDate.Ticks > 0);
            Debug.WriteLine($"{totalHours}, {totalHoursAligned}");
            
            for (int i = 0; i <= totalHoursAligned; i++)
            {
                var currentTicks = await GetTicks(product, webFactory, i);
                Thread.Sleep(200);
                int startIndex = tickData.Length;
                Array.Resize(ref tickData, tickData.Length + currentTicks.Length);
                Array.Copy(currentTicks,0, tickData, startIndex, currentTicks.Length);
            }

            if (_cfg.TimeFrameMajorScale == DateTimePart.Tick) return tickData;
            
            result = tickData.Resample(_cfg.TimeFrameMajorScale, _cfg.TimeFrameMinorScale);
            
            return result;
        }

        private async Task<Tick[]> GetTicks(string product, WebFactory webFactory, int i)
        {
            var date = _cfg.StartDate.AddHours(i);
            var bi5DataUrl = string.Format(_dataUrl, product, date.Year, date.Month - 1, date.Day, date.Hour);
            Console.WriteLine(bi5DataUrl);
            byte[] compressedBi5 = await webFactory.DownloadTickDataFile(bi5DataUrl);
            if (compressedBi5 == null || compressedBi5.Length == 0) return Array.Empty<Tick>();
            Tick[] currentTicks = LzmaCompressor.DecompressLzmaBytes(compressedBi5)
                .ToTickArray(date, 5).ToArray();
            return currentTicks;
        }
    }
}