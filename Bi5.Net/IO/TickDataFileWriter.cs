using System.Collections.Generic;
using Bi5.Net.Models;

namespace Bi5.Net.IO
{
    public class TickDataFileWriter : FileWriter<Tick>
    {
        protected override bool Write(IEnumerable<Tick> data)
        {
            throw new System.NotImplementedException();
        }
    }
}