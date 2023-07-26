using System;
using System.Linq;

namespace Bi5.Net.Models;

public class GroupedBars
{
    public IGrouping<DateTime, BarWithExtraDate> BarGroup { get; set; }
    public string FileFormat { get; set; }
}