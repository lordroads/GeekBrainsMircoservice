namespace MetricsControl.Models.Requests;

public class GetAllHddMetricsApiRequest
{
    public Uri AgentAddress { get; set; }
    public TimeSpan FromTime { get; set; }
    public TimeSpan ToTime { get; set; }
}
