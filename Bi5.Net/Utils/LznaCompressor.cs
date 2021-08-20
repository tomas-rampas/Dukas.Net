using System;
using System.IO;

namespace Bi5.Net.Utils
{
    public class LznaCompressor
    {
        public static byte[] DecompressFileLZMA(string inFile)
        {
            var coder = new SevenZip.Compression.LZMA.Decoder();
            using var input = new FileStream(inFile, FileMode.Open);
            using var output = new MemoryStream();

            var properties = new byte[5];
            input.Read(properties, 0, 5);

            var fileLengthBytes = new byte[8];
            input.Read(fileLengthBytes, 0, 8);
            var fileLength = BitConverter.ToInt64(fileLengthBytes, 0);
            coder.SetDecoderProperties(properties);
            coder.Code(input, output, input.Length, fileLength, null);
            output.Position = 0;
            var data = new byte[output.Length];
            output.Read(data, 0, data.Length);
            return data;
        }
        
    }
}