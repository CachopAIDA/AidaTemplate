using System;
using AidaTemplate.Api.Options;
using Core.Logging.AppInsights;
using Core.Logging.Boot.Extensions;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation.ApplicationId;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AidaTemplate.Api.Startups {
    public static class LoggerStartup {
        public static IServiceCollection ConfigureLogging(this IServiceCollection services,
            IConfiguration configuration, bool isDevelopment) {
            var loggingOptions = configuration.GetSection("Logging").Get<LoggingOptions>();
            services.AddLogging(logging => {
                if (isDevelopment) {
                    logging.AddConsole()
                        .AddFilter(level => level >= LogLevel.Trace);
                }
                if (loggingOptions.Serilog.Enable) {
                    logging.AddFile(Program.ProductContext.StartupSettingsFileName);
                }
                logging.AddFilter<Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider>("Category", loggingOptions.LogLevel.Default);
            });

            //App Insights
            var appInsightsOptions = loggingOptions.ApplicationInsights;

            //App Insights LiveMetrics
            var moduleQuickPulseServiceEndpoint = loggingOptions.ApplicationInsights.QuickPulseService.EndpointAddress;
            if (!string.IsNullOrWhiteSpace(moduleQuickPulseServiceEndpoint)) {
                services.ConfigureTelemetryModule<QuickPulseTelemetryModule>((module, o) => {
                    module.QuickPulseServiceEndpoint = moduleQuickPulseServiceEndpoint;
                    module.AuthenticationApiKey = appInsightsOptions.LiveMetricsKey;
                });
            }

            var profileQueryEndpoint = loggingOptions.ApplicationInsights.ProfileQuery.EndpointAddress;
            if (!string.IsNullOrWhiteSpace(profileQueryEndpoint)) {
                services.AddSingleton<IApplicationIdProvider, ApplicationInsightsApplicationIdProvider>(_ =>
                    new ApplicationInsightsApplicationIdProvider() {ProfileQueryEndpoint = profileQueryEndpoint});
            }

            var endpointAddress = loggingOptions.ApplicationInsights.TelemetryChannel.EndpointAddress;
            if (!string.IsNullOrWhiteSpace(endpointAddress)) {
                services.AddSingleton<ITelemetryChannel>(_ => new ServerTelemetryChannel()
                    {EndpointAddress = endpointAddress});
            }

            //App Insights Telemetry
            services.AddReducedApplicationInsightsTelemetry(options => {
                options.InstrumentationKey = appInsightsOptions.InstrumentationKey;
                options.EnablePerformanceCounterCollectionModule = appInsightsOptions.EnablePerformanceCounters;
                options.EnableAzureInstanceMetadataTelemetryModule = appInsightsOptions.EnableAzureInstanceMetadata;
                options.EnableAppServicesHeartbeatTelemetryModule = appInsightsOptions.EnableAppServicesHeartbeat;
            }, _ => new IgnoreOptionsBuilder()
                .Add("/swagger/")
                .Add("/status.json", TimeSpan.FromSeconds(10))
                .Add("/healthmetrics", TimeSpan.FromSeconds(10))
                .Add<DependencyTelemetry>()
                .Add<MetricTelemetry>()
                .Add<PageViewPerformanceTelemetry>());

            return services;
        }
    }
}