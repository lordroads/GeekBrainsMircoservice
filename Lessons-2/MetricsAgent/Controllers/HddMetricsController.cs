using MetricsAgent.Models;
using MetricsAgent.Models.Requests;
using MetricsAgent.Service;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers;

[Route("api/metrics/hdd")]
[ApiController]
public class HddMetricsController : Controller
{
    private readonly ILogger<HddMetricsController> _logger;
    private readonly IHddMetricsRepository _hddMetricsRepository;

    public HddMetricsController(ILogger<HddMetricsController> logger, IHddMetricsRepository hddMetricsRepository)
    {
        _logger = logger;
        _hddMetricsRepository = hddMetricsRepository;
    }

    [HttpPost("create")]
    public IActionResult Create([FromBody] HddMetricCreateRequest request)
    {
        _hddMetricsRepository.Create(new HddMetric
        {
            Value = request.Value,
            Time = (int)request.Time.TotalSeconds
        });

        _logger.LogInformation("Create hdd metrics call.");
        return Ok();
    }

    [HttpGet("left")]
    public ActionResult<IList<HddMetric>> GetHddMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
    {
        _logger.LogInformation("Get all hdd metrics call.");
        return Ok(_hddMetricsRepository.GetByTimePeriod(fromTime, toTime));
    }
}
