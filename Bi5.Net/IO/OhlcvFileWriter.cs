using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bi5.Net.Models;
using Bi5.Net.Utils;

namespace Bi5.Net.IO
{
    public class OhlcvFileWriter : FileWriter<Bar>
    {
        public OhlcvFileWriter(LoaderConfig loaderConfig) : base(loaderConfig)
        {
        }

        protected override bool Write(string product, QuoteSide side, IEnumerable<Bar> data)
        {
            var dirPath = Path.Combine(FilePath, product, TimeFrame);
            Directory.CreateDirectory(dirPath);

            switch (FileScale)
            {
                case FileScale.Full:
                    IEnumerable<string> lines = data.Select(bar => bar.ToString());
                    File.WriteAllLines(Path.Combine(FilePath, $"{product}.csv"), lines);
                    return true;
                case FileScale.Day:
                    var groups =
                        data
                            .Select(b => new
                                {
                                    Bar = b,
                                    BarDateNoTime = new DateTime(b.Timestamp.Year, b.Timestamp.Month, b.Timestamp.Day),
                                }
                            )
                            .GroupBy(x => new { x.BarDateNoTime.Year, x.BarDateNoTime.Month, x.BarDateNoTime.Day })
                            .Select((g, i) => new
                                {
                                    BarGroup = g,
                                    BarDateNoTime = new DateTime(g.Key.Year, g.Key.Month, g.Key.Day)
                                }
                            );
                    foreach (var group in groups)
                    {
                        IEnumerable<string> groupData = group.BarGroup.Select(bar => bar.Bar.ToString());
                        var fileName = Path.Combine(dirPath,
                            $"{group.BarDateNoTime:yyyyMMdd}_{side.ToString()}.csv");
                        File.WriteAllLines(fileName, groupData);
                        GzipCompressor.GzipStream(fileName);
                        File.Delete(fileName);
                    }

                    return true;
                case FileScale.Month:
                    var groupsMonth =
                        data
                            .Select(b => new
                                {
                                    Bar = b,
                                    BarDateNoTime = new DateTime(b.Timestamp.Year, b.Timestamp.Month, b.Timestamp.Day),
                                }
                            )
                            .GroupBy(x => new { x.BarDateNoTime.Year, x.BarDateNoTime.Month })
                            .Select((g, i) => new
                                {
                                    BarGroup = g,
                                    BarDateNoTime = new DateTime(g.Key.Year, g.Key.Month, 1)
                                }
                            );
                    foreach (var group in groupsMonth)
                    {
                        IEnumerable<string> groupData = group.BarGroup.Select(bar => bar.Bar.ToString());
                        var fileName = Path.Combine(dirPath,
                            $"{group.BarDateNoTime:yyyyMM}_{side.ToString()}.csv");
                        File.WriteAllLines(fileName, groupData);
                        GzipCompressor.GzipStream(fileName);
                        File.Delete(fileName);
                    }

                    return true;
                case FileScale.Year:
                    var groupsYear =
                        data
                            .Select(b => new
                                {
                                    Bar = b,
                                    BarDateNoTime = new DateTime(b.Timestamp.Year, b.Timestamp.Month, b.Timestamp.Day),
                                }
                            )
                            .GroupBy(x => new { x.BarDateNoTime.Year })
                            .Select((g, i) => new
                                {
                                    BarGroup = g,
                                    BarDateNoTime = new DateTime(g.Key.Year, 1, 1)
                                }
                            );
                    foreach (var group in groupsYear)
                    {
                        IEnumerable<string> groupData = group.BarGroup.Select(bar => bar.Bar.ToString());
                        var fileName = Path.Combine(dirPath,
                            $"{group.BarDateNoTime:yyyy}_{side.ToString()}.csv");
                        File.WriteAllLines(fileName, groupData);
                        //TODO
                        GzipCompressor.GzipStream(fileName);
                        File.Delete(fileName);
                    }

                    return true;
                case FileScale.Min:
                case FileScale.Hour:
                case FileScale.Week:
                default:
                    Console.WriteLine($"The {FileScale} writer is not implemented yet :( ");
                    return false;
            }
        }
    }
}