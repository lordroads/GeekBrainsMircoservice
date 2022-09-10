using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsAgentTests;

public class HddMetricsControllerTests
{
    private HddMetricsController _hddMetricsController;

    public HddMetricsControllerTests()
    {
        _hddMetricsController = new HddMetricsController();
    }

    [Fact]
    public void GetHddMetrics()
    {
        TimeSpan fromTime = TimeSpan.FromSeconds(0);
        TimeSpan toTime = TimeSpan.FromSeconds(100);

        var result = _hddMetricsController.GetHddMetrics(fromTime, toTime);

        Assert.IsAssignableFrom<ActionResult<int>>(result);
    }
}
