using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using Core.Http;
using Core.Http.Api.Authentication;
using Core.Http.Api.Test.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AidaTemplate.Api.Tests {

    [SetUpFixture]
    public class TestFixture {
        public static IConfigurationRoot Configuration;
        public static OAuth2HttpApiClient OAuth2HttpApiClient;
        public static HttpClient httpClient;
        public static HttpMessageHandler messageHandler;
        public static readonly List<Claim> Claims = new List<Claim> {
            new Claim("applicationOwnerId", "ApplicationSourceId")
        };
        private static IdentityServerOptionsForTest identityServerOptions;

        [OneTimeSetUp]
        public void SetUp() {
            Configuration = PrepareConfiguration(new ConfigurationBuilder()).Build();
            identityServerOptions = Configuration.GetSection("IdentityServerConfig").Get<IdentityServerOptionsForTest>();
            var identityServer = InitializeIdentityServer();
            var server = InitializeApiServer(identityServer, Configuration["ApiUrl"]);
            InitializeHttpClients(server, identityServer, Configuration["ApiUrl"]);
        }
        
        private static IConfigurationBuilder PrepareConfiguration(IConfigurationBuilder configurationBuilder) {
            return configurationBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("testsettings.json", optional: false);
        }

        private static IdentityServerForTests InitializeIdentityServer() {
            return IdentityServerForTests
                .Run(options => {
                    options.ServerUrl = identityServerOptions.Authority;
                    options.ApiName = identityServerOptions.ApiName;
                    options.AuthClients = new List<AuthClient> {
                        new AuthClient(identityServerOptions.ClientId, identityServerOptions.ClientSecret, Claims),
                    };
                });
        }

        private TestServer InitializeApiServer(IdentityServerForTests identityServer, string baseAddress) {
            var webHostBuilder = new WebHostBuilder()
                .UseEnvironment("Test")
                .UseUrls(baseAddress)
                .ConfigureAppConfiguration((builderContext, configBuilder) => { PrepareConfiguration(configBuilder); })
                .ConfigureTestServices(services => {
                    services.Configure<IdentityServerConfig>(config => {
                        config.Authority = identityServer.ServerUrl;
                        config.ApiName = identityServer.ApiName;
                        config.RequireHttpsMetadata = true;
                    });
                    //TODO: services.AddSingleton(YourFactoryMock);
                })
                .UseStartup<Startup>();

            return new TestServer(webHostBuilder);
        }

        private void InitializeHttpClients(TestServer testServer, IdentityServerForTests identityServer, string baseAddress) {
            var authorizedClient = identityServer.AuthClients.First(x => x.ClientId == identityServerOptions.ClientId);
            httpClient = CreateClient(testServer, baseAddress, authorizedClient, identityServer);
            OAuth2HttpApiClient = new OAuth2HttpApiClient(
                identityServer.ServerUrl,
                authorizedClient.ClientId,
                authorizedClient.ClientSecret,
                identityServer.ApiName,
                httpClient);
        }

        private HttpClient CreateClient(TestServer server, string baseAddress, AuthClient authorizedClient,
            IdentityServerForTests identityServer) {
            messageHandler = server.CreateHandler();
            var client = new HttpClient(
                new AuthorizationTokenHandler(
                    new OAuth2AuthorizationClient(
                        new IdentityModel.Client.ClientCredentialsTokenRequest() {
                            Address = identityServer.ServerUrl,
                            ClientId = authorizedClient.ClientId,
                            ClientSecret = authorizedClient.ClientSecret,
                            Scope = identityServer.ApiName
                        }, new TokenStore()
                    ), messageHandler)) {
                BaseAddress = new Uri(baseAddress)
            };
            return client;
        }
    }
}
