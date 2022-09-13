using MetricsAgent.Models;
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

    public DotnetMetricsController(ILogger<DotnetMetricsController> logger, IDotnetMetricsRepository dotnetMetricsRepository)
    {
        _logger = logger;
        _dotnetMetricsRepository = dotnetMetricsRepository;
    }

    [HttpPost("create")]
    public IActionResult Create([FromBody] DotnetMetricCreateRequest request)
    {
        _dotnetMetricsRepository.Create(new DotnetMetric
        {
            Value = request.Value,
            Time = (int)request.Time.TotalSeconds
        });

        _logger.LogInformation("Create dotnet metrics call.");
        return Ok();
    }

    [HttpGet("errors-count/from/{fromTime}/to/{toTime}")]
    public ActionResult<IList<DotnetMetric>> GetDotnetMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
    {
        _logger.LogInformation("Get all dotnet metrics call.");
        return Ok(_dotnetMetricsRepository.GetByTimePeriod(fromTime, toTime));
    }
}
