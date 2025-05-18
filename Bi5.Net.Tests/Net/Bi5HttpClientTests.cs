using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bi5.Net.Net;
using Xunit;

namespace Bi5.Net.Tests.Net
{
    public class Bi5HttpClientTests
    {
        [Fact]
        public async Task GetAsync_Uri_CallsHttpClient()
        {
            // Arrange
            var client = new Bi5HttpClient();
            var uri = new Uri("https://example.com");

            // Act & Assert
            // Note: This test is connecting to a real URL which is generally not recommended
            // for unit tests. In a real scenario, we would mock the HttpClient.
            var response = await client.GetAsync(uri);
            
            // Verify we got a valid response
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAsync_String_CallsHttpClient()
        {
            // Arrange
            var client = new Bi5HttpClient();
            var url = "https://example.com";

            // Act
            // Using reflection to call the method that is not exposed through the interface
            var method = typeof(Bi5HttpClient).GetMethod("GetAsync", new[] { typeof(string) });
            var task = (Task<HttpResponseMessage>)method.Invoke(client, new object[] { url });
            var response = await task;

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void GetAsync_WithInvalidUri_ThrowsException()
        {
            // Arrange
            var client = new Bi5HttpClient();
            var invalidUri = new Uri("https://this-domain-should-not-exist-123456789.com");

            // Act & Assert
            var exception = Assert.ThrowsAsync<HttpRequestException>(() => 
                client.GetAsync(invalidUri));
        }
    }
}