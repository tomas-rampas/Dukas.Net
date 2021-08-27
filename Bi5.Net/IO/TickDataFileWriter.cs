using System.Collections.Generic;
using Bi5.Net.Models;

namespace Bi5.Net.IO
{
    public class TickDataFileWriter : FileWriter<Tick>
    {
        internal override bool Write(IEnumerable<Tick> data)
        {
            throw new System.NotImplementedException();
        }
    }
}