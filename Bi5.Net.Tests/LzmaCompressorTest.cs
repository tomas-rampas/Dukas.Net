using System;
using System.IO;
using Bi5.Net.Utils;
using Xunit;

namespace Bi5.Net.Tests
{
    public class LzmaCompressorTest
    {
        private const string SampleDataFile = @"..\..\..\..\DataSamples\14h_ticks.bi5"; 
        [Fact]
        public void Check_Decompress_Bi5_File_Test()
        {
            var bytes = LzmaCompressor.DecompressLzmaFile(SampleDataFile);
            Assert.NotNull(bytes);
            Assert.NotEmpty(bytes);
        }

        [Fact]
        public void Check_Decompress_Bi5_File_Wrong_Parameters_Test()
        {
            Assert.Throws<ArgumentException>(() => LzmaCompressor.DecompressLzmaFile(""));
            Assert.Throws<FileNotFoundException>(() => LzmaCompressor.DecompressLzmaFile("..\\abc.xyz"));
        }
        
        [Fact]
        public void Check_Decompress_Bi5_Stream_Test()
        {
            using var iStream = new FileStream(SampleDataFile,FileMode.Open);
            var bytes = LzmaCompressor.DecompressLzmaStream(iStream);
            Assert.NotNull(bytes);
            Assert.NotEmpty(bytes);
        }
        
        [Fact]
        public void Check_Decompress_Bi5_Stream_Wrong_Parameters_Test()
        {
            Assert.Throws<ArgumentNullException>(() => LzmaCompressor.DecompressLzmaStream(null));
        }

    }
}