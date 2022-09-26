﻿using MetricsControl.Models;
using MetricsControl.Models.Requests;
using MetricsControl.Models.Response;
using MetricsControl.Service;
using MetricsControl.Service.Client;
using Microsoft.AspNetCore.Mvc;

namespace MetricsControl.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DotnetMetricsController : Controller
{
    private readonly IAgentRepository _agentRepository;
    private readonly IMetricsAgentClient _metricsAgentClient;
    private readonly ILogger<CpuMetricsController> _logger;

    public DotnetMetricsController(IMetricsAgentClient metricsAgentClient, ILogger<CpuMetricsController> logger, IAgentRepository agentRepository)
    {
        _metricsAgentClient = metricsAgentClient;
        _logger = logger;
        _agentRepository = agentRepository;
    }

    [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
    public ActionResult<AllDotnetMetricsApiResponse> GetMetricsFromAgent([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
    {

        AgentInfo agent = _agentRepository.Get(agentId);

        if (agent == null)
        {
            return BadRequest();
        }

        _logger.LogInformation($"(DotNet) starting new request to metrics agent by id");

        var metrics = _metricsAgentClient.GetDotnetMetrics(new GetAllDotnetMetricsApiRequest
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

    [HttpGet("all/from/{fromTime}/to/{toTime}")]
    public ActionResult<IList<AllDotnetMetricsApiResponse>> GetCountErrorsFromAll([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
    {
        _logger.LogInformation($"(DotNet)  starting new request to metrics agent all");

        List<AllDotnetMetricsApiResponse> allDotnetMetricsApiResponses = new List<AllDotnetMetricsApiResponse>();
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

            var metrics = _metricsAgentClient.GetDotnetMetrics(new GetAllDotnetMetricsApiRequest
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

            allDotnetMetricsApiResponses.Add(metrics);
        }

        return Ok(allDotnetMetricsApiResponses);
    }
}
