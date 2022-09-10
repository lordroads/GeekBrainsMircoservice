using MetricsControl.Controllers;
using Xunit;
using System;
using Microsoft.AspNetCore.Mvc;

namespace MetricsManagerTests;

public class DotnetMetricsControllerTests
{
    private DotnetMetricsController _dotnetMetricsController;
    public DotnetMetricsControllerTests()
    {
        _dotnetMetricsController = new DotnetMetricsController();
    }

    [Fact]
    public void GetMetricsFromAgent_ReturnOk()
    {
        int agentId = 1;
        TimeSpan fromTime = TimeSpan.FromSeconds(0);
        TimeSpan toTime = TimeSpan.FromSeconds(100);

        var result = _dotnetMetricsController.GetMetricsFromAgent(agentId, fromTime, toTime);

        Assert.IsAssignableFrom<IActionResult>(result);
    }

    [Fact]
    public void GetCountErrorsFromAll_ReturnOk()
    {
        TimeSpan fromTime = TimeSpan.FromSeconds(0);
        TimeSpan toTime = TimeSpan.FromSeconds(100);

        var result = _dotnetMetricsController.GetCountErrorsFromAll(fromTime, toTime);

        Assert.IsAssignableFrom<IActionResult>(result);
    }
}
