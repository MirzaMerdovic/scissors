using System.Collections.Generic;

using static Scissors.HttpRequestInterceptor.Test.InMemoryConfigurationWriter;

namespace Scissors.HttpRequestInterceptor.Test.ComplexRoutes
{
    public sealed class ComplexRouteFixture : TestFixture
    {
        public ComplexRouteFixture() :
            base(new List<KeyValuePair<string, string>>
            {
                BuildMethodNamePair(0, "GET"),
                BuildPathPair(0, "/api/product/*/favorites"),
                BuildReturnStatusCodePair(0, 200.ToString()),
                BuildReturnJsonContentPair(0, 2.ToString()),

                BuildMethodNamePair(1, "GET"),
                BuildPathPair(1, "/api/product/*/type/*/details"),
                BuildReturnStatusCodePair(1, 200.ToString()),
                BuildReturnJsonContentPair(1, 2.ToString())
            })
        {
        }
    }
}
