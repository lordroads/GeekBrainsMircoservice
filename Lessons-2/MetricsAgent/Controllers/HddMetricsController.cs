using AutoMapper;
using MetricsAgent.Models;
using MetricsAgent.Models.Dto;
using MetricsAgent.Models.Requests;
using MetricsAgent.Models.Response;
using MetricsAgent.Service;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers;

[Route("api/metrics/hdd")]
[ApiController]
public class HddMetricsController : Controller
{
    private readonly ILogger<HddMetricsController> _logger;
    private readonly IHddMetricsRepository _hddMetricsRepository;
    private readonly IMapper _mapper;

    public HddMetricsController(ILogger<HddMetricsController> logger, IHddMetricsRepository hddMetricsRepository, IMapper mapper)
    {
        _logger = logger;
        _hddMetricsRepository = hddMetricsRepository;
        _mapper = mapper;
    }

    [HttpPost("create")]
    public IActionResult Create([FromBody] HddMetricCreateRequest request)
    {
        _hddMetricsRepository.Create(_mapper.Map<HddMetric>(request));

        _logger.LogInformation("Create hdd metrics call.");
        return Ok();
    }

    [HttpGet("all")]
    public ActionResult<AllHddMetricsApiResponse> GetAllMetrics()
    {
        _logger.LogInformation("Get all hdd metrics call. (Internal method.)");
        return Ok(_mapper.Map<AllHddMetricsApiResponse>(_hddMetricsRepository.GetAll()
            .Select(metric => _mapper.Map<HddMetricDto>(metric)).ToList()));
    }

    [HttpGet("left/from/{fromTime}/to/{toTime}")]
    public ActionResult<AllHddMetricsApiResponse> GetHddMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
    {
        _logger.LogInformation("Get all hdd metrics call.");
        return Ok(_mapper.Map<AllHddMetricsApiResponse>(_hddMetricsRepository.GetByTimePeriod(fromTime, toTime)
            .Select(metric => _mapper.Map<HddMetricDto>(metric)).ToList()));
    }
}
