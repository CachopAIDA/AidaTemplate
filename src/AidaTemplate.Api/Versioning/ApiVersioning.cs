using System.Collections.Generic;
using System.Linq;

namespace AidaTemplate.Api.Versioning {
    public static class ApiVersioning {
        public const int Min = 1;
        public const int Current = 1;
        public static List<Microsoft.AspNetCore.Mvc.ApiVersion> Versions(int min = Min, int max = Current) {
            return Enumerable.Range(min, max - min + 1).Select(x => new Microsoft.AspNetCore.Mvc.ApiVersion(x, 0)).ToList();
        }
    }
}
