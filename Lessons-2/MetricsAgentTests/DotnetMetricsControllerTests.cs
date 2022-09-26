using AutoMapper;
using MetricsAgent.Controllers;
using MetricsAgent.Models;
using MetricsAgent.Models.Dto;
using MetricsAgent.Models.Requests;
using MetricsAgent.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace MetricsAgentTests;

public class DotnetMetricsControllerTests
{
    private readonly DotnetMetricsController _dotnetMetricsController;
    private Mock<IDotnetMetricsRepository> _mockDotnetMetricsRepository;
    private Mock<ILogger<DotnetMetricsController>> _mockLogger;
    private Mock<IMapper> _mockMapper;

    public DotnetMetricsControllerTests()
    {
        _mockLogger = new Mock<ILogger<DotnetMetricsController>>();
        _mockDotnetMetricsRepository = new Mock<IDotnetMetricsRepository>();
        _mockMapper = new Mock<IMapper>();

        _dotnetMetricsController = new DotnetMetricsController(_mockLogger.Object, _mockDotnetMetricsRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public void GetDotnetMetrics()
    {
        TimeSpan fromTime = TimeSpan.FromSeconds(0);
        TimeSpan toTime = TimeSpan.FromSeconds(100);

        var result = _dotnetMetricsController.GetDotnetMetrics(fromTime, toTime);

        Assert.IsAssignableFrom<ActionResult<IList<DotnetMetricDto>>>(result);
    }

    [Fact]
    public void GetDotnetMetrics_ShouldCall_GetByTimePeriod_From_Repository()
    {
        TimeSpan fromTime = TimeSpan.FromSeconds(0);
        TimeSpan toTime = TimeSpan.FromSeconds(100);
        _mockDotnetMetricsRepository.Setup(repository => repository.GetByTimePeriod(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>())).Verifiable();

        var result = _dotnetMetricsController.GetDotnetMetrics(fromTime, toTime);

        _mockDotnetMetricsRepository.Verify(repository => repository.GetByTimePeriod(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()),
        Times.AtMostOnce());
    }

    [Fact]
    public void Create_ShouldCall_Create_From_Repository()
    {
        // Устанавливаем параметр заглушки
        // В заглушке прописываем, что в репозиторий прилетит CpuMetric - объект
        _mockDotnetMetricsRepository.Setup(repository => repository.Create(It.IsAny<DotnetMetric>())).Verifiable();

        // Выполняем действие на контроллере
        var result = _dotnetMetricsController.Create(new
        DotnetMetricCreateRequest
        {
            Time = TimeSpan.FromSeconds(1),
            Value = 50
        });

        // Проверяем заглушку на то, что пока работал контроллер
        // Вызвался метод Create репозитория с нужным типом объекта в параметре
        _mockDotnetMetricsRepository.Verify(repository => repository.Create(It.IsAny<DotnetMetric>()),
        Times.AtMostOnce());
    }
}
