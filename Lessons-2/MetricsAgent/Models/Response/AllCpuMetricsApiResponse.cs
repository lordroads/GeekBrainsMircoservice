using MetricsAgent.Models.Dto;

namespace MetricsAgent.Models.Response;

public class AllCpuMetricsApiResponse
{
    public IList<CpuMetricDto> Metrics { get; set; }
}
