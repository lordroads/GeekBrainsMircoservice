using AutoMapper;
using MetricsAgent.Converters;
using MetricsAgent.Mapper;
using MetricsAgent.Models;
using MetricsAgent.Service;
using MetricsAgent.Service.Implementations;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NLog.Web;
using System.Data.SQLite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region Configure Automapper

var mapperConfiguration = new MapperConfiguration(mapper => mapper.AddProfile(new MapperProfile()));
var mapper = mapperConfiguration.CreateMapper();
builder.Services.AddSingleton(mapper);

#endregion

#region Configure Options

builder.Services.Configure<DatabaseOptions>(options =>
{
    builder.Configuration.GetSection("Settings:DatabaseOptions").Bind(options);
});

#endregion

#region Configure DataBase

ConfigureSqlLiteConnection(builder.Services);

#endregion

#region Configure Repository

builder.Services.AddScoped<ICpuMetricsRepository, CpuMetricsRepository>();
builder.Services.AddScoped<IDotnetMetricsRepository, DotnetMetricsRepository>();
builder.Services.AddScoped<IHddMetricsRepository, HddMetricsRepository>();
builder.Services.AddScoped<INetworkMetricsRepository, NetworkMetricsRepository>();
builder.Services.AddScoped<IRamMetricsRepository, RamMetricsRepository>();

#endregion

#region Configure Logging

builder.Host.ConfigureLogging(logger =>
{
    logger.ClearProviders();
    logger.AddConsole();
}).UseNLog(new NLogAspNetCoreOptions { RemoveLoggerFactoryFilter = true });

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All | HttpLoggingFields.RequestQuery;
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
    logging.ResponseHeaders.Add("Authorization");
    logging.ResponseHeaders.Add("X-Real-IP");
    logging.ResponseHeaders.Add("X-Forwarder-For");
});

#endregion

builder.Services.AddControllers().
    AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new CustomTimeSpanConverter()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new OpenApiInfo { Title = "MetricsAgent" , Version = "v1" });

    setup.MapType<TimeSpan>(() => new OpenApiSchema {
        Type = "string",
        Example = new OpenApiString("00:00:00")
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseHttpLogging();

app.MapControllers();

app.Run();




void ConfigureSqlLiteConnection(IServiceCollection services)
{
    const string connectionString = "Data Source = metrics.db; Version = 3; Pooling = true; Max Pool Size = 100; ";
    var connection = new SQLiteConnection(connectionString);
    connection.Open();
    PrepareSchema(connection);
    //PrepareDataInSchema(connection);
}

void PrepareSchema(SQLiteConnection connection)
{
    using (var command = new SQLiteCommand(connection))
    {
        //CPU
        command.CommandText = @"CREATE TABLE IF NOT EXISTS cpumetrics(id INTEGER PRIMARY KEY, value INT, time INT)";
        command.ExecuteNonQuery();

        //DotNet
        command.CommandText = @"CREATE TABLE IF NOT EXISTS dotnetmetrics(id INTEGER PRIMARY KEY, value INT, time INT)";
        command.ExecuteNonQuery();

        //HDD
        command.CommandText = @"CREATE TABLE IF NOT EXISTS hddmetrics(id INTEGER PRIMARY KEY, value INT, time INT)";
        command.ExecuteNonQuery();

        //Network
        command.CommandText = @"CREATE TABLE IF NOT EXISTS networkmetrics(id INTEGER PRIMARY KEY, value INT, time INT)";
        command.ExecuteNonQuery();

        //Ram
        command.CommandText = @"CREATE TABLE IF NOT EXISTS rammetrics(id INTEGER PRIMARY KEY, value INT, time INT)";
        command.ExecuteNonQuery();
    }
}

//Generate fake data.
void PrepareDataInSchema(SQLiteConnection connection)
{
    using (var command = new SQLiteCommand(connection))
    {
        //CPU
        command.CommandText = @"INSERT INTO `cpumetrics` (`value`,`time`)
                                VALUES
                                  (76,1000),
                                  (8,2000),
                                  (1,3000),
                                  (15,4000),
                                  (12,5000),
                                  (29,6000),
                                  (53,7000),
                                  (69,8000),
                                  (95,9000),
                                  (97,10000);";
        command.ExecuteNonQuery();

        //DotNet
        command.CommandText = @"INSERT INTO `dotnetmetrics` (`value`,`time`)
                                VALUES
                                  (2,1000),
                                  (1,2000),
                                  (1,3000),
                                  (5,4000),
                                  (4,5000),
                                  (1,6000),
                                  (5,7000),
                                  (4,8000),
                                  (0,9000),
                                  (5,10000);";
        command.ExecuteNonQuery();

        //HDD
        command.CommandText = @"INSERT INTO `hddmetrics` (`value`,`time`)
                                VALUES
                                  (8503527,1000),
                                  (9841286,2000),
                                  (3566840,3000),
                                  (5439502,4000),
                                  (8842306,5000),
                                  (9038410,6000),
                                  (4953190,7000),
                                  (8896222,8000),
                                  (3240962,9000),
                                  (7748146,10000);";
        command.ExecuteNonQuery();

        //Network
        command.CommandText = @"INSERT INTO `networkmetrics` (`value`,`time`)
                                VALUES
                                  (32,1000),
                                  (37,2000),
                                  (31,3000),
                                  (92,4000),
                                  (61,5000),
                                  (37,6000),
                                  (3,7000),
                                  (1,8000),
                                  (55,9000),
                                  (25,10000);";

        command.ExecuteNonQuery();

        //Ram
        command.CommandText = @"INSERT INTO `rammetrics` (`value`,`time`)
                                VALUES
                                  (9743,1000),
                                  (6119,2000),
                                  (1580,3000),
                                  (3485,4000),
                                  (4157,5000),
                                  (6279,6000),
                                  (4463,7000),
                                  (3442,8000),
                                  (9024,9000),
                                  (7342,10000);";
        command.ExecuteNonQuery();
    }
}
