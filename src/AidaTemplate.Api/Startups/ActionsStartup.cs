using AidaTemplate.Api.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace AidaTemplate.Api.Startups {
    public static class ActionsStartup {
        public static IServiceCollection ConfigureActions(this IServiceCollection services) {
            services.AddScoped<GetSampleCommandHandler, GetSampleCommandHandler>();
            services.AddScoped<GetSampleQueryHandler, GetSampleQueryHandler>();
            return services;
        }
    }
}