using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Aida.Core.Metrics.HealthChecks.Prometheus.Xabaril;
using AidaTemplate.Api.Exceptions;
using AidaTemplate.Api.Startups;
using Core.Http.Api.Middleware.Logging;
using Core.Http.Filtering.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NSwag;
using NSwag.AspNetCore;
using Prometheus;
using Sherlock.UI;

#pragma warning disable 1591

namespace AidaTemplate.Api {
    public class Startup {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration) {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services
                .ConfigureLogging(configuration, Program.ProductContext.Environment == Program.Development)
                .ConfigureOptions(configuration, Program.ProductContext.StartupSettingsFileName);

            services.AddMvcCore()
                .AddMvcOptions(options =>
                    options.Filters.Add(new ProducesAttribute("application/json"))
                )
                .AddJsonOptions(options => {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .AddNewtonsoftJson(options => {
                    options.SerializerSettings.Converters.Add(new Core.Http.Api.Serialization.StringNullableEnumJsonConverter());
                    options.SerializerSettings.Converters.Add(new Core.Http.Api.Serialization.Iso8601DateTimeJsonConverter());
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                });

            services.AddCors()
                .AddAuthorization()
                .ConfigureAuthentication(configuration)
                .ConfigureVersioning()
                .ConfigureSwagger(Program.ProductContext.Name)
                .ConfigureHealthChecks(configuration)
                .ConfigureActions()
                .ConfigureSherlock(configuration)
                .AddHttpFilteringModelBinder();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IApiVersionDescriptionProvider provider) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseMiddleware<ExceptionHandlingMiddleware>()
                .UseRequestInsightsTracker()
                .UseOpenApi(options => {
                    options.PostProcess = (document, request) =>
                    {
                        var serverUrl = configuration["SwaggerBaseUrl"];
                        document.Servers.Clear();
                        document.Servers.Add(new OpenApiServer { Url = serverUrl });
                    };
                })
                .UseSwaggerUi3(settings => {
                    settings.TransformToExternalPath = (route, request) => {
                        var baseUri = new Uri(configuration["SwaggerBaseUrl"]);
                        var relativeUri = route.TrimStart('/');
                        return new Uri(baseUri, relativeUri).ToString();
                    };
                
                    foreach (var description in provider.ApiVersionDescriptions.Reverse()) {
                        settings.SwaggerRoutes.Add(new SwaggerUi3Route(description.GroupName.ToUpperInvariant(),
                            $"/swagger/{description.GroupName}/swagger.json"));
                    }
                
                    settings.OAuth2Client = new OAuth2ClientSettings {
                        ClientId = "Developer",
                        ClientSecret = "Developer",
                        AppName = Program.ApplicationName
                    };
                })
                .UseHealthChecks("/status.json",
                    new HealthCheckOptions
                        {Predicate = _ => true, ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse})
                .UsePrometheusHealthCheckMetrics()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints => {
                    endpoints
                        .MapControllers()
                        .RequireAuthorization();
                    endpoints.MapMetrics();
                })
                .UseSherlock(env);
        }

    }
}