using MetricsControl.Models.Dto;
using System.Text.Json.Serialization;

namespace MetricsControl.Models.Response;

public class AllNetworkMetricsApiResponse
{
    public int AgentId { get; set; }

    [JsonPropertyName("metrics")]
    public IList<NetworkMetricDto> Metrics { get; set; }
}
