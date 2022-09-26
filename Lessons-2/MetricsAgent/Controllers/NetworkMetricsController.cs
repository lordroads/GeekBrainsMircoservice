using AutoMapper;
using MetricsAgent.Models;
using MetricsAgent.Models.Dto;
using MetricsAgent.Models.Requests;
using MetricsAgent.Service;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers;

[Route("api/metrics/network")]
[ApiController]
public class NetworkMetricsController : Controller
{
    private readonly ILogger<NetworkMetricsController> _logger;
    private readonly INetworkMetricsRepository _networkMetricsRepository;
    private readonly IMapper _mapper;

    public NetworkMetricsController(ILogger<NetworkMetricsController> logger, INetworkMetricsRepository networkMetricsRepository, IMapper mapper)
    {
        _logger = logger;
        _networkMetricsRepository = networkMetricsRepository;
        _mapper = mapper;
    }

    [HttpPost("create")]
    public IActionResult Create([FromBody] NetworkMetricCreateRequest request)
    {
        _networkMetricsRepository.Create(_mapper.Map<NetworkMetric>(request));

        _logger.LogInformation("Create network metrics call.");
        return Ok();
    }

    [HttpGet("all")]
    public ActionResult<IList<NetworkMetricDto>> GetAllMetrics()
    {
        _logger.LogInformation("Get all network metrics call. (Internal method.)");
        return Ok(_networkMetricsRepository.GetAll()?
            .Select(metric => _mapper.Map<NetworkMetricDto>(metric)).ToList());
    }

    [HttpGet("from/{fromTime}/to/{toTime}/")]
    public ActionResult<IList<NetworkMetricDto>> GetNetworkMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
    {
        _logger.LogInformation("Get all network metrics call.");
        return Ok(_networkMetricsRepository.GetByTimePeriod(fromTime, toTime)?
            .Select(metric => _mapper.Map<NetworkMetricDto>(metric)).ToList());
    }
}
