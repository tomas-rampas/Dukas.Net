using CommandLine;

namespace Dukas.Net
{
    [Verb("resample", HelpText = "Perform resampling of existing Tick data; there is not any data downloaded")]
    public class ResampleOptions : CmdOptions
    {
    }
}