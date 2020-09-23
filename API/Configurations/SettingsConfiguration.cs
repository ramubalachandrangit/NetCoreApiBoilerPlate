using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Configurations
{
    public static class SettingsConfiguration
    {
        public static IServiceCollection AddAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = new AppSettings();
            configuration.Bind(nameof(appSettings), appSettings);
            services.AddSingleton(appSettings);
            return services;
        }
    }

    public class AppSettings
    {
        public StartUpSettings StartUpSettings { get; set; }
    }

    public class StartUpSettings
    {
        public string SwaggerUrl { get; set; }
    }
}
