using MetricsAgent.Models;
using MetricsAgent.Service;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Jobs;

public class HddMetricJob : IJob
{
    private IServiceScopeFactory _serviceScopeFactory;
    private PerformanceCounter _hddCounter;

    public HddMetricJob(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _hddCounter = new PerformanceCounter("Физический диск", "% активности диска", "_Total");
    }

    public Task Execute(IJobExecutionContext context)
    {
        using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
        {
            var hddMetricsRepository = serviceScope.ServiceProvider.GetRequiredService<IHddMetricsRepository>();

            try
            {
                int hddUsageInPercent = Convert.ToInt32(_hddCounter.NextValue());
                long time = Convert.ToInt64(TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds()).TotalSeconds);
                Debug.WriteLine($"[HddMetric]: {time} > {hddUsageInPercent}");

                hddMetricsRepository.Create(new HddMetric
                {
                    Value = hddUsageInPercent,
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
