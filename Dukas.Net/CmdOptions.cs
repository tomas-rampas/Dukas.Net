using System;
using System.Collections.Generic;
using Bi5.Net.Models;
using CommandLine;

namespace Dukas.Net
{
    public class CmdOptions
    {
        [Option('s', "start-date", Required = true, HelpText = "Tick data start date")]
        public DateTime StartDate { get; set; }
        
        [Option('e', "end-date", Required = false, HelpText = "Tick data end date. Current date time is used when argument not provided")]
        public DateTime? EndDate { get; set; }
        
        [Option('p', "products", Required = true, Separator=',', HelpText = "List of tick data demanded products")]
        public IEnumerable<string> Products { get; set; }

        [Option('q', "quote-side", Required = false, Default = QuoteSide.Bid,
            HelpText = "Requested Quote side. Possible values: Bid, Ask, Both")]
        public QuoteSide QuoteSide { get; set; }
        
        [Option("major-scale", Required = false, Default = DateTimePart.Day, 
            HelpText = "Time Frame major scale. Possible values: Sec, Min, Hour, Day, Week, Month, Year")]
        public DateTimePart TimeFrameMajorScale { get; set; }
        
        [Option("minor-scale", Required = false, Default = (uint)1, HelpText = "Time Frame minor scale")]
        public uint TimeFrameMinorScale { get; set; }

        [Option('o', "output-dir", Required = true, HelpText = "Output data directory")]
        public string OutputFolder { get; set; }
        
        [Option('f', "file-scale", Default = FileScale.Day, Required = true)]
        public FileScale FileScale { get; set; }
        [Option('h', "header", Default = false, HelpText = "Include header in resulting data file")]
        public bool IncHeader { get; set; }
        [Option('g', "gzip", Default = false, HelpText = "Compress result")]
        public bool GzipResult { get; set; }

        public static implicit operator LoaderConfig(CmdOptions opts)
        {
            return new LoaderConfig(
                opts.StartDate, opts.EndDate ?? DateTime.Now, opts.Products, opts.TimeFrameMajorScale, 
                opts.TimeFrameMinorScale, opts.OutputFolder, opts.QuoteSide, opts.FileScale, opts.IncHeader,
                opts.GzipResult
            );
        }
    }
}