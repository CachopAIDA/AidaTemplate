using CentralConfiguration.Client.Extensions;

namespace AidaTemplate.Api {
    public class ProductContext {
        public string Name { get; set; } = "AidaTemplate API";
        public string Id { get; set; } = "aidatemplate-api";
        public string Environment { get; set; } = "Development";
        public string BootAppInsightsKey { get; set; }
        public string BootLoggingFile { get; set; } = "startup-.json";
        public CentralConfigurationOptions ConfigurationApiOptions { get; set; }
        public string StartupSettingsFileName { get; set; } = "startup-settings.log";
    }
}