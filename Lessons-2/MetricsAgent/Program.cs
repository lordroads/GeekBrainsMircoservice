using AutoMapper;
using FluentMigrator.Runner;
using MetricsAgent.Converters;
using MetricsAgent.Jobs;
using MetricsAgent.Mapper;
using MetricsAgent.Models;
using MetricsAgent.Service;
using MetricsAgent.Service.Implementations;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NLog.Web;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region Configure Jobs

builder.Services.AddHostedService<QuartzHostedService>();

//Регистрируем сервисы
builder.Services.AddSingleton<IJobFactory, SingletonJobFactory>();
builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

// Добавление задач на исполнение
builder.Services.AddSingleton<CpuMetricJob>();
builder.Services.AddSingleton(new JobSchedule(jobType: typeof(CpuMetricJob), cronExpression: "0/5 * * * * ?")); // Запускать каждые 5 секунд

builder.Services.AddSingleton<DotnetMetricJob>();
builder.Services.AddSingleton(new JobSchedule(jobType: typeof(DotnetMetricJob), cronExpression: "0/5 * * * * ?"));

builder.Services.AddSingleton<HddMetricJob>();
builder.Services.AddSingleton(new JobSchedule(jobType: typeof(HddMetricJob), cronExpression: "0/5 * * * * ?"));

builder.Services.AddSingleton<NetworkMetricJob>();
builder.Services.AddSingleton(new JobSchedule(jobType: typeof(NetworkMetricJob), cronExpression: "0/5 * * * * ?"));

builder.Services.AddSingleton<RamMetricJob>();
builder.Services.AddSingleton(new JobSchedule(jobType: typeof(RamMetricJob), cronExpression: "0/5 * * * * ?"));


#endregion

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

builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(rb =>
        rb.AddSQLite()
            .WithGlobalConnectionString(builder.Configuration["Settings:DatabaseOptions:ConnectionString"])
            .ScanIn(typeof(Program).Assembly).For.Migrations())
        .AddLogging(lb => lb.AddFluentMigratorConsole());

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

var serviesScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (IServiceScope serviceScope = serviesScopeFactory.CreateScope())
{
    var migrationRunner = serviceScope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    migrationRunner.MigrateUp();
}

app.Run();
