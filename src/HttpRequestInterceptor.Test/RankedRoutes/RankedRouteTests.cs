using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Scissors.HttpRequestInterceptor.Test.RankedRoutes
{
    public sealed class RankedRouteTests : IClassFixture<RankedRouteFixture>
    {
        private readonly RankedRouteFixture _fixture;

        public RankedRouteTests(RankedRouteFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("https://local1.host", "2")]
        [InlineData("https://local2.host", "two")]
        public async Task Should_Mock_Route_By_Host_And_Rank(string host, string result)
        {
            var client = _fixture.GetClient();
            client.BaseAddress = new Uri(host);

            var response = await client.GetAsync("/api/product/2", CancellationToken.None);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync(CancellationToken.None);
            Assert.Equal(result, content);
        }

        [Theory]
        [InlineData("/api/user/2", "1")]
        [InlineData("/api/user/43", "one")]
        public async Task Should_Mock_Route_By_Path_And_Rank(string path, string result)
        {
            var client = _fixture.GetClient();

            var response = await client.GetAsync(path, CancellationToken.None);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync(CancellationToken.None);
            Assert.Equal(result, content);
        }
    }
}
