using MetricsControl.Models;
using System;

namespace MetricsManagerTests;

public class LazySingltone
{
    private static readonly Lazy<AgentPool> _instance = new Lazy<AgentPool>(() => new AgentPool());

    public static AgentPool Instance
    {
        get { return _instance.Value; }
    }
    public LazySingltone() { }
}
