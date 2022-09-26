using MetricsAgent.Models.Dto;

namespace MetricsAgent.Models.Response;

public class AllHddMetricsApiResponse
{
    public IList<HddMetricDto> Metrics { get; set; }
}
