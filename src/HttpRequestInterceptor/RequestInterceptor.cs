using Microsoft.Extensions.Options;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Scissors.HttpRequestInterceptor
{
    public sealed class RequestsInterceptor : DelegatingHandler
    {
        private const string Slash = "/";
        private const string WildCard = "*";

        private static readonly char[] _splitter = Slash.ToCharArray();

        private readonly Collection<HttpInterceptorOptions> _options;

        public RequestsInterceptor(IOptions<Collection<HttpInterceptorOptions>> options)
        {
            _options = options?.Value ?? new Collection<HttpInterceptorOptions>();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            var method = request.Method.Method;
            var uri = request.RequestUri;

            var options = _options.Where(x => x.MethodName.Equals(method, StringComparison.InvariantCultureIgnoreCase));
            var option = options.Where(x => IsConfigurationMatch(x, uri)).OrderBy(x => x.Rank).FirstOrDefault();

            if (option != null)
                return option.TryCreateResponse();

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        private static bool IsConfigurationMatch(HttpInterceptorOptions option, Uri requestUri)
        {
            var host = requestUri.Host;
            var pathAndQuery = requestUri.PathAndQuery;

            if (!option.Host.Equals(WildCard) && !host.Equals(option.Host, StringComparison.CurrentCultureIgnoreCase))
                return false;

            var path = option.Path.StartsWith(Slash, StringComparison.InvariantCultureIgnoreCase) ? option.Path : $"/{option.Path}";

            if (path.Equals(pathAndQuery, StringComparison.InvariantCultureIgnoreCase))
                return true;

            if (!path.Contains(WildCard))
                return false;

            var segmentsToMatch = path.Split(_splitter, StringSplitOptions.RemoveEmptyEntries);
            var querySegments = pathAndQuery.Split(_splitter, StringSplitOptions.RemoveEmptyEntries);

            var isMatch = segmentsToMatch.Length.Equals(querySegments.Length);

            if (!isMatch)
                return isMatch;

            isMatch = DoSegmentsMatch(segmentsToMatch, querySegments);

            return isMatch;
        }

        private static bool DoSegmentsMatch(string[] segmentsToMatch, string[] querySegments)
        {
            var isMatch = false;

            for (var i = 0; i < segmentsToMatch.Length; i++)
            {
                var match = segmentsToMatch[i];
                var segment = querySegments[i];

                isMatch = match.Equals(segment, StringComparison.InvariantCultureIgnoreCase) || match == WildCard;

                if (!isMatch)
                    return false;
            }

            return isMatch;
        }
    }
}