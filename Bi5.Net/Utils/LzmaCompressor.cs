using System;
using System.IO;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Bi5.Net.Tests")]

namespace Bi5.Net.Utils
{
    public static class LzmaCompressor
    {
        /// <summary>
        /// Decompresses compressed file on given path
        /// </summary>
        /// <param name="inFile">Path to the compressed file</param>
        /// <returns>Decompressed and decoded binary data</returns>
        /// <exception cref="ArgumentException">File path must not be empty</exception>
        /// <exception cref="FileNotFoundException">File must exists</exception>
        public static byte[] DecompressLzmaFile(string inFile)
        {
            if (string.IsNullOrWhiteSpace(inFile)) throw new ArgumentException("Empty file path", nameof(inFile));
            if (!File.Exists(inFile)) throw new FileNotFoundException("File does not exist", inFile);

            using var input = new FileStream(inFile, FileMode.Open);
            return DecompressLzmaStream(input);
        }

        /// <summary>
        /// Decompresses compressed data stream 
        /// </summary>
        /// <param name="inStream">Stream with data</param>
        /// <returns>Decompressed and decoded binary data</returns>
        /// <exception cref="ArgumentNullException">Stream must exists</exception>
        internal static byte[] DecompressLzmaStream(Stream inStream)
        {
            if (inStream == null) throw new ArgumentNullException(nameof(inStream));

            using var output = new MemoryStream();
            DecodeLzmaStream(inStream, output);
            output.Position = 0;
            var data = new byte[output.Length];
            output.Read(data, 0, data.Length);
            return data;
        }

        /// <summary>
        /// Decompress binary data
        /// </summary>
        /// <param name="inBytes">Byte array with data</param>
        /// <returns>Decompressed and decoded binary data</returns>
        /// <exception cref="ArgumentNullException">Byte of array must not be null</exception>
        internal static byte[] DecompressLzmaBytes(byte[] inBytes)
        {
            if (inBytes == null) throw new ArgumentNullException(nameof(inBytes));

            using var inStream = new MemoryStream();
            inStream.Write(inBytes);
            inStream.Position = 0;
            return DecompressLzmaStream(inStream);
        }

        /// <summary>
        /// Reads data from input stream, decode it, and put result in output stream. 
        /// </summary>
        /// <param name="inStream">Input Stream</param>
        /// <param name="outStream">Output Stream</param>
        private static void DecodeLzmaStream(Stream inStream, Stream outStream)
        {
            var coder = new SevenZip.Compression.LZMA.Decoder();

            var properties = new byte[5];
            var propertiesBytesRead = inStream.Read(properties, 0, 5);
            if (propertiesBytesRead < properties.Length)
            {
                throw new ApplicationException("Properties not defined");
            }
            var fileLengthBytes = new byte[8];
            inStream.Read(fileLengthBytes, 0, 8);
            var fileLength = BitConverter.ToInt64(fileLengthBytes, 0);
            coder.SetDecoderProperties(properties);
            coder.Code(inStream, outStream, inStream.Length, fileLength, null);
        }

        /// <summary>
        /// This method is not used and so future of this method is unclear. It'll be remove in the future. Likely,
        /// </summary>
        /// <param name="inFile">Output file</param>
        /// <param name="outFile">Output file</param>
        private static void CompressFileLzma(string inFile, string outFile)
        {
            var coder = new SevenZip.Compression.LZMA.Encoder();
            using var input = new FileStream(inFile, FileMode.Open);
            using var output = new FileStream(outFile, FileMode.Create);
            coder.WriteCoderProperties(output);

            output.Write(BitConverter.GetBytes(input.Length), 0, 8);

            coder.Code(input, output, input.Length, -1, null);
            output.Flush();
            output.Close();
        }
    }
}