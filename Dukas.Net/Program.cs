using System;
using System.Collections.Generic;
using System.Text;
using Bi5.Net;
using Bi5.Net.Models;
using CommandLine;
using CommandLine.Text;

//-s 2020-01-01 -e 2020-12-31 -p EURUSD,GBPUSD,BTCUSD,DEUIDFEUR --major-scale Min --minor-scale 1 
namespace Dukas.Net
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            var parserResult = Parser.Default.ParseArguments<CmdOptions>(args);
            parserResult
                .WithParsed(InitiateLoader)
                .WithNotParsed(errs => DisplayHelp(parserResult, errs));
        }

        private static void InitiateLoader(CmdOptions cmdOpts)
        {
            LoaderConfig cfg = cmdOpts;
            Console.WriteLine(cfg.QuoteSide.ToString());
            if (cfg == null) throw new ApplicationException($"Loader config wos not created");
            var ldr = new Loader(cfg);
            //TODO to be continued
        }

        private static readonly Func<string> DynamicData = () => {
            var sb = new StringBuilder();
            sb.AppendLine("-------  Sample Usage  ----------\n");
            sb.AppendLine(
                "Get 1 Minute OHLCV Bid-side data between 1st of Jan. 2020 and 31st of Dec. 2020 " +
                "for given list of products; store the data into the C:\\temp\n");
            sb.AppendLine(
                "\tdukas.net -s 2020-01-01 -e 2020-12-31 -p \"EURUSD,GBPUSD,BTCUSD,DEUIDXEUR\" " +
                "--major-scale Min --minor-scale 1 -o \"c:\\temp\" -q Bid");
            return $"{sb}";
        };

        static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errs)
        {
            var helpText = HelpText.AutoBuild(result, h =>
            {
                h.AddPostOptionsLine(DynamicData());
                return h;
            }, e => e);
            Console.WriteLine(helpText);
        }        
    }
}