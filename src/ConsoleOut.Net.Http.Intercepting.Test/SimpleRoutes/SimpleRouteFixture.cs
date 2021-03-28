using System.Collections.Generic;

using static ConsoleOut.Net.Http.Intercepting.Test.InMemoryConfigurationWriter;

namespace ConsoleOut.Net.Http.Intercepting.Test.SimpleRoutes
{
    public sealed class SimpleRouteFixture : TestFixture
    {
        public SimpleRouteFixture() :
            base(new List<KeyValuePair<string, string>>
            {
                BuildMethodNamePair(0, "GET"),
                BuildPathPair(0, "/api/product?id=2"),
                BuildHostPair(0, "*"),
                BuildReturnStatusCodePair(0, 200.ToString()),
                BuildReturnJsonContentPair(0, 2.ToString()),

                BuildMethodNamePair(1, "GET"),
                BuildPathPair(1, "/api/product/2"),
                BuildHostPair(1, "*"),
                BuildReturnStatusCodePair(1, 200.ToString()),
                BuildReturnJsonContentPair(1, 2.ToString()),

                BuildMethodNamePair(2, "GET"),
                BuildPathPair(2, "api/user/*"),
                BuildHostPair(2, "*"),
                BuildReturnStatusCodePair(2, 200.ToString()),
                BuildReturnJsonContentPair(2, 11.ToString())
            })
        {
        }
    }
}