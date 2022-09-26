namespace MetricsAgent.Models.Requests;

public class DotnetMetricCreateRequest
{
    public int Value { get; set; }
    public TimeSpan Time { get; set; }
}