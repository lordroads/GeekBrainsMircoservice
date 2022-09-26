using MetricsAgent.Models;
using Microsoft.Extensions.Options;
using System.Data.SQLite;
using Dapper;

namespace MetricsAgent.Service.Implementations;

public class HddMetricsRepository : IHddMetricsRepository
{
    private readonly IOptions<DatabaseOptions> _databaseOptions;

    public HddMetricsRepository(IOptions<DatabaseOptions> databaseOptions)
    {
        _databaseOptions = databaseOptions;
    }

    public void Create(HddMetric item)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        connection.Execute("INSERT INTO hddmetrics(value, time) VALUES(@value, @time)", new
        {
            value = item.Value,
            time = item.Time
        });
    }

    public void Delete(int id)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        connection.Execute("DELETE FROM hddmetrics WHERE id=@id", new
        {
            id = id
        });
    }

    public HddMetric Get(int id)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);
        return connection.QuerySingle<HddMetric>("SELECT * FROM hddmetrics WHERE id=@id", new
        {
            id = id
        });
    }

    public IList<HddMetric> GetAll()
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        return connection.Query<HddMetric>("SELECT * FROM hddmetrics").ToList();
    }

    public IList<HddMetric> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        return connection.Query<HddMetric>("SELECT * FROM hddmetrics WHERE time >= @fromTime and time <= @toTime", new
        {
            fromTime = fromTime.TotalSeconds,
            toTime = toTime.TotalSeconds
        }).ToList();
    }

    public void Update(HddMetric item)
    {
        using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

        connection.Execute("UPDATE hddmetrics SET value = @value, time =@time WHERE id = @id; ", new
        {
            id = item.Id,
            value = item.Value,
            time = item.Time
        });
    }
}