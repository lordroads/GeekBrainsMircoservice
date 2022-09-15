﻿using MetricsControl.Models;
using MetricsControl.Models.Requests;
using MetricsControl.Models.Response;
using MetricsControl.Service;
using MetricsControl.Service.Client;
using Microsoft.AspNetCore.Mvc;

namespace MetricsControl.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NetworkMetricsController : Controller
{
    private readonly IAgentRepository _agentRepository;
    private readonly IMetricsAgentClient _metricsAgentClient;
    private readonly ILogger<CpuMetricsController> _logger;

    public NetworkMetricsController(IMetricsAgentClient metricsAgentClient, ILogger<CpuMetricsController> logger, IAgentRepository agentRepository)
    {
        _metricsAgentClient = metricsAgentClient;
        _logger = logger;
        _agentRepository = agentRepository;
    }

    [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
    public ActionResult<AllNetworkMetricsApiResponse> GetMetricsFromAgent([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
    {
        AgentInfo agent = _agentRepository.Get(agentId);

        if (agent == null)
        {
            return BadRequest();
        }

        _logger.LogInformation($"(Network) starting new request to metrics agent by id");

        var metrics = _metricsAgentClient.GetNetworkMetrics(new GetAllNetworkMetricsApiRequest
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

    [HttpGet("from/{fromTime}/to/{toTime}")]
    public ActionResult<IList<AllNetworkMetricsApiResponse>> GetMetricsFromAll([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
    {
        _logger.LogInformation($"(Network)  starting new request to metrics agent all");

        List<AllNetworkMetricsApiResponse> allNetworkMetricsApiResponses = new List<AllNetworkMetricsApiResponse>();
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

            var metrics = _metricsAgentClient.GetNetworkMetrics(new GetAllNetworkMetricsApiRequest
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

            allNetworkMetricsApiResponses.Add(metrics);
        }

        return Ok(allNetworkMetricsApiResponses);
    }
}
