using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bi5.Net.Models;

namespace Bi5.Net.IO
{
    public class OhlcvFileWriter : FileWriter<Bar>
    {
        public OhlcvFileWriter(LoaderConfig loaderConfig):base(loaderConfig){}

        protected override bool Write(string product, IEnumerable<Bar> data)
        {
            switch (FileScale)
            {
                case FileScale.Full:
                    IEnumerable<string> lines = data.Select(bar => bar.ToString());
                    File.WriteAllLines(Path.Combine(FilePath, $"{product}.csv"), lines);
                    return true;
                case FileScale.Day:
                    var dirPath = Path.Combine(FilePath, product, TimeFrame);
                    Directory.CreateDirectory(dirPath);
                    var groups = data.GroupBy(x => new
                    {
                        //BarDateNoTime = new DateTime(x.Timestamp.Year, x.Timestamp.Month, x.Timestamp.Day),
                        BarDateNoTime = x.GroupDate,
                    });
                    foreach (var @group in groups)
                    {
                        IEnumerable<string> groupData = group.Select(bar => bar.ToString());
                        File.WriteAllLines(Path.Combine(dirPath, $"{@group.Key.BarDateNoTime:yyyyMMdd}.csv"), groupData);
                    }

                    return true;
                default:
                    Console.WriteLine($"The {FileScale} writer is not implemented yet :( ");
                    return false;
            }
        }
    }
}