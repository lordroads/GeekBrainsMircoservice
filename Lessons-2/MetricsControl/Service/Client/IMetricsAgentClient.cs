using MetricsControl.Models.Requests;
using MetricsControl.Models.Response;

namespace MetricsControl.Service.Client;

public interface IMetricsAgentClient
{
    AllRamMetricsApiResponse GetRamMetrics(GetAllRamMetricsApiRequest request);
    AllHddMetricsApiResponse GetHddMetrics(GetAllHddMetricsApiRequest request);
    AllDotnetMetricsApiResponse GetDotnetMetrics(GetAllDotnetMetricsApiRequest request);
    AllCpuMetricsApiResponse GetCpuMetrics(GetAllCpuMetricsApiRequest request);
    AllNetworkMetricsApiResponse GetNetworkMetrics(GetAllNetworkMetricsApiRequest request);

}
