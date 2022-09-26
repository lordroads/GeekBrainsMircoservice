using MetricsAgent.Models;
using MetricsAgent.Service;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Jobs;

public class CpuMetricJob : IJob
{
    private IServiceScopeFactory _serviceScopeFactory;
    private PerformanceCounter _cpuCounter;

    public CpuMetricJob(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
    }

    public Task Execute(IJobExecutionContext context)
    {
        using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
        {
            var cpuMetricsRepository = serviceScope.ServiceProvider.GetRequiredService<ICpuMetricsRepository>();

            try
            {
                int cpuUsageInPercent = Convert.ToInt32(_cpuCounter.NextValue());
                long time = Convert.ToInt64(TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds()).TotalSeconds);
                Debug.WriteLine($"[CpuMetric]: {time} > {cpuUsageInPercent}");

                cpuMetricsRepository.Create(new CpuMetric
                {
                    Value = cpuUsageInPercent,
                    Time = time
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR]: {ex.Message}");
            }
        }

        return Task.CompletedTask;
    }
}
