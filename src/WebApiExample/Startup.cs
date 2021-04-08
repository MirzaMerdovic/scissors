﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scissors.HttpRequestInterceptor;
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
            services.Configure<Collection<HttpInterceptorOptions>>(Configuration.GetSection("HttpInterceptorOptions"));

            services.AddTransient<RequestsInterceptor>();

            services.AddHttpClient<ProductAdapter>(
                x =>
                {
                    x.BaseAddress = new Uri("http://localhost:5000");
                    x.DefaultRequestHeaders.Add("User-Agent", "RequestInterceptorTests");
                }).AddHttpMessageHandler<RequestsInterceptor>();

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
