using System;
using System.IO;
using System.Linq;
using Bi5.Net.Utils;
using Xunit;

namespace Bi5.Net.Tests;

public class LzmaCompressorTest
{
    private const string SAMPLE_DATA_FILE = @"./DataSamples/14h_ticks.bi5";
    private const string RESULT_DATA_FILE = @"./DataSamples/14h_ticks.bin";
    private const string LZMA_BYTES_DATA_FILE = @"./DataSamples/14h_ticks.lzma";

    [Fact]
    public void Check_Decompress_Bi5_File_Test()
    {
        byte[] expectedResult = Convert.FromBase64String(File.ReadAllText(RESULT_DATA_FILE));
        var result = LzmaCompressor.DecompressLzmaFile(SAMPLE_DATA_FILE);
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.True(result.SequenceEqual(expectedResult));
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
        byte[] expectedResult = Convert.FromBase64String(File.ReadAllText(RESULT_DATA_FILE));

        using var iStream = new FileStream(SAMPLE_DATA_FILE, FileMode.Open);
        var result = LzmaCompressor.DecompressLzmaStream(iStream);
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.True(result.SequenceEqual(expectedResult));
    }

    [Fact]
    public void Check_Decompress_Bi5_Stream_Wrong_Parameters_Test()
    {
        Assert.Throws<ArgumentNullException>(() => LzmaCompressor.DecompressLzmaStream(null));
    }

    [Fact]
    public void Check_Decompress_Byte_Array()
    {
        byte[] expectedResult = Convert.FromBase64String(File.ReadAllText(RESULT_DATA_FILE));
        byte[] lzmaBytes = Convert.FromBase64String(File.ReadAllText(LZMA_BYTES_DATA_FILE));

        byte[] result = LzmaCompressor.DecompressLzmaBytes(lzmaBytes);
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.True(result.SequenceEqual(expectedResult));
    }
}