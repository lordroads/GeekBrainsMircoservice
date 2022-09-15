using System.Text.Json.Serialization;

namespace MetricsControl.Models.Dto;

public class CpuMetricDto
{
    [JsonPropertyName("value")]
    public int Value { get; set; }

    [JsonPropertyName("time")]
    public long Time { get; set; }
}
