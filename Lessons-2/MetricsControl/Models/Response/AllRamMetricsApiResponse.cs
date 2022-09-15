using MetricsControl.Models.Dto;
using System.Text.Json.Serialization;

namespace MetricsControl.Models.Response;

public class AllRamMetricsApiResponse
{
    public int AgentId { get; set; }

    [JsonPropertyName("metrics")]
    public IList<RamMetricDto> Metrics { get; set; }
}