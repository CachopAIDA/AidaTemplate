using System.Net;
using System.Threading.Tasks;
using Core.Http.Api.Test.Common;
using Core.Http.Api.Test.Common.Assert;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;

namespace AidaTemplate.Api.Tests {
    [TestFixture]
    public class ApiShould {
        
        [Test]
        public void get_the_status() {
            var apiClient = TestFixture.OAuth2HttpApiClient;

            var request = apiClient.AGet<string>("status.json");

            request.ShouldResponse().FailWithStatus(HttpStatusCode.ServiceUnavailable);
        }

        [Test]
        public async Task get_the_health_metrics() {
            var apiClient = TestFixture.httpClient;

            var request = await apiClient.GetAsync("healthmetrics");

            request.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Test]
        public async Task have_swagger_enabled() {
            var apiClient = TestFixture.httpClient;

            var request =  await apiClient.GetAsync("swagger/index.html");

            request.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}