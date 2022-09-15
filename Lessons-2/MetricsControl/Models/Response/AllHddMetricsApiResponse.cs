using MetricsControl.Models.Dto;
using System.Text.Json.Serialization;

namespace MetricsControl.Models.Response;

public class AllHddMetricsApiResponse
{
    public int AgentId { get; set; }

    [JsonPropertyName("metrics")]
    public IList<HddMetricDto> Metrics { get; set; }
}
