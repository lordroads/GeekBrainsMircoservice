
//Metrics Control - http://localhost:5004

using MetricsControl.Client;

bool isRun = true;
CpuMetricsClient cpuMetricsClient = new CpuMetricsClient("http://localhost:5004", new HttpClient());
DotnetMetricsClient dotnetMetricsClient = new DotnetMetricsClient("http://localhost:5004", new HttpClient());
HddMetricsClient hddMetricsClient = new HddMetricsClient("http://localhost:5004", new HttpClient());
NetworkMetricsClient networkMetricsClient = new NetworkMetricsClient("http://localhost:5004", new HttpClient());
RamMetricsClient ramMetricsClient = new RamMetricsClient("http://localhost:5004", new HttpClient());

while (isRun)
{
    Console.Clear();
    Console.WriteLine("Запросы к Апи:");
    Console.WriteLine("==============================================");
    Console.WriteLine("1 - Получить метрики за последнюю минуту (CPU)");
    Console.WriteLine("2 - Получить метрики за последнюю минуту (Dotnet)");
    Console.WriteLine("3 - Получить метрики за последнюю минуту (HDD)");
    Console.WriteLine("4 - Получить метрики за последнюю минуту (Network)");
    Console.WriteLine("5 - Получить метрики за последнюю минуту (RAM)");
    Console.WriteLine("0 - Выход из приложения");
    Console.WriteLine("==============================================");

    Console.WriteLine("Введите номер: ");
    if (int.TryParse(Console.ReadLine(), out int userEnterNumber))
    {
        switch (userEnterNumber)
        {
            case 0:
                Console.WriteLine("Good Bey!");
                Console.ReadKey(true);
                isRun = false;
                break;
            case 1:
                await GetCpuMetricsFromOneMin();
                break;
            case 2:
                await GetDotnetMetricsFromOneMin();
                break;
            case 3:
                await GetHddMetricsFromOneMin();
                break;
            case 4:
                await GetNetworkMetricsFromOneMin();
                break;
            case 5:
                await GetRamMetricsFromOneMin();
                break;
            default:
                Console.WriteLine("Не корректно введен номер.");
                break;
        }
    }
}

async Task GetCpuMetricsFromOneMin()
{
    try
    {
        TimeSpan toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        TimeSpan fromTime = toTime - TimeSpan.FromSeconds(60);

        AllCpuMetricsApiResponse cpuMetrics = await cpuMetricsClient.GetAllByIdAsync(
            1,
            fromTime.ToString("dd\\.hh\\:mm\\:ss"),
            toTime.ToString("dd\\.hh\\:mm\\:ss"));
        if (cpuMetrics.Metrics.Count == 0)
        {
            Console.WriteLine("Metrics - None!");
        }

        foreach (CpuMetricDto cpuMetric in cpuMetrics.Metrics)
        {
            Console.WriteLine($"{TimeSpan.FromSeconds(cpuMetric.Time).ToString("dd\\.hh\\:mm\\:ss")} >>> {cpuMetric.Value} % ");
        }
        Console.ReadKey(true);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERROR] - {ex.Message}");
        Console.ReadKey(true);
    }
}
async Task GetDotnetMetricsFromOneMin()
{
    try
    {
        TimeSpan toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        TimeSpan fromTime = toTime - TimeSpan.FromSeconds(60);

        AllDotnetMetricsApiResponse dotnetMetrics = await dotnetMetricsClient.GetAllByIdAsync(
            1,
            fromTime.ToString("dd\\.hh\\:mm\\:ss"),
            toTime.ToString("dd\\.hh\\:mm\\:ss"));
        if (dotnetMetrics.Metrics.Count == 0)
        {
            Console.WriteLine("Metrics - None!");
        }

        foreach (DotnetMetricDto dotnetMetric in dotnetMetrics.Metrics)
        {
            Console.WriteLine($"{TimeSpan.FromSeconds(dotnetMetric.Time).ToString("dd\\.hh\\:mm\\:ss")} >>> {dotnetMetric.Value} inc ");
        }
        Console.ReadKey(true);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERROR] - {ex.Message}");
        Console.ReadKey(true);
    }
}
async Task GetHddMetricsFromOneMin()
{
    try
    {
        TimeSpan toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        TimeSpan fromTime = toTime - TimeSpan.FromSeconds(60);

        AllHddMetricsApiResponse hddMetrics = await hddMetricsClient.GetAllByIdAsync(
            1,
            fromTime.ToString("dd\\.hh\\:mm\\:ss"),
            toTime.ToString("dd\\.hh\\:mm\\:ss"));
        if (hddMetrics.Metrics.Count == 0)
        {
            Console.WriteLine("Metrics - None!");
        }

        foreach (HddMetricDto hddMetric in hddMetrics.Metrics)
        {
            Console.WriteLine($"{TimeSpan.FromSeconds(hddMetric.Time).ToString("dd\\.hh\\:mm\\:ss")} >>> {hddMetric.Value} % ");
        }
        Console.ReadKey(true);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERROR] - {ex.Message}");
        Console.ReadKey(true);
    }
}
async Task GetNetworkMetricsFromOneMin()
{
    try
    {
        TimeSpan toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        TimeSpan fromTime = toTime - TimeSpan.FromSeconds(60);

        AllNetworkMetricsApiResponse networkMetrics = await networkMetricsClient.GetAllByIdAsync(
            1,
            fromTime.ToString("dd\\.hh\\:mm\\:ss"),
            toTime.ToString("dd\\.hh\\:mm\\:ss"));
        if (networkMetrics.Metrics.Count == 0)
        {
            Console.WriteLine("Metrics - None!");
        }

        foreach (NetworkMetricDto networkMetric in networkMetrics.Metrics)
        {
            Console.WriteLine($"{TimeSpan.FromSeconds(networkMetric.Time).ToString("dd\\.hh\\:mm\\:ss")} >>> {networkMetric.Value} Mb ");
        }
        Console.ReadKey(true);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERROR] - {ex.Message}");
        Console.ReadKey(true);
    }
}
async Task GetRamMetricsFromOneMin()
{
    try
    {
        TimeSpan toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        TimeSpan fromTime = toTime - TimeSpan.FromSeconds(60);

        AllRamMetricsApiResponse ramMetrics = await ramMetricsClient.GetAllByIdAsync(
            1,
            fromTime.ToString("dd\\.hh\\:mm\\:ss"),
            toTime.ToString("dd\\.hh\\:mm\\:ss"));
        if (ramMetrics.Metrics.Count == 0)
        {
            Console.WriteLine("Metrics - None!");
        }

        foreach (RamMetricDto ramMetric in ramMetrics.Metrics)
        {
            Console.WriteLine($"{TimeSpan.FromSeconds(ramMetric.Time).ToString("dd\\.hh\\:mm\\:ss")} >>> {ramMetric.Value} % ");
        }
        Console.ReadKey(true);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERROR] - {ex.Message}");
        Console.ReadKey(true);
    }
}