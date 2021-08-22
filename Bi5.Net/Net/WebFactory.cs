using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Bi5.Net.Net
{
    public class WebFactory : IWebFactory
    {
        private static readonly HttpClient Client = new();

        static WebFactory() {} 

        public async Task<byte[]> DownloadFile(string uri)
        {
            if (!Uri.TryCreate(uri, UriKind.Absolute, out var uriResult)) 
                throw new InvalidOperationException("URI is invalid.");
            
            Debug.WriteLine(uriResult);
            using HttpResponseMessage httpResponse = await Task.Run(async ()=> await Client.GetAsync(uriResult));
            return await httpResponse.Content.ReadAsByteArrayAsync(CancellationToken.None);
        }
    }
}