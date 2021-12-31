using System;
using System.Collections.Generic;
using AidaTemplate.Api.Versioning;
using Core.Http.Api.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSwag;
using OpenApiOAuthFlow = NSwag.OpenApiOAuthFlow;
using OpenApiOAuthFlows = NSwag.OpenApiOAuthFlows;
using OpenApiSecurityScheme = NSwag.OpenApiSecurityScheme;

namespace AidaTemplate.Api.Startups {
    public static class SwaggerStartUp {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services, string productName) {
            for (var v = ApiVersioning.Min; v <= ApiVersioning.Current; v++) {
                AddSwaggerDocFor(services, productName, v);
            }

            return services;
        }

        private static void AddSwaggerDocFor(IServiceCollection services, string productName, int v) {
            var documentName = $"v{v}";
            services.AddSwaggerDocument((document, serviceProvider) => {
                document.DocumentName = documentName;
                document.Title = productName.ToUpper();
                document.Version = documentName;
                document.ApiGroupNames = new[] { $"v{v}" };
                var idOptions = serviceProvider.GetService<IOptions<IdentityServerConfig>>().Value;
                document.AddSecurity("oauth2",
                    new List<string> {idOptions.ApiName, $"{productName} - full access"},
                    new OpenApiSecurityScheme {
                        Description = "OAuth2 - Client Credentials",
                        Type = OpenApiSecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows {
                            ClientCredentials = new OpenApiOAuthFlow {
                                TokenUrl = new Uri(new Uri(idOptions.Authority), "connect/token").ToString(),
                                Scopes = new Dictionary<string, string> {
                                    {idOptions.ApiName, $"{productName} - full access"}
                                }
                            }
                        }
                    });
            });
        }
    }
}