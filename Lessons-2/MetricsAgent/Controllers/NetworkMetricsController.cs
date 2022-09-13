using MetricsAgent.Models;
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

    public NetworkMetricsController(ILogger<NetworkMetricsController> logger, INetworkMetricsRepository networkMetricsRepository)
    {
        _logger = logger;
        _networkMetricsRepository = networkMetricsRepository;
    }

    [HttpPost("create")]
    public IActionResult Create([FromBody] NetworkMetricCreateRequest request)
    {
        _networkMetricsRepository.Create(new NetworkMetric
        {
            Value = request.Value,
            Time = (int)request.Time.TotalSeconds
        });

        _logger.LogInformation("Create network metrics call.");
        return Ok();
    }

    [HttpGet("from/{fromTime}/to/{toTime}/")]
    public ActionResult<IList<NetworkMetric>> GetNetworkMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
    {
        _logger.LogInformation("Get all network metrics call.");
        return Ok(_networkMetricsRepository.GetByTimePeriod(fromTime, toTime));
    }
}
