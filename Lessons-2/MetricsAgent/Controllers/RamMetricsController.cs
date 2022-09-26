using AutoMapper;
using MetricsAgent.Models;
using MetricsAgent.Models.Dto;
using MetricsAgent.Models.Requests;
using MetricsAgent.Service;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers;

[Route("api/metrics/ram")]
[ApiController]
public class RamMetricsController : Controller
{
    private readonly ILogger<RamMetricsController> _logger;
    private readonly IRamMetricsRepository _ramMetricsRepository;
    private readonly IMapper _mapper;

    public RamMetricsController(ILogger<RamMetricsController> logger, IRamMetricsRepository ramMetricsRepository, IMapper mapper)
    {
        _logger = logger;
        _ramMetricsRepository = ramMetricsRepository;
        _mapper = mapper;
    }

    [HttpPost("create")]
    public IActionResult Create([FromBody] RamMetricCreateRequest request)
    {
        _ramMetricsRepository.Create(_mapper.Map<RamMetric>(request));

        _logger.LogInformation("Create ram metrics call.");
        return Ok();
    }
    [HttpGet("all")]
    public ActionResult<IList<RamMetricDto>> GetAllMetrics()
    {
        _logger.LogInformation("Get all ram metrics call. (Internal method.)");
        return Ok(_ramMetricsRepository.GetAll()?
            .Select(metric => _mapper.Map<RamMetricDto>(metric)).ToList());
    }

    [HttpGet("available/from/{fromTime}/to/{toTime}")]
    public ActionResult<IList<RamMetricDto>> GetRamMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
    {
        _logger.LogInformation("Get all ram metrics call.");
        return Ok(_ramMetricsRepository.GetByTimePeriod(fromTime, toTime)?
            .Select(metric => _mapper.Map<RamMetricDto>(metric)).ToList());
    }
}
