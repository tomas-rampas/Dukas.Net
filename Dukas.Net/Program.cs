using System;
using System.Text;
using System.Threading.Tasks;
using Bi5.Net;
using Bi5.Net.Models;
using CommandLine;

namespace Dukas.Net
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException +=
                (s, e) => Console.WriteLine(e);

            var parserResult = Parser.Default.ParseArguments<ResampleOptions, FetchOptions>(args)
                .MapResult(
                    (ResampleOptions opts) =>
                    {
                        ResampleData(opts);
                        return 0;
                    },
                    (FetchOptions opts) =>
                    {
                        FetchData(opts);
                        return 0;
                    },
                    errs => 1);
        }

        private static void ResampleData(ResampleOptions opts)
        {
            var ldr = new Loader(CheckCmdParamsAndCreateConfig(opts));
            Task.FromResult(ldr.ResampleAndFlush().Result);
        }

        private static void FetchData(FetchOptions opts)
        {
            var ldr = new Loader(CheckCmdParamsAndCreateConfig(opts));
            Task.FromResult(ldr.GetAndFlush().Result);
        }

        private static LoaderConfig CheckCmdParamsAndCreateConfig(CmdOptions opts)
        {
            LoaderConfig cfg = opts;
            if (cfg == null) throw new ApplicationException($"Config wos not created, check command line parameters.");
            return cfg;
        }

        private static readonly Func<string> DynamicData = () =>
        {
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
    }
}