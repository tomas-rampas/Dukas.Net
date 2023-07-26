using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bi5.Net.Models;
using Bi5.Net.Utils;

namespace Bi5.Net.IO;

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
                        .Select(b => new BarWithExtraDate
                            {
                                Bar = b,
                                BarDateNoTime = new DateTime(b.Timestamp.Year, b.Timestamp.Month, b.Timestamp.Day),
                            }
                        )
                        .GroupBy(x => x.BarDateNoTime)
                        .Select((g, i) => new GroupedBars
                            {
                                BarGroup = g,
                                FileFormat = "yyyyMMdd"
                            }
                        );
                WriteFileScaledGroupedBars(side, groups, dirPath);
                return true;
            case FileScale.Month:
                groups =
                    data
                        .Select(b => new BarWithExtraDate
                            {
                                Bar = b,
                                BarDateNoTime = new DateTime(b.Timestamp.Year, b.Timestamp.Month, 1),
                            }
                        )
                        .GroupBy(x => x.BarDateNoTime)
                        .Select((g, i) => new GroupedBars
                            {
                                BarGroup = g,
                                FileFormat = "yyyyMM"
                            }
                        );
                WriteFileScaledGroupedBars(side, groups, dirPath);

                return true;
            case FileScale.Year:
                groups =
                    data
                        .Select(b => new BarWithExtraDate
                            {
                                Bar = b,
                                BarDateNoTime = new DateTime(b.Timestamp.Year, 1, 1),
                            }
                        )
                        .GroupBy(x => x.BarDateNoTime)
                        .Select((g, i) => new GroupedBars
                            {
                                BarGroup = g,
                                FileFormat = "yyyy"
                            }
                        );
                WriteFileScaledGroupedBars(side, groups, dirPath);

                return true;
            case FileScale.Min:
            case FileScale.Hour:
            case FileScale.Week:
            default:
                Console.WriteLine($"The {FileScale} writer is not implemented yet :( ");
                return false;
        }
    }

    private void WriteFileScaledGroupedBars(QuoteSide side, IEnumerable<GroupedBars> groups, string dirPath)
    {
        foreach (var group in groups)
        {
            IEnumerable<string> groupData = group.BarGroup.Select(bar => bar.Bar.ToString());
            var fileName = Path.Combine(dirPath,
                $"{group.BarGroup.Key.ToString(group.FileFormat)}_{side.ToString()}.csv");
            File.WriteAllLines(fileName, groupData);
            if (!Compress) continue;
            GzipCompressor.GzipStream(fileName);
            File.Delete(fileName);
        }
    }
}