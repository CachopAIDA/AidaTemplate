using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace AidaTemplate.Tests.Infrastructure {
    internal static class TestConfiguration {
        private static IConfiguration configuration;

        public static IConfiguration Get() {
            if (configuration != null) return configuration;
            configuration = new ConfigurationBuilder()
                .SetBasePath(TestContext.CurrentContext.TestDirectory)
                .AddJsonFile("appsettings.json", true, false)
                .Build();
            return configuration;
        }

        public static string GetConnectionString(string connectionStringName) {
            return Get().GetConnectionString(connectionStringName);
        }
    }
}