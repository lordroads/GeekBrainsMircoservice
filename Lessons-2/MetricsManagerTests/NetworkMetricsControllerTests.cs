using MetricsControl.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsManagerTests;

public class NetworkMetricsControllerTests
{
    private NetworkMetricsController _networkMetricsController;
    public NetworkMetricsControllerTests()
    {
        _networkMetricsController = new NetworkMetricsController();
    }

    [Fact]
    public void GetMetricsFromAgent_ReturnOk()
    {
        int agentId = 1;
        TimeSpan fromTime = TimeSpan.FromSeconds(0);
        TimeSpan toTime = TimeSpan.FromSeconds(100);

        var result = _networkMetricsController.GetNetworkMetricsFromAgent(agentId, fromTime, toTime);

        Assert.IsAssignableFrom<IActionResult>(result);
    }

    [Fact]
    public void GetCountErrorsFromAll_ReturnOk()
    {
        TimeSpan fromTime = TimeSpan.FromSeconds(0);
        TimeSpan toTime = TimeSpan.FromSeconds(100);

        var result = _networkMetricsController.GetNetworkMetricsFromAll(fromTime, toTime);

        Assert.IsAssignableFrom<IActionResult>(result);
    }
}
