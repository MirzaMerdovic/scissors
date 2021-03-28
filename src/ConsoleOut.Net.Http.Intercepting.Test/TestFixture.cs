using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;

namespace ConsoleOut.Net.Http.Intercepting.Test
{
    public abstract class TestFixture : IDisposable
    {
        private const string BaseUrl = "https://local.host/";

        private bool _disposedValue = false;

        private ServiceProvider _provider;

        private IHttpClientFactory _factory;

        protected TestFixture(List<KeyValuePair<string, string>> configurationPairs)
        {
            _provider = BuildServiceProvider(configurationPairs);

            _factory = _provider.GetRequiredService<IHttpClientFactory>();
        }

        public HttpClient GetClient() => _factory.CreateClient("test");

        private static ServiceProvider BuildServiceProvider(IEnumerable<KeyValuePair<string, string>> configurationPairs)
        {
            var builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(configurationPairs);
            var configuration = builder.Build();

            IServiceCollection services = new ServiceCollection();

            services.Configure<Collection<HttpInterceptorOptions>>(configuration.GetSection("HttpInterceptorOptions"));

            services.AddTransient<RequestsInterceptor>();

            services.AddHttpClient("test", c =>
            {
                c.BaseAddress = new Uri(BaseUrl);
                c.Timeout = TimeSpan.FromSeconds(1);
                c.DefaultRequestHeaders.Add("User-Agent", "RequestInterceptorTests");
            })
            .AddHttpMessageHandler<RequestsInterceptor>();

            return services.BuildServiceProvider();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _provider?.Dispose();

                    _factory = null;
                    _provider = null;
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
