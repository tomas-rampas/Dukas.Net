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
using static Bi5.Net.Utils.DateTimeUtils;

namespace Bi5.Net
{
    public class Loader
    {
        private string _dataUrl =
            "https://datafeed.dukascopy.com/datafeed/{0}/{1:0000}/{2:00}/{3:00}/{4:00}h_ticks.bi5";

        private readonly LoaderConfig _cfg;

        protected Loader()
        {
        }

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
                //var fileWriter = WriterFactory.CreateWriter(timedData, _cfg);
                //fileWriter.Write(product, timedData);
            }
#if DEBUG
            watch.Stop();
            Console.WriteLine($"Fetch Data Taken : {watch.ElapsedMilliseconds} ms.");
            //Array.ForEach(timedData.ToArray(), Console.WriteLine);
#endif
            return true;
        }

        /// <summary>
        /// Fetch data
        /// </summary>
        /// <param name="product">Product to fetch data for</param>
        /// <returns></returns>
        public async Task<IEnumerable<ITimedData>> Get(string product)
        {
            Tick[] tickData = Array.Empty<Tick>();

            var webFactory = new WebFactory();
            await foreach (ITimedData[] currentTicks in Fetch(product, webFactory))
            {
                int startIndex = tickData.Length;
                Array.Resize(ref tickData, tickData.Length + currentTicks.Length);
                Array.Copy(currentTicks, 0, tickData, startIndex, currentTicks.Length);

                // once full day arrived, create file and flush content to it
                if (tickData.Length > 0 && IsLastHour(tickData.Last().Timestamp))
                {
                    var result = tickData.Resample(_cfg.TimeFrameMajorScale, _cfg.TimeFrameMinorScale);
                    Console.WriteLine($"Last Date: {tickData.Last().Timestamp:yyyy-MM-dd HH:mm:ss}");
                    var fileWriter = WriterFactory.CreateWriter(result, _cfg);
                    fileWriter.Write(product, result);
                    tickData = Array.Empty<Tick>();
                }
            }

            // ReSharper disable once PossibleMultipleEnumeration
            return tickData;
        }

        private async IAsyncEnumerable<ITimedData[]> Fetch(string product, WebFactory webFactory)
        {
            IFileWriter tickDataFileWriter = new TickDataFileWriter(_cfg);

            var startDate = CalculateEffectiveDate(_cfg.StartDate);
            var endDate = CalculateEffectiveDate(_cfg.EndDate, true);

            IEnumerable<ITimedData> result = ArraySegment<ITimedData>.Empty;
            Console.WriteLine($"Loading {product} from {startDate:yyyy-MM-dd HH:mm:ss} to " +
                              $"{endDate:yyyy-MM-dd HH:mm:ss}");

            var tickData = Array.Empty<Tick>();
            var totalHours = (endDate - startDate).TotalHours;
            var totalHoursAligned = (int)totalHours + Convert.ToInt32(endDate.Ticks % startDate.Ticks > 0);
            Debug.WriteLine($"{totalHours}, {totalHoursAligned}");

            for (int hourOffset = 0; hourOffset < totalHours; hourOffset++)
            {
                var date = startDate.AddHours(hourOffset);
                var lastHour = GetLastHour(date);
                ITimedData[] currentTicks = await GetTicks(product, webFactory, date);
                Thread.Sleep(50);
                if (currentTicks.Any())
                {
                    int startIndex = tickData.Length;
                    Array.Resize(ref tickData, tickData.Length + currentTicks.Length);
                    Array.Copy(currentTicks, 0, tickData, startIndex, currentTicks.Length);
                    // store hourly tick data
                    tickDataFileWriter.Write(product, currentTicks);
                }

                if (lastHour == date.Hour)
                {
                    Debug.WriteLine($"{date:yyyy-MM-dd HH:mm} finished");
                }

                yield return currentTicks;
            }

            // if (_cfg.TimeFrameMajorScale == DateTimePart.Tick) return tickDataFileWriter.FilePaths;
            //
            // result = tickData.Resample(_cfg.TimeFrameMajorScale, _cfg.TimeFrameMinorScale);
            //
            // return result;
        }

        private async Task<Tick[]> GetTicks(string product, WebFactory webFactory, DateTime date)
        {
            var bi5DataUrl = string.Format(_dataUrl, product, date.Year, date.Month - 1, date.Day, date.Hour);
            Console.WriteLine(bi5DataUrl);
            byte[] compressedBi5 = await webFactory.DownloadTickDataFile(bi5DataUrl);
            if (compressedBi5 == null || compressedBi5.Length == 0) return Array.Empty<Tick>();
            Tick[] currentTicks = LzmaCompressor
                .DecompressLzmaBytes(compressedBi5)
                .ToTickArray(date, 5).ToArray();
            return currentTicks;
        }
    }
}