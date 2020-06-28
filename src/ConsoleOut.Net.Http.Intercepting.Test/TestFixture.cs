using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;

namespace ConsoleOut.Net.Http.Intercepting.Test
{
    public class TestFixture : IDisposable
    {
        private const string BaseUrl = "http://localhost:5000/";

        private bool _disposedValue = false;

        public HttpClient Client { get; }

        public TestFixture()
        {
            Client = CreateClient();
        }

        private static HttpClient CreateClient()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", false);

            var configuration = builder.Build();
            IConfigurationSection section = configuration.GetSection("HttpInterceptorOptions");

            IServiceCollection services = new ServiceCollection();
            services.Configure<Collection<HttpInterceptorOptions>>(section);
            services.AddTransient<RequestsInterceptor>();

            services.AddHttpClient("product", c =>
            {
                c.BaseAddress = new Uri(BaseUrl);
                c.DefaultRequestHeaders.Add("User-Agent", "RequestInterceptorTests");
            })
            .AddHttpMessageHandler<RequestsInterceptor>();

            var clientFactory = services.BuildServiceProvider().GetRequiredService<IHttpClientFactory>();
            var client = clientFactory.CreateClient("product");

            return client;
        }

        #region IDisposable Support



        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Client?.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
