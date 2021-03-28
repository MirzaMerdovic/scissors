using static ConsoleOut.Net.Http.Intercepting.Test.InMemoryConfigurationWriter;

namespace ConsoleOut.Net.Http.Intercepting.Test.RankedRoutes
{
    public class RankedRouteFixture : TestFixture
    {
        public RankedRouteFixture() :
            base(new()
            {
                BuildMethodNamePair(0, "GET"),
                BuildPathPair(0, "/api/product/2"),
                BuildHostPair(0, "local1.host"),
                BuildReturnStatusCodePair(0, 200.ToString()),
                BuildReturnJsonContentPair(0, "2"),
                BuildRankPair(0, 0.ToString()),

                BuildMethodNamePair(1, "GET"),
                BuildPathPair(1, "/api/product/2"),
                BuildHostPair(1, "*"),
                BuildReturnStatusCodePair(1, 200.ToString()),
                BuildReturnJsonContentPair(1, "two"),
                BuildRankPair(1, 10.ToString()),

                BuildMethodNamePair(2, "GET"),
                BuildPathPair(2, "/api/user/2"),
                BuildHostPair(2, "*"),
                BuildReturnStatusCodePair(2, 200.ToString()),
                BuildReturnJsonContentPair(2, "1"),
                BuildRankPair(2, 0.ToString()),

                BuildMethodNamePair(3, "GET"),
                BuildPathPair(3, "/api/user/*"),
                BuildHostPair(3, "*"),
                BuildReturnStatusCodePair(3, 200.ToString()),
                BuildReturnJsonContentPair(3, "one"),
                BuildRankPair(3, 10.ToString()),
            })
        {
        }
    }
}
