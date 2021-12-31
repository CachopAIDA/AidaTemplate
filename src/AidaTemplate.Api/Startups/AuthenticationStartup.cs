using Core.Http.Api.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AidaTemplate.Api.Startups {
    public static class AuthenticationStartup {
        public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration) {
            var iConfigurationSection = configuration.GetSection("IdentityServerConfig");
            services.Configure<IdentityServerConfig>(iConfigurationSection);
            services
                .AddAuthentication("Bearer")
                .AddJwtBearer(options => {
                    var isOptions = iConfigurationSection.Get<IdentityServerConfig>();
                    options.Authority = isOptions.Authority;
                    options.Audience = isOptions.ApiName;
                    options.RequireHttpsMetadata = isOptions.RequireHttpsMetadata;
                });
            return services;
        }
    }
}