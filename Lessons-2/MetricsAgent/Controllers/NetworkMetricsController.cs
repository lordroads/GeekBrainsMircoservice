using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers;

[Route("api/metrics/network")]
[ApiController]
public class NetworkMetricsController : Controller
{
    [HttpGet("from/{fromTime}/to/{toTime}/")]
    public IActionResult GetNetworkMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
    {
        return Ok();
    }
}
