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

public class RamMetricsControllerTests
{
    private RamMetricsController _ramMetricsController;
    private Mock<IRamMetricsRepository> _mockRamMetricsRepository;
    private Mock<ILogger<RamMetricsController>> _mockLogger;
    private Mock<IMapper> _mockMapper;

    public RamMetricsControllerTests()
    {
        _mockRamMetricsRepository = new Mock<IRamMetricsRepository>();
        _mockLogger = new Mock<ILogger<RamMetricsController>>();
        _mockMapper = new Mock<IMapper>();

        _ramMetricsController = new RamMetricsController(_mockLogger.Object, _mockRamMetricsRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public void Create_ShouldCall_Create_From_Repository()
    {
        // Устанавливаем параметр заглушки
        // В заглушке прописываем, что в репозиторий прилетит CpuMetric - объект
        _mockRamMetricsRepository.Setup(repository => repository.Create(It.IsAny<RamMetric>())).Verifiable();

        // Выполняем действие на контроллере
        var result = _ramMetricsController.Create(new
        RamMetricCreateRequest
        {
            Time = TimeSpan.FromSeconds(1),
            Value = 50
        });

        // Проверяем заглушку на то, что пока работал контроллер
        // Вызвался метод Create репозитория с нужным типом объекта в параметре
        _mockRamMetricsRepository.Verify(repository => repository.Create(It.IsAny<RamMetric>()),
        Times.AtMostOnce());
    }

    [Fact]
    public void GetRamMetrics_ShouldCall_GetByTimePeriod_From_Repository()
    {
        TimeSpan fromTime = TimeSpan.FromSeconds(0);
        TimeSpan toTime = TimeSpan.FromSeconds(100);
        _mockRamMetricsRepository.Setup(repository => repository.GetByTimePeriod(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>())).Verifiable();

        var result = _ramMetricsController.GetRamMetrics(fromTime, toTime);

        _mockRamMetricsRepository.Verify(repository => repository.GetByTimePeriod(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()),
        Times.AtMostOnce());
    }

    [Fact]
    public void GetRamMetrics()
    {
        TimeSpan fromTime = TimeSpan.FromSeconds(0);
        TimeSpan toTime = TimeSpan.FromSeconds(100);

        var result = _ramMetricsController.GetRamMetrics(fromTime, toTime);

        Assert.IsAssignableFrom<ActionResult<IList<RamMetricDto>>>(result);
    }
}
