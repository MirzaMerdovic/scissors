using static Scissors.Test.InMemoryConfigurationWriter;

namespace Scissors.Test.ResponseContents
{
    public sealed class ResponseContentFixture : TestFixture
    {
        public ResponseContentFixture() :
            base(new()
            {
                BuildMethodNamePair(0, "GET"),
                BuildPathPair(0, "/api/product/not-supported"),
                BuildReturnStatusCodePair(0, 521.ToString()),
                BuildReturnJsonContentPair(0, null),

                BuildMethodNamePair(1, "POST"),
                BuildPathPair(1, "/api/product/2"),
                BuildReturnStatusCodePair(1, 201.ToString()),
                BuildReturnJsonContentPair(1, "{'id':42}"),

                BuildMethodNamePair(2, "POST"),
                BuildPathPair(2, "/api/product/2/headers"),
                BuildReturnStatusCodePair(2, 200.ToString()),
                BuildReturnJsonContentPair(2, null),
                BuildResponseHeaderNamePair(2, 0, "User-Agent"),
                BuildResponseHeaderValuePair(2, 0, "Test")
            })
        {
        }
    }
}
