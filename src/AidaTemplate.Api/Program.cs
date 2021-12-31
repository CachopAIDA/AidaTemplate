using CentralConfiguration.Client.Extensions;
using Core.Logging.Boot.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace AidaTemplate.Api {
    public class Program {
        public static readonly ProductContext ProductContext = GetProductContext();
        public static string ApplicationName => ProductContext.Name;
        public const string Development = "Development";

        public static void Main(string[] args) {
            Host.CreateDefaultBuilder(args)
                .UseEnvironment(ProductContext.Environment)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder
                        .ConfigureAppConfiguration(SetConfigurationProviders)
                        .UseStartup<Startup>();
                })
                .BuildWithBootLogs(options => {
                    options.ApplicationName = ProductContext.Id;
                    options.Environment = ProductContext.Environment;
                    options.EnableAppInsights = ProductContext.Environment != Development;
                    options.AppInsightsKey = ProductContext.BootAppInsightsKey;
                    options.EnableSerilog = ProductContext.Environment != Development;
                    options.SeriLogFile = ProductContext.BootLoggingFile;
                })
                .Run();
        }

        // This method is used by nswag to generate the api client package.
        public static IHostBuilder CreateHostBuilder(string[] args) {
            return Host.CreateDefaultBuilder(args)
                .UseEnvironment(Development)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder
                        .ConfigureAppConfiguration(x => {})
                        .UseStartup<Startup>();
                });
        }

        private static ProductContext GetProductContext() {
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("environment.json", true, false)
                .Build();
            return new ProductContext {
                Name = "AidaTemplate Api",
                Id = "aidatemplate-api",
                Environment = config["ASPNETCORE_ENVIRONMENT"],
                BootAppInsightsKey = config["SIMASUITE_BOOT_APPINSIGHTS_KEY"],  // Ensure your host has this key as ENV variable
                BootLoggingFile = "startup-.log",                               // You can view your startup logs in this file
                ConfigurationApiOptions = new CentralConfigurationOptions {
                    ApiUrl = config["CENTRALCONFIGURATION_URL"],                //Ensure your host has these keys as ENV variable
                    AuthConfig = new AuthConfig(
                        config["CENTRALCONFIGURATION_AUTHORIZATION_CLIENTSECRET"],
                        config["CENTRALCONFIGURATION_AUTHORIZATION_URL"])
                },
                StartupSettingsFileName = "startup-settings.log"                // You can view your startup settings in this file
            };
        }

        private static void SetConfigurationProviders(WebHostBuilderContext context,
            IConfigurationBuilder configurationBuilder) {
            configurationBuilder
                .SetBasePath(context.HostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddCentralConfigurationApiProvider(ProductContext.ConfigurationApiOptions, ProductContext.Id, ProductContext.Environment)
                .AddLocalSettingsInDevelopment(context, "localsettings.json");
        }
    }

    public static class ConfigurationBuilderExtensions {
        public static IConfigurationBuilder AddLocalSettingsInDevelopment(this IConfigurationBuilder builder, WebHostBuilderContext context, string localSettingsFile) {
            if (!context.HostingEnvironment.IsDevelopment()) return builder;
            builder.AddJsonFile(localSettingsFile, true, true);
            return builder;
        }

        public static IConfigurationBuilder AddCentralConfigurationApiProvider(this IConfigurationBuilder configurationBuilder, CentralConfigurationOptions centralConfigurationOptions, string productKey, string environment) {
            configurationBuilder.AddCentralConfiguration(productKey, environment,
                centralConfigurationOptions,
                optional: true, reloadOnChange: true);
            return configurationBuilder;
        }
    }
}