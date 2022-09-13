using MetricsAgent.Models;
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

    public CpuMetricsController(ILogger<CpuMetricsController> logger, ICpuMetricsRepository cpuMetricsRepository)
    {
        _logger = logger;
        _cpuMetricsRepository = cpuMetricsRepository;
    }

    [HttpPost("create")]
    public IActionResult Create([FromBody] CpuMetricCreateRequest request)
    {
        _cpuMetricsRepository.Create(new CpuMetric
        {
            Value = request.Value,
            Time = (int)request.Time.TotalSeconds
        });

        _logger.LogInformation("Create cpu metrics call.");
        return Ok();
    }

    [HttpGet("from/{fromTime}/to/{toTime}")]
    public ActionResult<IList<CpuMetric>> GetCpuMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
    {
        _logger.LogInformation("Get all cpu metrics call.");
        return Ok(_cpuMetricsRepository.GetByTimePeriod(fromTime, toTime));
    }

    [HttpGet("from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
    public ActionResult<IList<CpuMetric>> GetCpuMetricsByPercentile([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime, [FromRoute] int percentile)
    {
        _logger.LogInformation("Get all cpu metrics by percentile call.");
        return Ok(_cpuMetricsRepository.GetByTimePeriod(fromTime, toTime).Where(x => x.Value >= percentile));
    }
}
