using MetricsControl.Models.Dto;
using System.Text.Json.Serialization;

namespace MetricsControl.Models.Response;

public class AllCpuMetricsApiResponse
{
    public int AgentId { get; set; }

    [JsonPropertyName("metrics")]
    public IList<CpuMetricDto> Metrics { get; set; }
}
