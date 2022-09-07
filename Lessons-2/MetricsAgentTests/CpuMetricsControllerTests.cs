using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsAgentTests;

public class CpuMetricsControllerTests
{
    private CpuMetricsController _cpuMetricsController;
    
    public CpuMetricsControllerTests()
    {
        _cpuMetricsController = new CpuMetricsController();
    }

    [Fact]
    public void GetCpuMetrics_ResultOk()
    {
        TimeSpan fromTime = TimeSpan.FromSeconds(0);
        TimeSpan toTime = TimeSpan.FromSeconds(100);

        var result = _cpuMetricsController.GetCpuMetrics(fromTime, toTime);

        Assert.IsAssignableFrom<IActionResult>(result);
    }
}
