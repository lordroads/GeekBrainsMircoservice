using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers;

[Route("api/metrics/cpu")]
[ApiController]
public class CpuMetricsController : Controller
{
    [HttpGet("from/{fromTime}/to/{toTime}")]
    public IActionResult GetCpuMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
    {
        return Ok();
    }

    [HttpGet("from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
    public IActionResult GetCpuMetricsByPercentile([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime, [FromRoute] int percentile)
    {
        return Ok();
    }
}
