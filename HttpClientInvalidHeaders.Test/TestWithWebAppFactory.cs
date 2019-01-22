using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HttpClientInvalidHeaders.Test
{
    public class TestWithWebAppFactory : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public TestWithWebAppFactory(WebApplicationFactory<Startup> webApplicationFactory)
        {
            _client = webApplicationFactory.CreateClient();
        }

        [Fact]
        public async Task CanSendRequest()
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("/api/values", UriKind.Relative)
            };
            request.Headers.TryAddWithoutValidation("User-Agent", "UserAgent: {K: V}");
            request.Content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.SendAsync(request);

            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public void CanAddHeader()
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("/api/values", UriKind.Relative)
            };

            bool headerAdded = request.Headers.TryAddWithoutValidation("User-Agent", "UserAgent: {K: V}");

            Assert.True(headerAdded);
        }
    }
}
