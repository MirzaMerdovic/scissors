using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ConsoleOut.Net.Http.Intercepting.Test.ComplexRoutes
{
    public sealed class ComplexRouteTests : IClassFixture<ComplexRouteFixture>
    {
        private ComplexRouteFixture _fixture;

        public ComplexRouteTests(ComplexRouteFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("/api/product/2/favorites")]
        [InlineData("/api/product/34/favorites")]
        [InlineData("/api/product/2/type/toys/details")]
        [InlineData("/api/product/33/type/shoes/details")]
        [InlineData("/api/product/9/type/2/details")]
        public async Task Should_Mock_Route(string path)
        {
            var client = _fixture.GetClient();

            var response = await client.GetAsync(path);
            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal("2", content);
        }

        [Theory]
        [InlineData("/api/product/2")]
        [InlineData("/api/product/2/favorites/1")]
        [InlineData("/api/product/2/type/toys/prices")]
        [InlineData("/api/product/33/type/shoes")]
        public async Task Should_Not_Mock_Route(string path)
        {
            var client = _fixture.GetClient();
            var get = client.GetAsync(path, CancellationToken.None);

            _ = await Assert.ThrowsAsync<HttpRequestException>(() => get);
        }
    }
}
