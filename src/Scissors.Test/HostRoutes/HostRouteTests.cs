using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Scissors.Test.HostRoutes
{
    public class HostRouteTests : IClassFixture<HostRouteFixture>
    {
        private readonly HostRouteFixture _fixture;

        public HostRouteTests(HostRouteFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("https://some-host.com", "/api/product/2", "2")]
        [InlineData("https://some-host-2.com", "/api/product/2", "two")]
        public async Task Should_Mock_Route(string host, string path, string result)
        {
            var client = _fixture.GetClient();
            client.BaseAddress = new Uri(host);

            var response = await client.GetAsync(path, CancellationToken.None);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(result, content);
        }

        [Fact]
        public async Task Should_Not_Mock_Route()
        {
            var client = _fixture.GetClient();
            client.BaseAddress = new Uri("https://smth.com");

            var get = client.GetAsync("/api/product/2", CancellationToken.None);

            _ = await Assert.ThrowsAsync<HttpRequestException>(() => get);
        }
    }
}
