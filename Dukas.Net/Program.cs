using System;
using Bi5.Net.Models;
using CommandLine;

namespace Dukas.Net
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            LoaderConfig cfg;
            Parser.Default.ParseArguments<CmdOptions>(args).WithParsed<CmdOptions>(
                o =>
                {
                    cfg = o;
                }
            );
        }
    }
}