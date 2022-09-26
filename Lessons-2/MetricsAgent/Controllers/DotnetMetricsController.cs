using AutoMapper;
using MetricsAgent.Models;
using MetricsAgent.Models.Dto;
using MetricsAgent.Models.Requests;
using MetricsAgent.Service;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers;

[Route("api/metrics/dotnet")]
[ApiController]
public class DotnetMetricsController : Controller
{
    private readonly ILogger<DotnetMetricsController> _logger;
    private readonly IDotnetMetricsRepository _dotnetMetricsRepository;
    private readonly IMapper _mapper;

    public DotnetMetricsController(ILogger<DotnetMetricsController> logger, IDotnetMetricsRepository dotnetMetricsRepository, IMapper mapper)
    {
        _logger = logger;
        _dotnetMetricsRepository = dotnetMetricsRepository;
        _mapper = mapper;
    }

    [HttpPost("create")]
    public IActionResult Create([FromBody] DotnetMetricCreateRequest request)
    {
        _dotnetMetricsRepository.Create(_mapper.Map<DotnetMetric>(request));

        _logger.LogInformation("Create dotnet metrics call.");
        return Ok();
    }

    [HttpGet("all")]
    public ActionResult<IList<DotnetMetricDto>> GetAllMetrics()
    {
        _logger.LogInformation("Get all dotnet metrics call. (Internal method.)");
        return Ok(_dotnetMetricsRepository.GetAll()?
            .Select(metric => _mapper.Map<DotnetMetricDto>(metric)).ToList());
    }

    [HttpGet("errors-count/from/{fromTime}/to/{toTime}")]
    public ActionResult<IList<DotnetMetricDto>> GetDotnetMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
    {
        _logger.LogInformation("Get all dotnet metrics call.");
        return Ok(_dotnetMetricsRepository.GetByTimePeriod(fromTime, toTime)?
            .Select(metric => _mapper.Map<DotnetMetricDto>(metric)).ToList());
    }
}
