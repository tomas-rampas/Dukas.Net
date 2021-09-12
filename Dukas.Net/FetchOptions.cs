using CommandLine;

namespace Dukas.Net
{
    [Verb("fetch", isDefault: true, HelpText = "Fetch and Resample tick data of given products")]
    public class FetchOptions : CmdOptions
    {
    }
}