using Microsoft.Extensions.Logging;

namespace AidaTemplate.Api.Options {
    public class LoggingOptions {
        public LoggingLogLevelOptions LogLevel { get; set; }
        public string AppName { get; set; }
        public ApplicationInsightsOptions ApplicationInsights { get; set; }
        public SerilogOptions Serilog { get; set; }
    }


    public class ApplicationInsightsOptions {
        public string InstrumentationKey { get; set; }
        public string LiveMetricsKey { get; set; }
        public bool EnablePerformanceCounters { get; set; }
        public bool EnableAzureInstanceMetadata { get; set; }
        public bool EnableAppServicesHeartbeat { get; set; }
        public ApplicationInsightsChannelOptions TelemetryChannel { get; set; }
        public ApplicationInsightsChannelOptions QuickPulseService { get; set; }
        public ApplicationInsightsChannelOptions ProfileQuery { get; set; }
    }

    public class SerilogOptions {
        public bool Enable { get; set; }
    }

    public class ApplicationInsightsChannelOptions {
        public string EndpointAddress { get; set; }
    }

    public class LoggingLogLevelOptions {
        public LogLevel Default { get; set; }
    }
}