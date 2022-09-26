using MetricsAgent.Models;
using MetricsAgent.Service;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Jobs;

public class RamMetricJob : IJob
{
    private IServiceScopeFactory _serviceScopeFactory;
    private PerformanceCounter _ramCounter;

    public RamMetricJob(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _ramCounter = new PerformanceCounter("Память", "% использования выделенной памяти");
    }

    public Task Execute(IJobExecutionContext context)
    {
        using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
        {
            var ramMetricsRepository = serviceScope.ServiceProvider.GetRequiredService<IRamMetricsRepository>();

            try
            {
                int ramUsageInPercent = Convert.ToInt32(_ramCounter.NextValue());
                long time = Convert.ToInt64(TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds()).TotalSeconds);
                Debug.WriteLine($"[RamMetric]: {time} > {ramUsageInPercent}");

                ramMetricsRepository.Create(new RamMetric
                {
                    Value = ramUsageInPercent,
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
