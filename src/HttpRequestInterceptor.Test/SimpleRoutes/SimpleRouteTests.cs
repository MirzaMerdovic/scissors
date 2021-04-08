using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Scissors.HttpRequestInterceptor.Test.SimpleRoutes
{
    public class SimpleRouteTests : IClassFixture<SimpleRouteFixture>
    {
        private SimpleRouteFixture _fixture;

        public SimpleRouteTests(SimpleRouteFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("/api/product?id=2")]
        [InlineData("/api/product/2")]
        public async Task Should_Mock_Route(string path)
        {
            var client = _fixture.GetClient();

            var response = await client.GetAsync(path);
            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal("2", content);
        }

        [Theory]
        [InlineData("/api/product/56")]
        [InlineData("/api/product/2/favorites")]
        [InlineData("/api/product?id=56")]
        [InlineData("/api/product?id=56&type=discount")]
        public async Task Should_Not_Mock_Route(string path)
        {
            var client = _fixture.GetClient();
            var get = client.GetAsync(path, CancellationToken.None);

            _ = await Assert.ThrowsAsync<HttpRequestException>(() => get);
        }

        [Fact]
        public async Task Should_Mock_Paths_Withotu_Slash()
        {
            var client = _fixture.GetClient();

            var response = await client.GetAsync("api/user/11");
            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal("11", content);
        }
    }
}
