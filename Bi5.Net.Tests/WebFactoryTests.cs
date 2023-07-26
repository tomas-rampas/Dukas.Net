using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bi5.Net.Net;
using NSubstitute;
using Xunit;

namespace Bi5.Net.Tests;

public class WebFactoryTests
{
    private const string SampleDataFile = @"./DataSamples/14h_ticks.bi5";

    [Fact]
    public async Task Download_TickData_Test()
    {
        var httpClient = Substitute.For<IBi5HttpClient>();
        var contentBytes = await File.ReadAllBytesAsync(SampleDataFile);
        httpClient.GetAsync(Arg.Any<Uri>()).Returns(
            new HttpResponseMessage
            {
                Content = new ByteArrayContent(contentBytes)
            });
        var factory = new WebFactory(httpClient);
        var response = await factory.DownloadTickDataFile("http://localhost");
        Assert.NotNull(response);
        Assert.True(response.Length > 0);
        Assert.True(response.SequenceEqual(contentBytes));
    }
}