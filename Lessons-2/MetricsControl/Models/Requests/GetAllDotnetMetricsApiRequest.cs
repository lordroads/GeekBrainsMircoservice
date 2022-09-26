namespace MetricsControl.Models.Requests;

public class GetAllDotnetMetricsApiRequest
{
    public Uri AgentAddress { get; set; }
    public TimeSpan FromTime { get; set; }
    public TimeSpan ToTime { get; set; }
}
