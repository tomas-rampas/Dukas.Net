using System;
using Bi5.Net.Models;

namespace Bi5.Net.IO;

public class BarWithExtraDate
{
    public Bar Bar { get; set; }
    public DateTime BarDateNoTime { get; set; }
}