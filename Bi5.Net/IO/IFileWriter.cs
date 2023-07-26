using System;
using System.Collections;
using Bi5.Net.Models;

namespace Bi5.Net.IO;

internal interface IFileWriter
{
    /// <summary>
    /// Writes data to file
    /// </summary>
    /// <param name="product"></param>
    /// <param name="side">Bid, Ask, Both</param>
    /// <param name="data">List of prices</param>
    /// <returns>True if bool ended up successfully</returns>
    // ReSharper disable once UnusedMethodReturnValue.Global
    bool Write(string product, QuoteSide side, IEnumerable data);

    string GetTickDataPath(string product, QuoteSide side, DateTime tickHour);
}