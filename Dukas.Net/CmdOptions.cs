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
        [Option('e', "end-date", Required = false, HelpText = "Tick data end date")]
        public DateTime? EndDate { get; set; }
        [Option('p', "products", Required = true, HelpText = "List of tick data demanded products")]
        public IEnumerable<string> Products { get; set; }
        [Option("major-scale", Required = true, Default = DateTimePart.Day, HelpText = "Time Frame major scale")]
        public DateTimePart TimeFrameMajorScale { get; set; }
        [Option("minor-scale", Required = true, Default = 1, HelpText = "Time Frame minor scale")]
        public byte TimeFrameMinorScale { get; set; }

        public static implicit operator LoaderConfig(CmdOptions opts)
        {
            return new LoaderConfig(
                opts.StartDate, opts.EndDate, opts.Products, opts.TimeFrameMajorScale, opts.TimeFrameMinorScale
            );
        }
    }
}