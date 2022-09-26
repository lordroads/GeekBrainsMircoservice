using MetricsAgent.Models;
using MetricsAgent.Service;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Jobs;

public class DotnetMetricJob : IJob
{
    private IServiceScopeFactory _serviceScopeFactory;
    private PerformanceCounter _dotnetCounter;

    public DotnetMetricJob(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _dotnetCounter = new PerformanceCounter("Журнал событий", "Событий/с");
    }

    public Task Execute(IJobExecutionContext context)
    {
        using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
        {
            var dotnetMetricsRepository = serviceScope.ServiceProvider.GetRequiredService<IDotnetMetricsRepository>();

            try
            {
                int dotnetUsageInEventsSecond = Convert.ToInt32(_dotnetCounter.NextValue());
                long time = Convert.ToInt64(TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds()).TotalSeconds);
                Debug.WriteLine($"[DotnetMetric]: {time} > {dotnetUsageInEventsSecond}");

                dotnetMetricsRepository.Create(new DotnetMetric
                {
                    Value = dotnetUsageInEventsSecond,
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
