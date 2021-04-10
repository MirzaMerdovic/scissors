using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scissors;
using System;
using System.Collections.ObjectModel;

namespace WebApiExample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Collection<HttpRequestInterceptorOptions>>(Configuration.GetSection("HttpInterceptorOptions"));

            services.AddTransient<HttpRequestInterceptor>();

            services.AddHttpClient<ProductAdapter>(
                x =>
                {
                    x.BaseAddress = new Uri("http://localhost:5000");
                    x.DefaultRequestHeaders.Add("User-Agent", "RequestInterceptorTests");
                }).AddHttpMessageHandler<HttpRequestInterceptor>();

            services.AddTransient<IProductAdapter, ProductAdapter>();

            services.AddMvc(x => x.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
