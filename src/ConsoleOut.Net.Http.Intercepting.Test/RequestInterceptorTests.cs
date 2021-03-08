using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ConsoleOut.Net.Http.Intercepting.Test
{
    public class RequestInterceptorTests : IClassFixture<TestFixture>
    {
        private TestFixture _fixture;

        public RequestInterceptorTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("/api/product?id=2")]
        [InlineData("/api/product/2")]
        [InlineData("/api/customer/12/product?id=2")]
        public async Task ShouldMockGet(string path)
        {
            var response = await _fixture.Client.GetAsync(path);
            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal("2", content);
        }

        [Theory]
        [InlineData("/api/customer/56", "/api/customer/56/bookmarks")]
        [InlineData("/api/customer/1", "/api/customer/1/bookmarks")]
        [InlineData("/api/customer/johnsmith", "/api/customer/johnsmith/bookmarks")]
        public async Task ShouldMockRoute(string validPath, string invalidPath)
        {
            var response = await _fixture.Client.GetAsync(validPath);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            await Assert.ThrowsAsync<HttpRequestException>(() => _fixture.Client.GetAsync(invalidPath));
        }

        [Theory]
        [InlineData("/api/customer/56/addresses", "/api/customer/56/addresses/2")]
        [InlineData("/api/customer/4/products/23/details", "/api/customer/4/products/23/details/1")]
        public async Task ShouldMockComplexRoute(string validPath, string invalidPath)
        {
            var response = await _fixture.Client.GetAsync(validPath);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            await Assert.ThrowsAsync<HttpRequestException>(() => _fixture.Client.GetAsync(invalidPath));
        }

        [Fact]
        public async Task ShouldThrowHttpRequestExceptionForNotInterecptedRoute()
        {
            await Assert.ThrowsAsync<HttpRequestException>(() => _fixture.Client.GetAsync("/api/product/23"));
        }

        [Fact]
        public async Task ShouldMockNotFount()
        {
            var response = await _fixture.Client.GetAsync("/api/product/404");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal("Product with the Id: 404 doesn't exist.", content);
        }

        [Fact]
        public async Task ShouldMockBadRequest()
        {
            var requestContent = JsonConvert.SerializeObject(new { Description = "Test", Price = 2 });
            using (var stringContent = new StringContent(requestContent))
            {
                var response = await _fixture.Client.PostAsync("api/product", stringContent);
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

                var content = await response.Content.ReadAsStringAsync();
                Assert.Equal("Name is required", content);
            }
        }

        [Fact]
        public async Task ShouldMockInternalServerError()
        {
            var requestContent = JsonConvert.SerializeObject(new { });
            using (var stringContent = new StringContent(requestContent))
            {
                var response = await _fixture.Client.PutAsync("/api/product", stringContent);
                Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            }
        }

        [Fact]
        public async Task ShouldMockUnauthorizedRequest()
        {
            var response = await _fixture.Client.GetAsync("/api/product/forbidden");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ShouldMockBadGateway()
        {
            var response = await _fixture.Client.GetAsync("/api/product/bad");
            Assert.Equal(HttpStatusCode.BadGateway, response.StatusCode);
        }

        [Fact]
        public async Task ShouldMockTimeout()
        {
            var response = await _fixture.Client.GetAsync("/api/product/timeout");
            Assert.Equal(HttpStatusCode.GatewayTimeout, response.StatusCode);
        }

        [Fact]
        public async Task ShouldThrowNotSupportedException()
        {
            await Assert.ThrowsAsync<NotSupportedException>(() => _fixture.Client.GetAsync("/api/product/not-supported"));
        }

        [Fact]
        public async Task ShouldMock201WithResponseHeaders()
        {
            var requestContent = JsonConvert.SerializeObject(new { });
            using var stringContent = new StringContent(requestContent);

            var response = await _fixture.Client.PostAsync("/api/product/headers", stringContent);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var definition = new { id = 0 };
            var payload = await response.Content.ReadAsStringAsync();

            var content = JsonConvert.DeserializeAnonymousType(payload, definition);
            Assert.Equal(42, content.id);
            Assert.True(response.Headers.TryGetValues("test-header", out var headers));
            Assert.Single(headers);
            Assert.Equal("test", headers.First());
        }
    }
}