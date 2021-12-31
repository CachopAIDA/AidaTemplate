using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using AidaTemplate.Api.Metrics;
using AidaTemplate.Api.Model;
using AidaTemplate.Api.UseCases;
using Core.Http.Api.Versioning;
using Core.Http.Filtering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AidaTemplate.Api.Controllers {
    [Route("api/v{version:apiVersion}/samples")]
    [ApiController]
    public class SamplesController : VersionedController<SamplesController> {
        private readonly GetSampleCommandHandler getSampleCommandHandler;
        private readonly GetSampleQueryHandler getSampleQueryHandler;
        private readonly ILogger<SamplesController> logger;

        public SamplesController(GetSampleCommandHandler getSampleCommandHandler, GetSampleQueryHandler getSampleQueryHandler, ILogger<SamplesController> logger) {
            this.getSampleCommandHandler = getSampleCommandHandler;
            this.getSampleQueryHandler = getSampleQueryHandler;
            this.logger = logger;
        }

        [HttpPost]
        public ActionResult<string> Post() {
            try {
                var metric = MetricsStore.GetSampleMetric();
                metric.Labels("helloLabel");
                var message = getSampleCommandHandler.Handle();
                return Ok(message);
            }
            catch (Exception ex) {
                var metric = MetricsStore.GetSampleFaultedMetric();
                metric.Labels("helloLabel")
                    .Inc();
                logger.LogError(ex.Message, ex.StackTrace);
                throw;
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<Sample>>> Get([FromQuery]Query<SampleFilter> query) {
            try {
                logger.LogInformation("get");
                var metric = MetricsStore.GetSampleMetric();
                metric.Labels("helloLabel");
                var message = await getSampleQueryHandler.Execute(query);
                return Ok(message);
            }
            catch (Exception ex) {
                var metric = MetricsStore.GetSampleFaultedMetric();
                metric.Labels("helloLabel")
                    .Inc();
                logger.LogError(ex.Message, ex.StackTrace);
                throw;
            }
        }
    }
}