using System;
using System.IO;

namespace Bi5.Net.Utils
{
    public static class LzmaCompressor
    {
        public static byte[] DecompressLzmaFile(string inFile)
        {
            if (string.IsNullOrWhiteSpace(inFile)) throw new ArgumentException("Empty file path", nameof(inFile));
            if (!File.Exists(inFile)) throw new FileNotFoundException("File does not exist", inFile);
            
            using var input = new FileStream(inFile, FileMode.Open);
            return  DecompressLzmaStream(input);
        }

        public static byte[] DecompressLzmaStream(Stream inStream)
        {
            if (inStream == null) throw new ArgumentNullException(nameof(inStream));
            
            using var output = new MemoryStream();
            DecodeBi5Stream(inStream, output);
            output.Position = 0;
            var data = new byte[output.Length];
            output.Read(data, 0, data.Length);
            return data;
        }

        private static void DecodeBi5Stream(Stream inStream, Stream outStream)
        {
            var coder = new SevenZip.Compression.LZMA.Decoder();

            var properties = new byte[5];
            inStream.Read(properties, 0, 5);

            var fileLengthBytes = new byte[8];
            inStream.Read(fileLengthBytes, 0, 8);
            var fileLength = BitConverter.ToInt64(fileLengthBytes, 0);
            coder.SetDecoderProperties(properties);
            coder.Code(inStream, outStream, inStream.Length, fileLength, null);
        }
    }
}