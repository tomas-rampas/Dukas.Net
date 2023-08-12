using CommandLine;

namespace Dukas.Net
{
    [Verb("fetch", isDefault: true, HelpText = "Fetch and Resample tick data of given products", Hidden = false)]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class FetchOptions : CmdOptions
    {
    }
}