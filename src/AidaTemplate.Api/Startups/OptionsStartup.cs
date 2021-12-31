using System.IO;
using AidaTemplate.Api.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AidaTemplate.Api.Startups {
    public static class OptionsStartup {
        public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration, string startupSettingsLogFile) {
            SaveStartupConfigurationToLocal(configuration, startupSettingsLogFile);

            services.Configure<CustomersApiOptions>(configuration.GetSection("CustomersApiClient"));
            return services;
        }

        private static void SaveStartupConfigurationToLocal(IConfiguration configuration, string fileName) {
            try {
                var root = (IConfigurationRoot) configuration;
                File.WriteAllText(fileName, root.GetDebugView());
            }
            catch { /*Assure not to fail in logging tasks */}
        }
    }
}