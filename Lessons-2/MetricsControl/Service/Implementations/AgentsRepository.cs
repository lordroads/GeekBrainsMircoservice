using Dapper;
using MetricsControl.Models;
using Microsoft.Extensions.Options;
using System.Data.SQLite;

namespace MetricsControl.Service.Implementations;

public class AgentsRepository : IAgentRepository
{
    private readonly IOptions<DatabaseOptions> _databaseOptions;

    public AgentsRepository(IOptions<DatabaseOptions> databaseOptions)
    {
        _databaseOptions = databaseOptions;
    }

    public void Create(AgentInfo item)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        connection.Execute("INSERT INTO infoofagents(address, enable) VALUES(@address, @enable)", new
        {
            address = item.Address,
            enable = item.Enable
        });
    }

    public void Delete(int id)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        connection.Execute("DELETE FROM infoofagents WHERE id = @id", new
        {
            id = id
        });
    }

    public AgentInfo Get(int id)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        return connection.QuerySingle<AgentInfo>("SELECT * FROM infoofagents WHERE id=@id", new
        {
            id = id
        });
    }

    public IList<AgentInfo> GetAll()
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        return connection.Query<AgentInfo>("SELECT * FROM infoofagents").ToList(); ;
    }

    public void Update(AgentInfo item)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        connection.Execute("UPDATE infoofagents SET address = @address, enable =@enable WHERE id = @id", new
        {
            agentid = item.Id,
            address = item.Address,
            enable = item.Enable
        });
    }
}
