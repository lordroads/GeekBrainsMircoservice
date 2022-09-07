using MetricsControl.Controllers;
using MetricsControl.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Xunit;
using Xunit.Priority;

namespace MetricsManagerTests;

public class AgentsControllerTests
{
    private AgentsController _agentsController;
    private AgentPool _agentPool;

    public AgentsControllerTests()
    {
        _agentPool = LazySingltone.Instance;
        _agentsController = new AgentsController(_agentPool);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void RegisterAgentTest(int agentId)
    {
        AgentInfo agentInfo = new AgentInfo() { AgentId = agentId, Enable = true };
        IActionResult actionResult = _agentsController.RegisterAgent(agentInfo);
        Assert.IsAssignableFrom<IActionResult>(actionResult);
    }

    [Fact]
    [Priority(2)]
    public void GetAgentsTest()
    {
        ActionResult<AgentInfo[]> actionResult = _agentsController.GetAllAgents();
        OkObjectResult result = Assert.IsAssignableFrom<OkObjectResult>(actionResult.Result);
        Assert.NotNull(result.Value as AgentInfo[]);
        Assert.NotEmpty(result.Value as AgentInfo[]);
    }
}
