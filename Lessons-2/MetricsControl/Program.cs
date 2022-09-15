using FluentMigrator.Runner;
using MetricsControl.Converters;
using MetricsControl.Models;
using MetricsControl.Service;
using MetricsControl.Service.Client;
using MetricsControl.Service.Client.Implementations;
using MetricsControl.Service.Implementations;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NLog.Web;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region Configure Service Singleton

//Переделано на БД SQLite
//builder.Services.AddSingleton<AgentPool>();

#endregion

#region Configure Client with Polly

builder.Services.AddHttpClient<IMetricsAgentClient, MetricsAgentClient>()
                .AddTransientHttpErrorPolicy(p =>
                p.WaitAndRetryAsync(
                    retryCount: 3, 
                    sleepDurationProvider: (attemptCount) => TimeSpan.FromSeconds(attemptCount * 2), 
                    onRetry: (response, sleepDuration, attemptCount, context) => {
                        var logger = builder.Services.BuildServiceProvider().GetService<ILogger<Program>>();
                        logger.LogError(response.Exception != null ? response.Exception : 
                            new Exception($"\n{response.Result.StatusCode}: {response.Result.RequestMessage}"), $"(attempt: {attemptCount}) request exception.");
                    }));

#endregion

#region Configure Options

builder.Services.Configure<DatabaseOptions>(options =>
{
    builder.Configuration.GetSection("Settings:DatabaseOptions").Bind(options);
});

#endregion

#region Configure Repository

builder.Services.AddScoped<IAgentRepository, AgentsRepository>();

#endregion

#region Configure DataBase

builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(rb =>
        rb.AddSQLite()
            .WithGlobalConnectionString(builder.Configuration["Settings:DatabaseOptions:ConnectionString"])
            .ScanIn(typeof(Program).Assembly).For.Migrations())
        .AddLogging(lb => lb.AddFluentMigratorConsole());

#endregion

#region Configure Automapper

//var mapperConfiguration = new MapperConfiguration(mapper => mapper.AddProfile(new MapperProfile()));
//var mapper = mapperConfiguration.CreateMapper();
//builder.Services.AddSingleton(mapper);

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
    setup.SwaggerDoc("v1", new OpenApiInfo { Title = "MetricsControl", Version = "v1" });

    setup.MapType<TimeSpan>(() => new OpenApiSchema
    {
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

var serviesScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (IServiceScope serviceScope = serviesScopeFactory.CreateScope())
{
    var migrationRunner = serviceScope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    migrationRunner.MigrateUp();
}

app.MapControllers();

app.Run();
