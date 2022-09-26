using MetricsAgent.Models.Dto;

namespace MetricsAgent.Models.Response;

public class AllRamMetricsApiResponse
{
    public IList<RamMetricDto> Metrics { get; set; }
}
