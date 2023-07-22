#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bi5.Net.IO;
using Bi5.Net.Models;
using Bi5.Net.Net;
using Bi5.Net.Products;
using Bi5.Net.Utils;
using static Bi5.Net.Utils.DateTimeUtils;

namespace Bi5.Net
{
    public class Loader
    {
        /// <summary>
        /// Max degree of parallelism defines number of threads for data processing
        /// </summary>
        private static readonly int MaxDegreeOfParallelism = Environment.ProcessorCount - 1;

        /// <summary>
        /// Dukacopy data endpoint format template 
        /// </summary>
        private readonly string _dataUrl =
            "https://datafeed.dukascopy.com/datafeed/{0}/{1:0000}/{2:00}/{3:00}/{4:00}h_ticks.bi5";

        private readonly LoaderConfig _cfg = null!;
        private readonly IFileWriter _tickDataFileWriter = null!;

        private Loader()
        {
        }

        public Loader(LoaderConfig cfg)
        {
            _cfg = cfg;
            _tickDataFileWriter = new TickDataFileWriter(_cfg);
        }

        /// <summary>
        /// Fetch data and write it to the given directory
        /// </summary>
        /// <returns>true if success; false otherwise</returns>
        public async Task<bool> GetAndFlush()
        {
            var watch = Stopwatch.StartNew();
            var products = DukascopyProducts.Catalogue
                .Where(x => _cfg.Products.Any(p => p == x.Key)
                            || _cfg.Products.All(p => p.ToUpper() == "ALL"))
                .Select(x => x.Value).ToList();

            CheckProductsInCatalogue(products);

            await products
                .ToAsyncEnumerable()
                .AsyncParallelForEach(FetchAndGet, MaxDegreeOfParallelism, TaskScheduler.Default);

            watch.Stop();
            var timeSpan = TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds);
            Console.WriteLine(
                $"Fetch Data Took  " +
                $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}.{timeSpan.Milliseconds:D3}");
            return true;
        }

        private void CheckProductsInCatalogue(IEnumerable<Product> products)
        {
            var unknownProducts = _cfg.Products.Where(_ => !products.Any()).ToArray();
            if (unknownProducts.Any())
            {
                Console.WriteLine("Undefined products:");
                Array.ForEach(unknownProducts, Console.WriteLine);
            }
        }

        public async Task<bool> ResampleAndFlush()
        {
            var watch = Stopwatch.StartNew();
            var products = DukascopyProducts.Catalogue
                .Where(x => _cfg.Products.Any(p => p == x.Key)
                            || _cfg.Products.All(p => p.ToUpper() == "ALL"))
                .Select(x => x.Value).ToList();

            CheckProductsInCatalogue(products);

            await products
                .ToAsyncEnumerable()
                .AsyncParallelForEach(LoadAndGet, MaxDegreeOfParallelism, TaskScheduler.Default);

            watch.Stop();
            var timeSpan = TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds);
            Console.WriteLine(
                $"Fetch Data Took  " +
                $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}.{timeSpan.Milliseconds:D3}");
            return true;
        }

        private async Task<IEnumerable<ITimedData>> LoadAndGet(Product product)
        {
            //var ticks = await GetTicksFromDisk(product, _tickDataFileWriter, DateTime.Now);
            var tickData = Array.Empty<Tick>();
            var lastEndIndex = 0;
            var webFactory = new WebFactory();

            await foreach (ITimedData[]? currentTicks in Fetch(product, webFactory, true))
            {
                if (currentTicks == null || currentTicks.Length < 1) continue;
                int startIndex = lastEndIndex;
                int requiredSize = lastEndIndex + currentTicks.Length;

                if (tickData.Length < requiredSize)
                {
                    Array.Resize(ref tickData, requiredSize);
                }

                Array.Copy(currentTicks, 0, tickData,
                    startIndex, currentTicks.Length);

                lastEndIndex += currentTicks.Length;
            }

            FlushData(product.Name, tickData);

            return tickData;
        }

        /// <summary>
        /// Get data for product
        /// </summary>
        /// <param name="product">Product to get data for</param>
        /// <returns>Array of Tick data</returns>
        private async Task<IEnumerable<ITimedData>> FetchAndGet(Product product)
        {
            var tickData = Array.Empty<Tick>();

            WebFactory webFactory = new();
            var lastEndIndex = 0;

            await foreach (ITimedData[]? currentTicks in Fetch(product, webFactory))
            {
                if (currentTicks == null || currentTicks.Length < 1) continue;

                var firstTick = currentTicks.FirstOrDefault(x => true);
                var currentDay = firstTick?.Timestamp.Date.Day;
                var lastTick = tickData.LastOrDefault(x => true);
                if (lastTick != null && lastTick.Timestamp.Date.Day != currentDay)
                {
                    FlushTicks(product, tickData, lastTick);
                    lastEndIndex = 0;
                }

                int startIndex = lastEndIndex;
                int requiredSize = lastEndIndex + currentTicks.Length;

                if (tickData.Length < requiredSize)
                {
                    Array.Resize(ref tickData, requiredSize);
                }

                Array.Copy(currentTicks, 0, tickData,
                    startIndex, currentTicks.Length);

                lastEndIndex += currentTicks.Length;

                // as soon as full day completed, create file and flush content to it
                if (lastEndIndex > 0 && lastTick != null && IsLastHour(lastTick.Timestamp, _cfg.UseMarketDate))
                {
                    FlushTicks(product, tickData, lastTick);
                    lastEndIndex = 0;
                }
            }

            if (tickData.Any()) FlushTicks(product, tickData, tickData.Last());

            // ReSharper disable once PossibleMultipleEnumeration
            return tickData;
        }

        private void FlushTicks(Product product, Tick[] tickData, Tick? lastTick)
        {
            FlushData(product.Name, tickData);
            Console.WriteLine($"Product {product.Name} - Last Date: {lastTick?.Timestamp:yyyy-MM-dd HH:mm:ss}");
            Array.Clear(tickData, 0, tickData.Length);
        }

        private void FlushData(string product, Tick[] tickData)
        {
            FlushData(product, tickData, QuoteSide.Bid);
            FlushData(product, tickData, QuoteSide.Ask);
        }

        private void FlushData(string product, IEnumerable<Tick> tickData, QuoteSide side)
        {
            var result = tickData.Where(x => x != null)
                .Resample(_cfg.TimeFrameMajorScale, _cfg.TimeFrameMinorScale, side);
            if (result != null)
            {
                var fileWriter = WriterFactory.CreateWriter(result, _cfg);
                fileWriter.Write(product, side, result);
            }
        }

        private async IAsyncEnumerable<ITimedData[]?> Fetch(Product product, WebFactory webFactory,
            bool onlyFromDisk = false)
        {
            var startDate = _cfg.StartDate; //CalculateEffectiveDate(_cfg.StartDate);
            var endDate = _cfg.EndDate; // CalculateEffectiveDate(_cfg.EndDate, true);

            IEnumerable<ITimedData> result = ArraySegment<ITimedData>.Empty;
            Console.WriteLine($"Loading {product.Name} from {startDate:yyyy-MM-dd HH:mm:ss} to " +
                              $"{endDate:yyyy-MM-dd HH:mm:ss}");

            var totalHours = (endDate - startDate).TotalHours;
            var totalHoursAligned = (int)totalHours + Convert.ToInt32(endDate.Ticks % startDate.Ticks > 0);
            Debug.WriteLine($"{totalHours}, {totalHoursAligned}");

            for (int hourOffset = 0; hourOffset < totalHours; hourOffset++)
            {
                var date = startDate.AddHours(hourOffset);
                var lastHour = GetLastHour(date, _cfg.UseMarketDate);

                // try to read existing ticks from disk rather than request new data from web
                var ticks = await GetTicksFromDisk(product, _tickDataFileWriter, date);
                if (ticks != null && ticks.Any())
                {
                    yield return ticks;
                }

                if (onlyFromDisk)
                {
                    continue;
                }

                ITimedData[]? currentTicks = await GetTicks(product, webFactory, date);
                Thread.Sleep(50);
                if (currentTicks != null && currentTicks.Any())
                {
                    // store hourly tick data
                    _tickDataFileWriter.Write(product.Name, QuoteSide.Both, currentTicks);
                }

                if (lastHour == date.Hour)
                {
                    Debug.WriteLine($"{date:yyyy-MM-dd HH:mm} finished");
                }

                yield return currentTicks;
            }
        }

        private async Task<ITimedData[]?> GetTicksFromDisk(Product product, IFileWriter tickDataFileWriter,
            DateTime date)
        {
            IEnumerable<Tick> ticksFromDisk =
                await ((TickDataFileWriter)tickDataFileWriter).ReadTickFromDisk(product.Name, QuoteSide.Both, date);
            if (ticksFromDisk == null) return null;
            var ticks = ticksFromDisk as ITimedData[] ?? ticksFromDisk.ToArray();
            return ticks;
        }

        private async Task<Tick[]?> GetTicks(Product product, WebFactory webFactory, DateTime date)
        {
            var bi5DataUrl = string.Format(_dataUrl, product.Name, date.Year, date.Month - 1, date.Day, date.Hour);
            Console.WriteLine(bi5DataUrl);
            byte[] compressedBi5 = await webFactory.DownloadTickDataFile(bi5DataUrl);
            if (compressedBi5 == null || compressedBi5.Length == 0) return Array.Empty<Tick>();
            Tick[]? currentTicks = LzmaCompressor
                .DecompressLzmaBytes(compressedBi5)
                .ToTickArray(date, product.Decimals).ToArray();
            return currentTicks;
        }
    }
}