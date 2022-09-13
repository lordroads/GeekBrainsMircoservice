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

public class NetworkMetricsControllerTests
{
    private NetworkMetricsController _networkMetricsController;
    private Mock<INetworkMetricsRepository> _mockNetworkMetricsRepository;
    private Mock<ILogger<NetworkMetricsController>> _mockLogger;

    public NetworkMetricsControllerTests()
    {
        _mockNetworkMetricsRepository = new Mock<INetworkMetricsRepository>();
        _mockLogger = new Mock<ILogger<NetworkMetricsController>>();

        _networkMetricsController = new NetworkMetricsController(_mockLogger.Object, _mockNetworkMetricsRepository.Object);
    }

    [Fact]
    public void Create_ShouldCall_Create_From_Repository()
    {
        // Устанавливаем параметр заглушки
        // В заглушке прописываем, что в репозиторий прилетит CpuMetric - объект
        _mockNetworkMetricsRepository.Setup(repository => repository.Create(It.IsAny<NetworkMetric>())).Verifiable();

        // Выполняем действие на контроллере
        var result = _networkMetricsController.Create(new
        NetworkMetricCreateRequest
        {
            Time = TimeSpan.FromSeconds(1),
            Value = 50
        });

        // Проверяем заглушку на то, что пока работал контроллер
        // Вызвался метод Create репозитория с нужным типом объекта в параметре
        _mockNetworkMetricsRepository.Verify(repository => repository.Create(It.IsAny<NetworkMetric>()),
        Times.AtMostOnce());
    }

    [Fact]
    public void GetNetworkMetrics_ShouldCall_GetByTimePeriod_From_Repository()
    {
        TimeSpan fromTime = TimeSpan.FromSeconds(0);
        TimeSpan toTime = TimeSpan.FromSeconds(100);
        _mockNetworkMetricsRepository.Setup(repository => repository.GetByTimePeriod(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>())).Verifiable();

        var result = _networkMetricsController.GetNetworkMetrics(fromTime, toTime);

        _mockNetworkMetricsRepository.Verify(repository => repository.GetByTimePeriod(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()),
        Times.AtMostOnce());
    }

    [Fact]
    public void GetNetworkMetrics()
    {
        TimeSpan fromTime = TimeSpan.FromSeconds(0);
        TimeSpan toTime = TimeSpan.FromSeconds(100);

        var result = _networkMetricsController.GetNetworkMetrics(fromTime, toTime);

        Assert.IsAssignableFrom<ActionResult<IList<NetworkMetric>>>(result);
    }
}
