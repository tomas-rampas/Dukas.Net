using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bi5.Net.Net;

/// <summary>
/// Custom Http client due allowing unit testing
/// </summary>
[SuppressMessage("Performance", "CA1822:Mark members as static")]
public class Bi5HttpClient : IBi5HttpClient
{
    private static readonly HttpClient Client = new();

    // ReSharper disable once UnusedMember.Global
    public async Task<HttpResponseMessage> GetAsync(string requestUri)
    {
        return await Client.GetAsync(requestUri);
    }

    public async Task<HttpResponseMessage> GetAsync(Uri requestUri)
    {
        return await Client.GetAsync(requestUri);
    }
}