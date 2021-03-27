using static ConsoleOut.Net.Http.Intercepting.Test.InMemoryConfigurationWriter;

namespace ConsoleOut.Net.Http.Intercepting.Test.HostRoutes
{
    public sealed class HostRouteFixture : TestFixture
    {
        public HostRouteFixture() :
            base(new()
            {
                BuildMethodNamePair(0, "GET"),
                BuildPathPair(0, "/api/product/2"),
                BuildHostPair(0, "some-host.com"),
                BuildReturnStatusCodePair(0, 200.ToString()),
                BuildReturnJsonContentPair(0, "2"),

                BuildMethodNamePair(1, "GET"),
                BuildPathPair(1, "/api/product/2"),
                BuildHostPair(1, "some-host-2.com"),
                BuildReturnStatusCodePair(1, 200.ToString()),
                BuildReturnJsonContentPair(1, "two"),
            })
        {
        }
    }
}
