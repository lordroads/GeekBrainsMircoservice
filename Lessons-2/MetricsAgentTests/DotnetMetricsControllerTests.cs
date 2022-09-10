using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsAgentTests;

public class DotnetMetricsControllerTests
{
    private readonly DotnetMetricsController _dotnetMetricsController;

    public DotnetMetricsControllerTests()
    {
        _dotnetMetricsController = new DotnetMetricsController();
    }

    [Fact]
    public void GetDotnetMetrics()
    {
        TimeSpan fromTime = TimeSpan.FromSeconds(0);
        TimeSpan toTime = TimeSpan.FromSeconds(100);

        var result = _dotnetMetricsController.GetDotnetMetrics(fromTime, toTime);

        Assert.IsAssignableFrom<IActionResult>(result);
    }
}
