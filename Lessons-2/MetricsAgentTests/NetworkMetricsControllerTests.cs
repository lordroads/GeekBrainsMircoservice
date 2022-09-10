using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsAgentTests;

public class NetworkMetricsControllerTests
{
    private NetworkMetricsController _networkMetricsController;
    
    public NetworkMetricsControllerTests()
    {
        _networkMetricsController = new NetworkMetricsController();
    }

    [Fact]
    public void GetNetworkMetrics()
    {
        TimeSpan fromTime = TimeSpan.FromSeconds(0);
        TimeSpan toTime = TimeSpan.FromSeconds(100);

        var result = _networkMetricsController.GetNetworkMetrics(fromTime, toTime);

        Assert.IsAssignableFrom<IActionResult>(result);
    }
}
