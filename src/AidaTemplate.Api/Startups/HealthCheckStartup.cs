using System;
using AidaTemplate.Api.Options;
using Core.Http.Api.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AidaTemplate.Api.Startups {
    public static class HealthCheckStartup {
        public static IServiceCollection ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration) {
            var builder = services.AddHealthChecks();
            
            builder
                .AddSqlServer(
                    connectionString: configuration.GetConnectionString("SqlServer"),
                    name: "central-database");

            //Identity Server
            var identityServerOptions = configuration.GetSection("IdentityServerConfig").Get<IdentityServerConfig>();
            builder.AddIdentityServer(new Uri(identityServerOptions.Authority), "identity-server");
            
            //Any External Service
            builder.AddUrlGroup(serviceProvider => {
                var externalApiOptions = serviceProvider.GetService<IOptionsSnapshot<CustomersApiOptions>>().Value;
                var statusUri = new Uri(new Uri(externalApiOptions.BaseUrl), "status.json");
                return statusUri;
            }, "customers-api");
            
            return services;
        }
    }
}