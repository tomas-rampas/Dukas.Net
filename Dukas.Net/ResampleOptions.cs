using CommandLine;

namespace Dukas.Net
{
    [Verb("resample", HelpText = "Perform resampling of existing Tick data; there is not any data downloaded",
        Hidden = false)]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ResampleOptions : CmdOptions
    {
    }
}