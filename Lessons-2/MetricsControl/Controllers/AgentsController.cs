using MetricsControl.Models;
using Microsoft.AspNetCore.Mvc;

namespace MetricsControl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : Controller
    {
        private readonly AgentPool _agentPool;

        public AgentsController(AgentPool agentPool)
        {
            _agentPool = agentPool;
        }

        [HttpPost]
        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {
            if (agentInfo is not null)
            {
                _agentPool.Add(agentInfo);
            }

            return Ok();
        }

        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            if (_agentPool.Agents.ContainsKey(agentId))
            {
                _agentPool.Agents[agentId].Enable = true;
            }

            return Ok();
        }

        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            if (_agentPool.Agents.ContainsKey(agentId))
            {
                _agentPool.Agents[agentId].Enable = false;
            }

            return Ok();
        }

        [HttpGet("get")]
        public ActionResult<AgentInfo[]> GetAllAgents()
        {
            return Ok(_agentPool.Get());
        }
    }
}
