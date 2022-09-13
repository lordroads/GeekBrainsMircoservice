using MetricsAgent.Controllers;
using MetricsAgent.Models;
using MetricsAgent.Models.Requests;
using MetricsAgent.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace MetricsAgentTests;

public class CpuMetricsControllerTests
{
    private CpuMetricsController _cpuMetricsController;
    private Mock<ICpuMetricsRepository> _mockCpuMetricsRepository;
    private Mock<ILogger<CpuMetricsController>> _mockLogger;

    public CpuMetricsControllerTests()
    {
        _mockCpuMetricsRepository = new Mock<ICpuMetricsRepository>();
        _mockLogger = new Mock<ILogger<CpuMetricsController>>();

        _cpuMetricsController = new CpuMetricsController(_mockLogger.Object, _mockCpuMetricsRepository.Object);
    }

    [Fact]
    public void GetCpuMetrics_ResultOk()
    {
        TimeSpan fromTime = TimeSpan.FromSeconds(0);
        TimeSpan toTime = TimeSpan.FromSeconds(100);

        var result = _cpuMetricsController.GetCpuMetrics(fromTime, toTime);

        Assert.IsAssignableFrom<ActionResult<IList<CpuMetric>>>(result);
    }

    [Fact]
    public void GetCpuMetrics_ShouldCall_GetByTimePeriod_From_Repository()
    {
        TimeSpan fromTime = TimeSpan.FromSeconds(0);
        TimeSpan toTime = TimeSpan.FromSeconds(100);
        _mockCpuMetricsRepository.Setup(repository => repository.GetByTimePeriod(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>())).Verifiable();

        var result = _cpuMetricsController.GetCpuMetrics(fromTime, toTime);

        _mockCpuMetricsRepository.Verify(repository => repository.GetByTimePeriod(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()),
        Times.AtMostOnce());
    }

    [Fact]
    public void Create_ShouldCall_Create_From_Repository()
    {
        _mockCpuMetricsRepository.Setup(repository => repository.Create(It.IsAny<CpuMetric>())).Verifiable();

        var result = _cpuMetricsController.Create(new
        CpuMetricCreateRequest
        {
            Time = TimeSpan.FromSeconds(1),
            Value = 50
        });

        _mockCpuMetricsRepository.Verify(repository => repository.Create(It.IsAny<CpuMetric>()),
        Times.AtMostOnce());
    }
}
