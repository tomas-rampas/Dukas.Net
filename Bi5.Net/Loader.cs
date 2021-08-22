using Bi5.Net.Models;

namespace Bi5.Net
{
    public class Loader
    {
        private const string DataUrl =
            "https://www.dukascopy.com/datafeed/EURGBP/2021/01/01/14h_ticks.bi5";

        private readonly LoaderConfig _cfg;
        
        protected Loader() {}

        public Loader(LoaderConfig cfg)
        {
            _cfg = cfg;
        }
    }
}