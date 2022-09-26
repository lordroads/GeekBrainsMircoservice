using MetricsAgent.Models.Dto;

namespace MetricsAgent.Models.Response;

public class AllDotnetMetricsApiResponse
{
    public IList<DotnetMetricDto> Metrics { get; set; }
}
