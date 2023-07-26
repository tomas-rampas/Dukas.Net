using System;
using System.Collections;
using System.Collections.Generic;
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
    bool Write(string product, QuoteSide side, IEnumerable data);

    List<string> FilePaths { get; }
    string GetTickDataPath(string product, QuoteSide side, DateTime tickHour);
    string GetTickDataFolder(string product);
}