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

public class HddMetricsControllerTests
{
    private HddMetricsController _hddMetricsController;
    private Mock<IHddMetricsRepository> _mockHddMetricsRepository;
    private Mock<ILogger<HddMetricsController>> _mockLogger;
    private Mock<IMapper> _mockMapper;

    public HddMetricsControllerTests()
    {
        _mockHddMetricsRepository = new Mock<IHddMetricsRepository>();
        _mockLogger = new Mock<ILogger<HddMetricsController>>();
        _mockMapper = new Mock<IMapper>();

        _hddMetricsController = new HddMetricsController(_mockLogger.Object, _mockHddMetricsRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public void Create_ShouldCall_Create_From_Repository()
    {
        // Устанавливаем параметр заглушки
        // В заглушке прописываем, что в репозиторий прилетит CpuMetric - объект
        _mockHddMetricsRepository.Setup(repository => repository.Create(It.IsAny<HddMetric>())).Verifiable();

        // Выполняем действие на контроллере
        var result = _hddMetricsController.Create(new
        HddMetricCreateRequest
        {
            Time = TimeSpan.FromSeconds(1),
            Value = 50
        });

        // Проверяем заглушку на то, что пока работал контроллер
        // Вызвался метод Create репозитория с нужным типом объекта в параметре
        _mockHddMetricsRepository.Verify(repository => repository.Create(It.IsAny<HddMetric>()),
        Times.AtMostOnce());
    }

    [Fact]
    public void GetHddMetrics_ShouldCall_GetByTimePeriod_From_Repository()
    {
        TimeSpan fromTime = TimeSpan.FromSeconds(0);
        TimeSpan toTime = TimeSpan.FromSeconds(100);
        _mockHddMetricsRepository.Setup(repository => repository.GetByTimePeriod(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>())).Verifiable();

        var result = _hddMetricsController.GetHddMetrics(fromTime, toTime);

        _mockHddMetricsRepository.Verify(repository => repository.GetByTimePeriod(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()),
        Times.AtMostOnce());
    }

    [Fact]
    public void GetHddMetrics()
    {
        TimeSpan fromTime = TimeSpan.FromSeconds(0);
        TimeSpan toTime = TimeSpan.FromSeconds(100);

        var result = _hddMetricsController.GetHddMetrics(fromTime, toTime);

        Assert.IsAssignableFrom<ActionResult<IList<HddMetricDto>>>(result);
    }
}
