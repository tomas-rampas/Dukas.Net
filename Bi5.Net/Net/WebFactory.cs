using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Bi5.Net.Tests")]

namespace Bi5.Net.Net
{
    public class WebFactory
    {
        private readonly IBi5HttpClient _client;

        public WebFactory()
        {
            _client = new Bi5HttpClient();
        }

        public WebFactory(IBi5HttpClient client)
        {
            _client = client;
        }

        public async Task<byte[]> DownloadTickDataFile(string uri)
        {
            if (!Uri.TryCreate(uri, UriKind.Absolute, out var uriResult))
                throw new InvalidOperationException("URI {uri} is invalid.");

            Debug.WriteLine(uriResult);
            using var httpResponse = await Task.Run(async () =>
                await _client.GetAsync(uriResult)
            );
            Debug.WriteLine(httpResponse.StatusCode);
            if (httpResponse.StatusCode != HttpStatusCode.OK)
                return Array.Empty<byte>();
            return await httpResponse.Content.ReadAsByteArrayAsync(CancellationToken.None);
        }
    }
}