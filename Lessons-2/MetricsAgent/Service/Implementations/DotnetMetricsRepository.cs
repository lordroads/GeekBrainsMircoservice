using MetricsAgent.Models;
using System.Data.SQLite;
using Dapper;
using Microsoft.Extensions.Options;

namespace MetricsAgent.Service.Implementations;

public class DotnetMetricsRepository : IDotnetMetricsRepository
{
    private readonly IOptions<DatabaseOptions> _databaseOptions;

    public DotnetMetricsRepository(IOptions<DatabaseOptions> databaseOptions)
    {
        _databaseOptions = databaseOptions;
    }

    public void Create(DotnetMetric item)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        connection.Execute("INSERT INTO dotnetmetrics(value, time) VALUES(@value, @time)", new
        {
            value = item.Value,
            time = item.Time
        });
    }

    public void Delete(int id)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        connection.Execute("DELETE FROM dotnetmetrics WHERE id=@id", new
        {
            id = id
        });
    }

    public DotnetMetric Get(int id)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        return connection.QuerySingle<DotnetMetric>("SELECT * FROM dotnetmetrics WHERE id=@id", new
        {
            id = id
        });
    }

    public IList<DotnetMetric> GetAll()
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        return connection.Query<DotnetMetric>("SELECT * FROM dotnetmetrics").ToList();
    }

    public IList<DotnetMetric> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        return connection.Query<DotnetMetric>("SELECT * FROM dotnetmetrics WHERE time >= @fromTime and time <= @toTime", new
        {
            fromTime = fromTime.TotalSeconds,
            toTime = toTime.TotalSeconds
        }).ToList();
    }

    public void Update(DotnetMetric item)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        connection.Execute("UPDATE dotnetmetrics SET value = @value, time =@time WHERE id = @id; ", new
        {
            id = item.Id,
            value = item.Value,
            time = item.Time
        });
    }
}