using MetricsAgent.Models;
using Microsoft.Extensions.Options;
using System.Data.SQLite;
using Dapper;

namespace MetricsAgent.Service.Implementations;

public class RamMetricsRepository : IRamMetricsRepository
{
    private readonly IOptions<DatabaseOptions> _databaseOptions;

    public RamMetricsRepository(IOptions<DatabaseOptions> databaseOptions)
    {
        _databaseOptions = databaseOptions;
    }

    public void Create(RamMetric item)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        connection.Execute("INSERT INTO rammetrics(value, time) VALUES(@value, @time)", new
        {
            value = item.Value,
            time = item.Time,
        });
    }

    public void Delete(int id)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        connection.Execute("DELETE FROM rammetrics WHERE id=@id", new
        {
            id = id
        });
    }

    public RamMetric Get(int id)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        return connection.QuerySingle<RamMetric>("SELECT * FROM rammetrics WHERE id=@id", new
        {
            id = id
        });
    }

    public IList<RamMetric> GetAll()
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        return connection.Query<RamMetric>("SELECT * FROM rammetrics").ToList();
    }

    public IList<RamMetric> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        return connection.Query<RamMetric>("SELECT * FROM rammetrics WHERE time >= @fromTime and time <= @toTime", new
        {
            fromTime = fromTime,
            toTime = toTime
        }).ToList();
    }

    public void Update(RamMetric item)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        connection.Execute("UPDATE rammetrics SET value = @value, time =@time WHERE id = @id; ", new
        {
            id = item.Id,
            value = item.Value,
            time = item.Time
        });
    }
}