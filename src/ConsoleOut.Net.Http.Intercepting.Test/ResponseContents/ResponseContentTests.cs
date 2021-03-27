using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ConsoleOut.Net.Http.Intercepting.Test.ResponseContents
{
    public class ResponseContentTests : IClassFixture<ResponseContentFixture>
    {
        private readonly ResponseContentFixture _fixture;

        public ResponseContentTests(ResponseContentFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("/api/product/not-supported")]
        public async Task Should_Throw_NotSupported_Exception(string path)
        {
            var client = _fixture.GetClient();

            var get = client.GetAsync(path, CancellationToken.None);

            _ = await Assert.ThrowsAsync<NotSupportedException>(() => get);
        }

        [Fact]
        public async Task Should_Return_201_With_Response()
        {
            var requestContent = JsonConvert.SerializeObject(new { });
            using var stringContent = new StringContent(requestContent);

            var client = _fixture.GetClient();
            var response = await client.PostAsync("/api/product/2", stringContent);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var definition = new { id = 0 };
            var payload = await response.Content.ReadAsStringAsync();

            var content = JsonConvert.DeserializeAnonymousType(payload, definition);
            Assert.Equal(42, content.id);
        }


        [Fact]
        public async Task Should_Return_200_With_Headers()
        {
            var requestContent = JsonConvert.SerializeObject(new { });
            using var stringContent = new StringContent(requestContent);

            var client = _fixture.GetClient();
            var response = await client.PostAsync("/api/product/2/headers", stringContent);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var headers = response.Headers;
            Assert.Single(headers);

            var header = headers.First();
            Assert.Equal("User-Agent", header.Key);
            Assert.Equal("Test", header.Value.First());
        }
    }
}
