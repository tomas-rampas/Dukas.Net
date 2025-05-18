using System;
using System.IO;
using System.Linq;
using Bi5.Net.Utils;
using Xunit;

namespace Bi5.Net.Tests
{

public class LzmaCompressorTest
{
    private const string SampleDataFile = "./DataSamples/14h_ticks.bi5";
    private const string ResultDataFile = "./DataSamples/14h_ticks.bin";
    private const string LzmaBytesDataFile = "./DataSamples/14h_ticks.lzma";

    [Fact]
    public void Check_Decompress_Bi5_File_Test()
    {
        var expectedResult = Convert.FromBase64String(File.ReadAllText(ResultDataFile));
        var result = LzmaCompressor.DecompressLzmaFile(SampleDataFile);
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
        var expectedResult = Convert.FromBase64String(File.ReadAllText(ResultDataFile));

        using var iStream = new FileStream(SampleDataFile, FileMode.Open);
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
        var expectedResult = Convert.FromBase64String(File.ReadAllText(ResultDataFile));
        var lzmaBytes = Convert.FromBase64String(File.ReadAllText(LzmaBytesDataFile));

        var result = LzmaCompressor.DecompressLzmaBytes(lzmaBytes);
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.True(result.SequenceEqual(expectedResult));
    }
}}
