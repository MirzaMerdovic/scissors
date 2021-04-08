using System.Collections.Generic;

namespace Scissors.HttpRequestInterceptor.Test
{
    internal static class InMemoryConfigurationWriter
    {
        private const string Column = ":";
        private const string Headers = "Headers";
        private const string SectionName = nameof(HttpInterceptorOptions) + Column;

        // HttpInterceptorOptions:0:
        private static string BasePath(int index) => $"{SectionName}{index}{Column}";

        // Headers:0:
        private static string BaseHeaderPath(int index) => $"{Headers}{Column}{index}{Column}";

        // HttpInterceptorOptions:0:MethodName
        public static KeyValuePair<string, string> BuildMethodNamePair(int index, string value) =>
            new($"{BasePath(index)}{nameof(HttpInterceptorOptions.MethodName)}", value);

        // HttpInterceptorOptions:0:Host
        public static KeyValuePair<string, string> BuildHostPair(int index, string value) =>
            new($"{BasePath(index)}{nameof(HttpInterceptorOptions.Host)}", value);

        // HttpInterceptorOptions:0:Path
        public static KeyValuePair<string, string> BuildPathPair(int index, string value) =>
            new($"{BasePath(index)}{nameof(HttpInterceptorOptions.Path)}", value);

        // HttpInterceptorOptions:0:ResponseStatusCode
        public static KeyValuePair<string, string> BuildReturnStatusCodePair(int index, string value) =>
            new($"{BasePath(index)}{nameof(HttpInterceptorOptions.ResponseStatusCode)}", value);

        // HttpInterceptorOptions:0:ResponseJsonContent
        public static KeyValuePair<string, string> BuildReturnJsonContentPair(int index, string value) =>
            new($"{BasePath(index)}{nameof(HttpInterceptorOptions.ResponseJsonContent)}", value);

        // HttpInterceptorOptions:0:Rank
        public static KeyValuePair<string, string> BuildRankPair(int index, string value) =>
            new($"{BasePath(index)}{nameof(HttpInterceptorOptions.Rank)}", value);

        // HttpInterceptorOptions:0:Headers:0:Name
        public static KeyValuePair<string, string> BuildResponseHeaderNamePair(int parentIndex, int index, string value) =>
            new($"{BasePath(parentIndex)}{BaseHeaderPath(index)}{nameof(HttpInterceptorOptions.HttpResponseHeader.Name)}", value);

        // HttpInterceptorOptions:0:Headers:0:Value
        public static KeyValuePair<string, string> BuildResponseHeaderValuePair(int parentIndex, int index, string value) =>
            new($"{BasePath(parentIndex)}{BaseHeaderPath(index)}{nameof(HttpInterceptorOptions.HttpResponseHeader.Value)}", value);
    }
}
