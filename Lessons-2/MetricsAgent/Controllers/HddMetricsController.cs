using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers;

[Route("api/metrics/hdd")]
[ApiController]
public class HddMetricsController : Controller
{
    [HttpGet("left")]
    public ActionResult<int> GetHddMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
    {
        return Ok();
    }
}
