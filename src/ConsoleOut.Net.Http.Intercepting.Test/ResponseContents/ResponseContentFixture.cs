using System.Collections.Generic;

using static ConsoleOut.Net.Http.Intercepting.Test.InMemoryConfigurationWriter;

namespace ConsoleOut.Net.Http.Intercepting.Test.ResponseContents
{
    public sealed class ResponseContentFixture : TestFixture
    {
        public ResponseContentFixture() :
            base(new()
            {
                BuildMethodNamePair(0, "GET"),
                BuildPathPair(0, "/api/product/not-supported"),
                BuildHostPair(0, "*"),
                BuildReturnStatusCodePair(0, 521.ToString()),
                BuildReturnJsonContentPair(0, null),

                BuildMethodNamePair(1, "POST"),
                BuildPathPair(1, "/api/product/headers"),
                BuildHostPair(1, "*"),
                BuildReturnStatusCodePair(1, 201.ToString()),
                BuildReturnJsonContentPair(1, "{'id':42}")
            })
        {
        }
    }
}
