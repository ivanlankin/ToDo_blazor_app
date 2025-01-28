using BlazorWebApp.Models;
using BlazorWebApp.RabbitMq;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorWebApp.IoC
{
    public static class ServiceCollectionExtension
    {
        //public static void AddDemoApiClientService(this IServiceCollection services, Action<ApiClientOptions> options)
        //{
        //    services.Configure(options);
        //    services.AddSingleton(provider =>
        //    {
        //        var options = provider.GetRequiredService<IOptions<ApiClientOptions>>().Value;
        //        return new DemoApiClientService(options);
        //    });
        //}
        public static void AddDemoRabbitMqClientService(this IServiceCollection services)
        {
            services.AddSingleton<IRabbitMqService, RabbitMqService>();
            services.AddSingleton<DemoApiClientService>(); 
        }
    }
}
