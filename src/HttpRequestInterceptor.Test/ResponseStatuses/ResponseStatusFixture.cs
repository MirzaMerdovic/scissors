using static Scissors.HttpRequestInterceptor.Test.InMemoryConfigurationWriter;

namespace Scissors.HttpRequestInterceptor.Test.ResponseStatuses
{
    public sealed class ResponseStatusFixture : TestFixture
    {
        public ResponseStatusFixture() :
            base(new()
            {
                BuildMethodNamePair(0, "GET"),
                BuildPathPair(0, "/api/product/bad-request"),
                BuildReturnStatusCodePair(0, 400.ToString()),
                BuildReturnJsonContentPair(0, "Name is required"),

                BuildMethodNamePair(1, "GET"),
                BuildPathPair(1, "/api/product/unauthorized"),
                BuildReturnStatusCodePair(1, 401.ToString()),
                BuildReturnJsonContentPair(1, null),

                BuildMethodNamePair(2, "GET"),
                BuildPathPair(2, "/api/product/forbidden"),
                BuildReturnStatusCodePair(2, 403.ToString()),
                BuildReturnJsonContentPair(2, null),

                BuildMethodNamePair(3, "GET"),
                BuildPathPair(3, "/api/product/not-found"),
                BuildReturnStatusCodePair(3, 404.ToString()),
                BuildReturnJsonContentPair(3, null),

                BuildMethodNamePair(4, "GET"),
                BuildPathPair(4, "/api/product/request-timeout"),
                BuildReturnStatusCodePair(4, 408.ToString()),
                BuildReturnJsonContentPair(4, null),

                BuildMethodNamePair(5, "GET"),
                BuildPathPair(5, "/api/product/server-error"),
                BuildReturnStatusCodePair(5, 500.ToString()),
                BuildReturnJsonContentPair(5, null),

                BuildMethodNamePair(6, "GET"),
                BuildPathPair(6, "/api/product/bad-gateway"),
                BuildReturnStatusCodePair(6, 502.ToString()),
                BuildReturnJsonContentPair(6, null),

                BuildMethodNamePair(7, "GET"),
                BuildPathPair(7, "/api/product/gateway-timeout"),
                BuildReturnStatusCodePair(7, 504.ToString()),
                BuildReturnJsonContentPair(7, null)
            })
        {
        }
    }
}