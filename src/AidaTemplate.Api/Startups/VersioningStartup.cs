using AidaTemplate.Api.Controllers;
using AidaTemplate.Api.Versioning;
using Core.Http.Api.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AidaTemplate.Api.Startups {
    public static class VersioningStartup {
        public static IServiceCollection ConfigureVersioning(this IServiceCollection services) {
            services.AddVersionedApiExplorer(options => {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            var apiVersionsDefinition = new NetCore31ApiVersioningDefinition();
            services.AddSingleton<ApiVersioningDefinition>(_ => apiVersionsDefinition);
            services.AddApiVersioning(options => {
                options.UseApiBehavior = false;
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(ApiVersioning.Current, 0);
                SamplesController.Convention(options, apiVersionsDefinition);
                ApiVersionsController.Convention(options, apiVersionsDefinition);
            });
            return services;
        }
    }
}