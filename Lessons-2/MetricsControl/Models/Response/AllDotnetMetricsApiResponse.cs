using MetricsControl.Models.Dto;
using System.Text.Json.Serialization;

namespace MetricsControl.Models.Response;

public class AllDotnetMetricsApiResponse
{
    public int AgentId { get; set; }

    [JsonPropertyName("metrics")]
    public IList<DotnetMetricDto> Metrics { get; set; }
}
