using System;

using MapsCustomConnector.Configs;
using MapsCustomConnector.Services;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(MapsCustomConnector.Startup))]

namespace MapsCustomConnector
{
    /// <summary>
    /// This represents the entity used for startup bootstrapping.
    /// </summary>
    public class Startup : FunctionsStartup
    {
        /// <inheritdoc/>
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            builder.ConfigurationBuilder
                   .AddEnvironmentVariables();

            base.ConfigureAppConfiguration(builder);
        }

        /// <inheritdoc/>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            ConfigureAppSettings(builder.Services);
            ConfigureClients(builder.Services);
            ConfigureMapServices(builder.Services);
        }

        private static void ConfigureAppSettings(IServiceCollection services)
        {
            var settings = services.BuildServiceProvider()
                                   .GetService<IConfiguration>()
                                   .Get<MapsSettings>(MapsSettings.Name);
            services.AddSingleton(settings);

            var codespaces = bool.TryParse(Environment.GetEnvironmentVariable("OpenApi__RunOnCodespaces"), out var isCodespaces) && isCodespaces;
            if (codespaces)
            {
                /* ⬇️⬇️⬇️ Add this ⬇️⬇️⬇️ */
                services.AddSingleton<IOpenApiConfigurationOptions>(_ =>
                        {
                            var options = new DefaultOpenApiConfigurationOptions()
                            {
                                IncludeRequestingHostName = false
                            };

                            return options;
                        });
                /* ⬆️⬆️⬆️ Add this ⬆️⬆️⬆️ */
            }
        }

        private static void ConfigureClients(IServiceCollection services)
        {
            services.AddHttpClient("google");
            services.AddHttpClient("naver");
        }

        private static void ConfigureMapServices(IServiceCollection services)
        {
            services.AddSingleton<IMapServiceFactory, MapServiceFactory>();
        }
    }
}