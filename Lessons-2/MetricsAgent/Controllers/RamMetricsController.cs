using MetricsAgent.Models;
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

    public RamMetricsController(ILogger<RamMetricsController> logger, IRamMetricsRepository ramMetricsRepository)
    {
        _logger = logger;
        _ramMetricsRepository = ramMetricsRepository;
    }

    [HttpPost("create")]
    public IActionResult Create([FromBody] RamMetricCreateRequest request)
    {
        _ramMetricsRepository.Create(new RamMetric
        {
            Value = request.Value,
            Time = (int)request.Time.TotalSeconds
        });

        _logger.LogInformation("Create ram metrics call.");
        return Ok();
    }

    [HttpGet("available")]
    public ActionResult<IList<RamMetric>> GetRamMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
    {
        _logger.LogInformation("Get all ram metrics call.");
        return Ok(_ramMetricsRepository.GetByTimePeriod(fromTime, toTime));
    }
}
