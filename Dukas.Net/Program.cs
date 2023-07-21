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
        public static async Task<int> Main(params string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException +=
                (s, e) => Console.WriteLine(e);

            var result = await (await Parser.Default.ParseArguments<ResampleOptions, FetchOptions>(args)
                .WithParsedAsync<ResampleOptions>(ResampleData))
                .WithParsedAsync<FetchOptions>(FetchData);
            return 0;
        }

        private static async Task<bool> ResampleData(ResampleOptions opts)
        {
            if (opts == null) throw new ArgumentNullException(nameof(opts));
            var ldr = new Loader(CheckCmdParamsAndCreateConfig(opts));
            return await ldr.ResampleAndFlush();
        }

        private static async Task<bool> FetchData(FetchOptions opts)
        {
            var ldr = new Loader(CheckCmdParamsAndCreateConfig(opts));
            return await ldr.GetAndFlush();
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
                "\tdukas.net fetch -s 2020-01-01 -e 2020-12-31 -p \"EURUSD,GBPUSD,BTCUSD,DEUIDXEUR\" " +
                "--major-scale Min --minor-scale 1 -o \"c:\\temp\" -q Bid");
            return $"{sb}";
        };
    }
}