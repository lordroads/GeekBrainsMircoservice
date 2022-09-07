using MetricsControl.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsManagerTests;

public class CpuMetricsControllerTest
{
    private CpuMetricsController _cpuMetricsController;
    public CpuMetricsControllerTest()
    {
        _cpuMetricsController = new CpuMetricsController();
    }

    [Fact]
    public void GetMetricsAll_ReturnOk()
    {
        TimeSpan fromTime = TimeSpan.FromSeconds(0);
        TimeSpan toTime = TimeSpan.FromSeconds(100);

        var result = _cpuMetricsController.GetMetricsFromAll(fromTime, toTime);

        Assert.IsAssignableFrom<IActionResult>(result);
    }

    [Fact]
    public void GetMetricsFromAgent_ReturnOk()
    {
        int agentId = 1;
        TimeSpan fromTime = TimeSpan.FromSeconds(0);
        TimeSpan toTime = TimeSpan.FromSeconds(100);

        var result = _cpuMetricsController.GetMetricsFromAgent(agentId, fromTime, toTime);

        Assert.IsAssignableFrom<IActionResult>(result);
    }

    [Fact]
    public void GetMetricsFromAllByPercentilet_ReturnOk()
    {
        int percentile = 50;
        TimeSpan fromTime = TimeSpan.FromSeconds(0);
        TimeSpan toTime = TimeSpan.FromSeconds(100);

        var result = _cpuMetricsController.GetMetricsFromAllByPercentile(fromTime, toTime, percentile);

        Assert.IsAssignableFrom<IActionResult>(result);
    }
}
