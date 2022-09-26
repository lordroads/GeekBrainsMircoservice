using MetricsAgent.Models;
using Microsoft.Extensions.Options;
using System.Data.SQLite;
using Dapper;

namespace MetricsAgent.Service.Implementations;

public class NetworkMetricsRepository : INetworkMetricsRepository
{
    private readonly IOptions<DatabaseOptions> _databaseOptions;

    public NetworkMetricsRepository(IOptions<DatabaseOptions> databaseOptions)
    {
        _databaseOptions = databaseOptions;
    }

    public void Create(NetworkMetric item)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        connection.Execute("INSERT INTO networkmetrics(value, time) VALUES(@value, @time)", new
        {
            value = item.Value,
            time = item.Time
        });
    }

    public void Delete(int id)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        connection.Execute("DELETE FROM networkmetrics WHERE id=@id", new
        {
            id = id
        });
    }

    public NetworkMetric Get(int id)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        return connection.QuerySingle<NetworkMetric>("SELECT * FROM networkmetrics WHERE id=@id", new
        {
            id = id
        });
    }

    public IList<NetworkMetric> GetAll()
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        return connection.Query<NetworkMetric>("SELECT * FROM networkmetrics").ToList();
    }

    public IList<NetworkMetric> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        return connection.Query<NetworkMetric>("SELECT * FROM networkmetrics WHERE time >= @fromTime and time <= @toTime", new
        {
            fromTime = fromTime,
            toTime = toTime
        }).ToList();
    }

    public void Update(NetworkMetric item)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        connection.Execute("UPDATE networkmetrics SET value = @value, time =@time WHERE id = @id; ", new
        {
            id = item.Id,
            value = item.Value,
            time = item.Time
        });
    }
}