using System.Collections.Generic;
using Bi5.Net.Models;

namespace Bi5.Net.IO
{
    public class TickDataFileWriter : FileWriter<Tick>
    {
        private TickDataFileWriter(){}

        public TickDataFileWriter(LoaderConfig cfg): base(cfg) {}
        protected override bool Write(string product, IEnumerable<Tick> data)
        {
            throw new System.NotImplementedException();
        }
    }
}