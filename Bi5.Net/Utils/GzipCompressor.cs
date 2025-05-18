using System;
using System.IO;
using System.IO.Compression;

namespace Bi5.Net.Utils
{

public static class GzipCompressor
{
    public static void GzipStream(string fileToGZip)
    {
        var fileToBeGZipped = new FileInfo(fileToGZip);
        var gzipFileName = new FileInfo(string.Concat(fileToGZip, ".gz"));

        using var fileToBeZippedAsStream = fileToBeGZipped.OpenRead();
        using var gzipTargetAsStream = gzipFileName.Create();
        using var gzipStream = new GZipStream(gzipTargetAsStream, CompressionMode.Compress);

        try
        {
            fileToBeZippedAsStream.CopyTo(gzipStream);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}}
