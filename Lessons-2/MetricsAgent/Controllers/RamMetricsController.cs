using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers;

[Route("api/metrics/ram")]
[ApiController]
public class RamMetricsController : Controller
{
    [HttpGet("available")]
    public ActionResult<int> GetRamMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
    {
        return Ok();
    }
}
