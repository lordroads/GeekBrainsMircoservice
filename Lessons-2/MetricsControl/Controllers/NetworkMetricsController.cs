using Microsoft.AspNetCore.Mvc;

namespace MetricsControl.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NetworkMetricsController : Controller
{
    [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
    public IActionResult GetMetricsFromAgent([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
    {
        return Ok();
    }

    [HttpGet("from/{fromTime}/to/{toTime}")]
    public IActionResult GetMetricsFromAll([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
    {
        return Ok();
    }
}
