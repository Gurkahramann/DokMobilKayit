using DockMobile.ApiClient.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace DockMobile.ApiClient.IoC
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApiClientService(this IServiceCollection services, Action<ApiClientOptions> configureOptions)
        {
            services.Configure(configureOptions);

            services.AddSingleton<ApiClientService>();

            return services;
        }
    }
}
