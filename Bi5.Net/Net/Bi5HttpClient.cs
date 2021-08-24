using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bi5.Net.Net
{
    /// <summary>
    /// Custom Http client due to allow unit testing
    /// </summary>
    public class Bi5HttpClient : IBi5HttpClient
    {
        private static HttpClient _client = new();
        
        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return await _client.GetAsync(requestUri);
        }
        public async Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            return await _client.GetAsync(requestUri);
        }

    }
}