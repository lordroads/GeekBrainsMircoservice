using MetricsControl.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsManagerTests;

public class HddMetricsControllerTests
{
    private HddMetricsController _hddMetricsController;
    public HddMetricsControllerTests()
    {
        _hddMetricsController = new HddMetricsController();
    }

    [Fact]
    public void GetMetricsFromAgent_ReturnOk()
    {
        int agentId = 1;
        TimeSpan fromTime = TimeSpan.FromSeconds(0);
        TimeSpan toTime = TimeSpan.FromSeconds(100);

        var result = _hddMetricsController.GetMetricsFromAgent(agentId, fromTime, toTime);

        Assert.IsAssignableFrom<IActionResult>(result);
    }

    [Fact]
    public void GetCountErrorsFromAll_ReturnOk()
    {
        TimeSpan fromTime = TimeSpan.FromSeconds(0);
        TimeSpan toTime = TimeSpan.FromSeconds(100);

        var result = _hddMetricsController.GetMetricsFromAll(fromTime, toTime);

        Assert.IsAssignableFrom<IActionResult>(result);
    }
}
