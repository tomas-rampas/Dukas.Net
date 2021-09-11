using System;
using System.IO;
using System.IO.Compression;

namespace Bi5.Net.Utils
{
    public class GzipCompressor
    {
        public static void GzipStream(string fileToGZip)
        {
            var fileToBeGZipped = new FileInfo(fileToGZip);
            var gzipFileName = new FileInfo(string.Concat(fileToGZip, ".gz"));

            using FileStream fileToBeZippedAsStream = fileToBeGZipped.OpenRead();
            using FileStream gzipTargetAsStream = gzipFileName.Create();
            using GZipStream gzipStream = new GZipStream(gzipTargetAsStream, CompressionMode.Compress);

            try
            {
                fileToBeZippedAsStream.CopyTo(gzipStream);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}