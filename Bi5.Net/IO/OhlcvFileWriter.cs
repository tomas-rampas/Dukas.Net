using System;
using System.Collections.Generic;
using System.Linq;
using Bi5.Net.Models;

namespace Bi5.Net.IO
{
    public class OhlcvFileWriter : FileWriter<Bar>
    {
        public OhlcvFileWriter(LoaderConfig loaderConfig):base(loaderConfig){}

        protected override bool Write(string product, IEnumerable<Bar> data)
        {
            switch (_fileScale)
            {
                case FileScale.Full:
                    IEnumerable<string> lines = data.Select(bar => bar.ToString());
                    System.IO.File.WriteAllLines(System.IO.Path.Combine(_filePath, $"{product}.csv"), lines);
                    return true;
                default:
                    Console.WriteLine($"The {_fileScale} writer is not implemented yet :( ");
                    return false;
            }
        }
    }
}