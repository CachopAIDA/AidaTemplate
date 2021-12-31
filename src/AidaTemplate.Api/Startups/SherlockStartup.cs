using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sherlock.Telemetry.AppInsights;
using Sherlock.UI;

namespace AidaTemplate.Api.Startups {
    public static class SherlockStartup {
        public static IServiceCollection ConfigureSherlock(this IServiceCollection services,
            IConfiguration configuration) {
            services.AddSherlock(configuration);
            services.AddSignalR();
            services.AddApplicationInsightsTelemetry();
            services.AddApplicationInsightsTelemetryProcessor<SherlockTelemetryFilter>();
            return services;
        }
    }
}