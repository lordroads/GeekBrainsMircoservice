namespace MetricsControl.Models.Requests;

public class GetAllRamMetricsApiRequest
{
    public Uri AgentAddress { get; set; }
    public TimeSpan FromTime { get; set; }
    public TimeSpan ToTime { get; set; }
}