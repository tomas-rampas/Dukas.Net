using System.Collections.Generic;
using Bi5.Net.Models;

namespace Bi5.Net.IO
{
    public abstract class FileWriter<T> where T : ITimedData
    {
        protected readonly LoaderConfig _cfg;

        protected FileWriter() {}
        
        public FileWriter(LoaderConfig cfg)
        {
            _cfg = cfg;
        }

        protected abstract bool Write(IEnumerable<T> data);
    }
}