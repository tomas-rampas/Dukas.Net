using System;
using System.Linq;

namespace Bi5.Net.Models;

public class GroupedBars
{
    public IGrouping<DateTime, BarWithExtraDate> BarGroup { get; init; }
    public string FileFormat { get; init; }
}