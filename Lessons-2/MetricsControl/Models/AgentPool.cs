namespace MetricsControl.Models;

public class AgentPool
{
    private Dictionary<int, AgentInfo> _agents;

    public Dictionary<int, AgentInfo> Agents 
    { 
        get { return _agents; } 
        set { _agents = value; }
    }

    public AgentPool()
    {
        _agents = new Dictionary<int, AgentInfo>();
    }

    public void Add(AgentInfo agent)
    {
        if (!_agents.ContainsKey(agent.Id))
        {
            _agents.Add(agent.Id, agent);
        }
    }

    public AgentInfo[] Get()
    {
        return _agents.Values.ToArray();
    }
}
