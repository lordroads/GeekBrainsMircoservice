using MetricsAgent.Models;
using System.Data.SQLite;

namespace MetricsAgent.Service.Implementations;

public class RamMetricsRepository : IRamMetricsRepository
{
    private const string connectionString = "Data Source = metrics.db; Version = 3; Pooling = true; Max Pool Size = 100; ";

    public void Create(RamMetric item)
    {
        using var connection = new SQLiteConnection(connectionString);
        connection.Open();
        using (var command = new SQLiteCommand(connection))
        {
            command.CommandText = "INSERT INTO rammetrics(value, time) VALUES(@value, @time)";

            command.Parameters.AddWithValue("@value", item.Value);
            command.Parameters.AddWithValue("@time", item.Time);

            command.Prepare();
            command.ExecuteNonQuery();
        }
    }

    public void Delete(int id)
    {
        using var connection = new SQLiteConnection(connectionString);
        connection.Open();

        using (var command = new SQLiteCommand(connection))
        {
            command.CommandText = "DELETE FROM rammetrics WHERE id=@id";

            command.Parameters.AddWithValue("@id", id);

            command.Prepare();
            command.ExecuteNonQuery();
        }

    }

    public RamMetric Get(int id)
    {
        using var connection = new SQLiteConnection(connectionString);
        connection.Open();

        using (var command = new SQLiteCommand(connection))
        {
            command.CommandText = "SELECT * FROM rammetrics WHERE id=@id";
            command.Parameters.AddWithValue("@id", id);

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new RamMetric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = reader.GetInt32(2)
                    };
                }
                else
                {
                    return null;
                }
            }
        }
    }

    public IList<RamMetric> GetAll()
    {
        var returnList = new List<RamMetric>();

        using var connection = new SQLiteConnection(connectionString);
        connection.Open();

        using (var command = new SQLiteCommand(connection))
        {
            command.CommandText = "SELECT * FROM rammetrics";

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new RamMetric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = reader.GetInt32(2)
                    });
                }
            }
        }

        return returnList;
    }

    public IList<RamMetric> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
    {
        var returnList = new List<RamMetric>();

        using var connection = new SQLiteConnection(connectionString);
        connection.Open();

        using (var command = new SQLiteCommand(connection))
        {
            command.CommandText = "SELECT * FROM rammetrics WHERE time >= @fromTime and time <= @toTime";
            command.Parameters.AddWithValue("@fromTime", fromTime.TotalSeconds);
            command.Parameters.AddWithValue("@toTime", toTime.TotalSeconds);

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new RamMetric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = reader.GetInt32(2)
                    });
                }
            }
        }

        return returnList;
    }

    public void Update(RamMetric item)
    {
        using var connection = new SQLiteConnection(connectionString);
        connection.Open();

        using (var command = new SQLiteCommand(connection))
        {
            command.CommandText = "UPDATE rammetrics SET value = @value, time =@time WHERE id = @id; ";

            command.Parameters.AddWithValue("@id", item.Id);
            command.Parameters.AddWithValue("@value", item.Value);
            command.Parameters.AddWithValue("@time", item.Time);

            command.Prepare();
            command.ExecuteNonQuery();
        }
    }
}