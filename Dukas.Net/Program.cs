﻿using System;
using System.IO;
using System.Linq;
using Bi5.Net.Models;
using Bi5.Net.Utils;

namespace Dukas.Net
{
    public static class Program
    {
        private const string DataUrl =
            "https://www.dukascopy.com/datafeed/EURGBP/2021/01/01/14h_ticks.bi5";

        private static void Main(string[] args)
        {
            var bytes = LzmaCompressor.DecompressLzmaFile(@"..\..\..\..\DataSamples\14h_ticks.bi5");

            var ticks = bytes.ToTickArray(new DateTime(2021, 1, 1), 5);

            var bars = ticks.GroupBy(t => t.Timestamp.Minute).Select(b=> new Bar
            {
                Minute = b.Key,
                Ticks = b.Count(),
                Timestamp = b.Last().Timestamp.Subtract(
                    new TimeSpan(0,0,b.Last().Timestamp.Second)),
                Open = b.OrderBy(x=>x.Timestamp).First().Bid,
                High = b.Max(x=>x.Bid),
                Low = b.Min(x=>x.Bid),
                Close = b.OrderBy(x=>x.Timestamp).Last().Bid,
                Volume = Math.Round(b.Sum(x=>x.BidVolume), 5)
            }).ToList();

            foreach (var bar in bars)
            {
                Console.WriteLine(bar);
            }
        }

        private static void CompressFileLzma(string inFile, string outFile)
        {
            var coder = new SevenZip.Compression.LZMA.Encoder();
            using var input = new FileStream(inFile, FileMode.Open);
            using var output = new FileStream(outFile, FileMode.Create);
            // Write the encoder properties
            coder.WriteCoderProperties(output);

            // Write the decompressed file size.
            output.Write(BitConverter.GetBytes(input.Length), 0, 8);

            // Encode the file.
            coder.Code(input, output, input.Length, -1, null);
            output.Flush();
            output.Close();
        }
    }
}