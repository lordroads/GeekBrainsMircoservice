using MetricsAgent.Models;
using MetricsAgent.Service;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Jobs;

public class NetworkMetricJob : IJob
{
    private string _categoryName = "Сетевой интерфейс";
    private string _counterName = "Всего байт/с";
    private IServiceScopeFactory _serviceScopeFactory;
    private PerformanceCounter _networkCounter;

    public NetworkMetricJob(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        string nameNetworkInterface = PerformanceCounterCategory.GetCategories()
            .FirstOrDefault(cat => cat.CategoryName == _categoryName)
            .GetInstanceNames()
            .FirstOrDefault();

        _networkCounter = new PerformanceCounter(_categoryName, _counterName, nameNetworkInterface);
    }

    public Task Execute(IJobExecutionContext context)
    {
        using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
        {
            var networkMetricsRepository = serviceScope.ServiceProvider.GetRequiredService<INetworkMetricsRepository>();

            try
            {
                int networkUsageInByte = Convert.ToInt32(_networkCounter.NextValue());
                long time = Convert.ToInt64(TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds()).TotalSeconds);
                Debug.WriteLine($"[NetworkMetric]: {time} > {networkUsageInByte}");

                networkMetricsRepository.Create(new NetworkMetric
                {
                    Value = networkUsageInByte,
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
