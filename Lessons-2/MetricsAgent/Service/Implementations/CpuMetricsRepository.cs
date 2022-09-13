using MetricsAgent.Models;
using Microsoft.Extensions.Options;
using System.Data.SQLite;
using Dapper;

namespace MetricsAgent.Service.Implementations;

public class CpuMetricsRepository : ICpuMetricsRepository
{
    private readonly IOptions<DatabaseOptions> _databaseOptions;

    public CpuMetricsRepository(IOptions<DatabaseOptions> databaseOptions)
    {
        _databaseOptions = databaseOptions;
    }

    public void Create(CpuMetric item)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        connection.Execute("INSERT INTO cpumetrics(value, time) VALUES(@value, @time)", new
        {
            value = item.Value,
            time = item.Time
        });
    }

    public void Delete(int id)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        connection.Execute("DELETE FROM cpumetrics WHERE id=@id", new
        {
            id = id
        });
    }

    public CpuMetric? Get(int id)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        return connection.QuerySingle<CpuMetric>("SELECT * FROM cpumetrics WHERE id=@id", new
        {
            id = id
        });
    }

    public IList<CpuMetric> GetAll()
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        return connection.Query<CpuMetric>("SELECT * FROM cpumetrics").ToList();
    }

    public IList<CpuMetric> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        return connection.Query<CpuMetric>("SELECT * FROM cpumetrics WHERE time >= @fromTime and time <= @toTime", new
        {
            fromTime = fromTime.TotalSeconds,
            toTime = toTime.TotalSeconds
        }).ToList();
    }

    public void Update(CpuMetric item)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        connection.Execute("UPDATE cpumetrics SET value = @value, time =@time WHERE id = @id", new
        {
            id = item.Id,
            value = item.Value,
            time = item.Time
        });
    }
}
