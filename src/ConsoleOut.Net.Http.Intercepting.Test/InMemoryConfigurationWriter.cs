using System.Collections.Generic;

namespace ConsoleOut.Net.Http.Intercepting.Test
{
    internal static class InMemoryConfigurationWriter
    {
        private const string Column = ":";

        private const string SectionName = nameof(HttpInterceptorOptions) + Column;

        private const string MethodNameKeyPath = Column + nameof(HttpInterceptorOptions.MethodName);
        private const string HostKeyPath = Column + nameof(HttpInterceptorOptions.Host);
        private const string PathKeyPath = Column + nameof(HttpInterceptorOptions.Path);
        private const string ReturnStatusCodeKeyPath = Column + nameof(HttpInterceptorOptions.ReturnStatusCode);
        private const string ReturnJsonContentKeyPath = Column + nameof(HttpInterceptorOptions.ReturnJsonContent);

        //HttpInterceptorOptions:0:MethodName
        public static KeyValuePair<string, string> BuildMethodNamePair(int index, string value) => new($"{SectionName}{index}{MethodNameKeyPath}", value);

        public static KeyValuePair<string, string> BuildHostPair(int index, string value) => new($"{SectionName}{index}{HostKeyPath}", value);

        public static KeyValuePair<string, string> BuildPathPair(int index, string value) => new($"{SectionName}{index}{PathKeyPath}", value);

        public static KeyValuePair<string, string> BuildReturnStatusCodePair(int index, string value) => new($"{SectionName}{index}{ReturnStatusCodeKeyPath}", value);

        public static KeyValuePair<string, string> BuildReturnJsonContentPair(int index, string value) => new($"{SectionName}{index}{ReturnJsonContentKeyPath}", value);
    }
}
