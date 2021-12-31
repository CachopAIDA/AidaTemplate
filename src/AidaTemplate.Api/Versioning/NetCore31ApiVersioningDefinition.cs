using System.Collections.Generic;
using Core.Http.Api.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace AidaTemplate.Api.Versioning {
    public class NetCore31ApiVersioningDefinition  : ApiVersioningDefinition {
        public List<ApiVersion> Versions { get; }

        public NetCore31ApiVersioningDefinition(int min = ApiVersioning.Min, int max = ApiVersioning.Current) {
            Versions = ApiVersioning.Versions(min, max);
        }
    }
}