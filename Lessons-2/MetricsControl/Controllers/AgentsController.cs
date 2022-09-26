using MetricsControl.Models;
using MetricsControl.Models.Dto;
using MetricsControl.Service;
using Microsoft.AspNetCore.Mvc;

namespace MetricsControl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : Controller
    {
        private readonly IAgentRepository _agentsRepository;
        private readonly ILogger<AgentsController> _logger;

        public AgentsController(IAgentRepository agentPool, ILogger<AgentsController> logger)
        {
            _agentsRepository = agentPool;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult RegisterAgent([FromBody] AgentInfoDto agentInfo)
        {
            _logger.LogInformation("Register Agent call.");

            if (agentInfo is not null)
            {
                _agentsRepository.Create(new AgentInfo
                {
                    Address = agentInfo.Address.ToString(),
                    Enable = agentInfo.Enable
                });

                return Ok();
            }

            return BadRequest();
        }

        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            _logger.LogInformation("Enable Agent By Id call.");

            AgentInfo agent = _agentsRepository.Get(agentId);

            if (agent == null)
                return BadRequest();

            agent.Enable = true;

            _agentsRepository.Update(agent);

            return Ok();
        }

        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            _logger.LogInformation("Disable Agent By Id call.");

            AgentInfo agent = _agentsRepository.Get(agentId);

            if (agent == null)
                return BadRequest();

            agent.Enable = false;

            _agentsRepository.Update(agent);

            return Ok();
        }

        [HttpGet("get")]
        public ActionResult<IList<AgentInfo>> GetAllAgents()
        {
            _logger.LogInformation("Get All Agents call.");
            return Ok(_agentsRepository.GetAll());
        }
    }
}
