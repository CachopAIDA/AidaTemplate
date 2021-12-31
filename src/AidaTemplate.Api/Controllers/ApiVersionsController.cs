using System.Collections.Generic;
using System.Linq;
using Core.Http.Api.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace AidaTemplate.Api.Controllers {
    [Route("api/v{version:apiVersion}/_versions")]
    [ApiController]
    public class ApiVersionsController : VersionedController<ApiVersionsController> {
        private readonly ApiVersioningDefinition apiVersionDefinition;

        public ApiVersionsController(ApiVersioningDefinition apiVersionDefinition) {
            this.apiVersionDefinition = apiVersionDefinition;
        }


        [HttpGet]
        public ActionResult<List<int>> Get() {
            return Ok(apiVersionDefinition.Versions.Select(v => v.MajorVersion));
        }

    }
}