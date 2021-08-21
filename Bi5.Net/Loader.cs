using Bi5.Net.Models;

namespace Bi5.Net
{
    public class Loader
    {
        private readonly LoaderConfig _cfg;
        
        protected Loader() {}

        public Loader(LoaderConfig cfg)
        {
            _cfg = cfg;
        }
    }
}