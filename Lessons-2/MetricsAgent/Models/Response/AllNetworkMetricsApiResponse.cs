using MetricsAgent.Models.Dto;

namespace MetricsAgent.Models.Response;

public class AllNetworkMetricsApiResponse
{
    public IList<NetworkMetricDto> Metrics { get; set; }
}
