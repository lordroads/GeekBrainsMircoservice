using MetricsControl.Models;
using MetricsControl.Models.Requests;
using MetricsControl.Models.Response;
using MetricsControl.Service;
using MetricsControl.Service.Client;
using Microsoft.AspNetCore.Mvc;

namespace MetricsControl.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CpuMetricsController : Controller
{
    private readonly IAgentRepository _agentRepository;
    private readonly IMetricsAgentClient _metricsAgentClient;
    private readonly ILogger<CpuMetricsController> _logger;

    public CpuMetricsController(IMetricsAgentClient metricsAgentClient, ILogger<CpuMetricsController> logger, IAgentRepository agentRepository)
    {
        _metricsAgentClient = metricsAgentClient;
        _logger = logger;
        _agentRepository = agentRepository;
    }

    [HttpGet("get-all-by-id")]
    public ActionResult<AllCpuMetricsApiResponse> GetCpuMetricsFromAgent([FromQuery] int agentId, [FromQuery] TimeSpan fromTime, [FromQuery] TimeSpan toTime)
    {
        AgentInfo agent = _agentRepository.Get(agentId);

        if (agent == null)
        {
            return BadRequest();
        }

        _logger.LogInformation($"(CPU) starting new request to metrics agent by id");

        var metrics = _metricsAgentClient.GetCpuMetrics(new GetAllCpuMetricsApiRequest
        {
            AgentAddress = new Uri(agent.Address),
            FromTime = fromTime,
            ToTime = toTime
        });

        if (metrics == null)
        {
            return BadRequest();
        }

        metrics.AgentId = agentId;

        return Ok(metrics);
    }

    [HttpGet("get-all")]
    public ActionResult<IList<AllCpuMetricsApiResponse>> GetCpuMetricsFromAll([FromQuery] TimeSpan fromTime, [FromQuery] TimeSpan toTime)
    {
        _logger.LogInformation($"(CPU)  starting new request to metrics agent all");

        List<AllCpuMetricsApiResponse> allCpuMetricsApiResponses = new List<AllCpuMetricsApiResponse>();
        var agents = _agentRepository.GetAll();

        if (agents == null)
        {
            return BadRequest();
        }

        foreach (var agent in agents)
        {
            if (agent == null)
            {
                return BadRequest();
            }

            var metrics = _metricsAgentClient.GetCpuMetrics(new GetAllCpuMetricsApiRequest
            {
                AgentAddress = new Uri(agent.Address),
                FromTime = fromTime,
                ToTime = toTime
            });

            if (metrics == null)
            {
                return BadRequest(agent);
            }

            metrics.AgentId = agent.Id;

            allCpuMetricsApiResponses.Add(metrics);
        }

        return Ok(allCpuMetricsApiResponses);
    }

    [HttpGet("get-all-by-percentile")]
    public IActionResult GetCpuMetricsFromAllByPercentile([FromQuery] TimeSpan fromTime, [FromQuery] TimeSpan toTime, [FromQuery] int percentile)
    {
        return Ok();
    }
}
