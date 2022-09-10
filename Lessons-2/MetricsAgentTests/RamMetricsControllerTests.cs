using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsAgentTests;

public class RamMetricsControllerTests
{
    private RamMetricsController _ramMetricsController;

    public RamMetricsControllerTests()
    {
        _ramMetricsController = new RamMetricsController();
    }

    [Fact]
    public void GetRamMetrics()
    {
        TimeSpan fromTime = TimeSpan.FromSeconds(0);
        TimeSpan toTime = TimeSpan.FromSeconds(100);

        var result = _ramMetricsController.GetRamMetrics(fromTime, toTime);

        Assert.IsAssignableFrom<ActionResult<int>>(result);
    }
}
