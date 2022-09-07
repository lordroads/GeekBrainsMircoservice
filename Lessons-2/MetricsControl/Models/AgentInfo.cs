using System;

namespace MetricsControl.Models;

public class AgentInfo
{
    public int AgentId { get; set; }
    public Uri AgentAddress { get; set; }
    public bool Enable { get; set; }
}
