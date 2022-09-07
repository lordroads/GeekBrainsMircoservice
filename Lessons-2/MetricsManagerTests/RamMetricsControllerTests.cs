using MetricsControl.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsManagerTests;

public class RamMetricsControllerTests
{
    private RamMetricsController _networkMetricsController;
    public RamMetricsControllerTests()
    {
        _networkMetricsController = new RamMetricsController();
    }

    [Fact]
    public void GetMetricsFromAgent_ReturnOk()
    {
        int agentId = 1;
        TimeSpan fromTime = TimeSpan.FromSeconds(0);
        TimeSpan toTime = TimeSpan.FromSeconds(100);

        var result = _networkMetricsController.GetMetricsFromAgent(agentId, fromTime, toTime);

        Assert.IsAssignableFrom<IActionResult>(result);
    }

    [Fact]
    public void GetCountErrorsFromAll_ReturnOk()
    {
        TimeSpan fromTime = TimeSpan.FromSeconds(0);
        TimeSpan toTime = TimeSpan.FromSeconds(100);

        var result = _networkMetricsController.GetMetricsFromAll(fromTime, toTime);

        Assert.IsAssignableFrom<IActionResult>(result);
    }
}
