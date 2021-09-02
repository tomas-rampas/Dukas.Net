using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bi5.Net.Net
{
    /// <summary>
    /// Custom Http client due allowing unit testing
    /// </summary>
    public class Bi5HttpClient : IBi5HttpClient
    {
        private static readonly HttpClient Client = new();
        
        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return await Client.GetAsync(requestUri);
        }
        public async Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            return await Client.GetAsync(requestUri);
        }

    }
}