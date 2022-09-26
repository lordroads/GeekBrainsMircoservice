using AutoMapper;
using MetricsAgent.Models;
using MetricsAgent.Models.Dto;
using MetricsAgent.Models.Requests;
using MetricsAgent.Service;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers;

[Route("api/metrics/cpu")]
[ApiController]
public class CpuMetricsController : Controller
{
    private readonly ILogger<CpuMetricsController> _logger;
    private readonly ICpuMetricsRepository _cpuMetricsRepository;
    private readonly IMapper _mapper;

    public CpuMetricsController(ILogger<CpuMetricsController> logger, 
                                ICpuMetricsRepository cpuMetricsRepository,
                                IMapper mapper)
    {
        _logger = logger;
        _cpuMetricsRepository = cpuMetricsRepository;
        _mapper = mapper;
    }

    [HttpPost("create")]
    public IActionResult Create([FromBody] CpuMetricCreateRequest request)
    {
        _cpuMetricsRepository.Create(_mapper.Map<CpuMetric>(request));

        _logger.LogInformation("Create cpu metrics call.");
        return Ok();
    }

    [HttpGet("all")]
    public ActionResult<IList<CpuMetricDto>> GetAllMetrics()
    {
        _logger.LogInformation("Get all cpu metrics call. (Internal method.)");
        return Ok(_cpuMetricsRepository.GetAll()?
            .Select(metric => _mapper.Map<CpuMetricDto>(metric)).ToList());
    }

    [HttpGet("from/{fromTime}/to/{toTime}")]
    public ActionResult<IList<CpuMetricDto>> GetCpuMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
    {
        _logger.LogInformation("Get all cpu metrics call.");
        return Ok(_cpuMetricsRepository.GetByTimePeriod(fromTime, toTime)?
            .Select(metric => _mapper.Map<CpuMetricDto>(metric)).ToList());
    }

    [HttpGet("from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
    public ActionResult<IList<CpuMetricDto>> GetCpuMetricsByPercentile([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime, [FromRoute] int percentile)
    {
        _logger.LogInformation("Get all cpu metrics by percentile call.");
        return Ok(_cpuMetricsRepository.GetByTimePeriod(fromTime, toTime)
            .Where(x => x.Value >= percentile)
            .Select(metric => _mapper.Map<CpuMetricDto>(metric)).ToList());
    }
}
