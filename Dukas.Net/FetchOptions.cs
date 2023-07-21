using CommandLine;
using CommandLine.Text;

namespace Dukas.Net
{
    [Verb("fetch", isDefault: true, HelpText = "Fetch and Resample tick data of given products", Hidden = false)]
    public class FetchOptions : CmdOptions
    {
    }
}