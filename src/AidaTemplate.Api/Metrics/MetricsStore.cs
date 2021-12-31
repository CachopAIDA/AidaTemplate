using Core.Messaging.Metrics;
using Prometheus;

namespace AidaTemplate.Api.Metrics {
    public class MetricsStore {

        public static Gauge GetSampleMetric() {
            const string metricName = "get_hello";
            var metricLabels = new[] {"command"};
            return MetricsHelper.GetMetric(metricName, "", metricLabels);
        }

        public static Gauge GetSampleFaultedMetric() {
            const string metricName = "get_hello_faulted";
            var metricLabels = new[] {"command"};
            return MetricsHelper.GetMetric(metricName, "", metricLabels);
        }

    }
}