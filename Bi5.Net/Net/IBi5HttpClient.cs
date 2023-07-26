using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bi5.Net.Net;

/// <summary>
/// Simple interface for HttpClient wrapper
/// </summary>
public interface IBi5HttpClient
{
    Task<HttpResponseMessage> GetAsync(string requestUri);
    Task<HttpResponseMessage> GetAsync(Uri requestUri);
}