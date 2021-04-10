using System.Threading.Tasks;
using Xunit;

namespace Scissors.Test.ResponseStatuses
{
    public sealed class ResponseStatusTests : IClassFixture<ResponseStatusFixture>
    {
        private ResponseStatusFixture _fixture;

        public ResponseStatusTests(ResponseStatusFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("/api/product/bad-request", 400, "Name is required")]
        [InlineData("/api/product/unauthorized", 401, "")]
        [InlineData("/api/product/forbidden", 403, "")]
        [InlineData("/api/product/not-found", 404, "")]
        [InlineData("/api/product/request-timeout", 408, "")]
        [InlineData("/api/product/server-error", 500, "")]
        [InlineData("/api/product/bad-gateway", 502, "")]
        [InlineData("/api/product/gateway-timeout", 504, "")]
        public async Task Shoul_Return_Error_Status(string path, int statusCode, string responseContent)
        {
            var client = _fixture.GetClient();

            var response = await client.GetAsync(path);
            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal(statusCode, (int)response.StatusCode);
            Assert.Equal(responseContent, content);
        }
    }
}
