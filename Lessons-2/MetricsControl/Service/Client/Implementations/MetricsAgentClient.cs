using MetricsControl.Models.Dto;
using MetricsControl.Models.Requests;
using MetricsControl.Models.Response;
using MetricsControl.Service.Client;
using Newtonsoft.Json;

namespace MetricsControl.Service.Client.Implementations;

public class MetricsAgentClient : IMetricsAgentClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MetricsAgentClient> _logger;

    public MetricsAgentClient(HttpClient httpClient, ILogger<MetricsAgentClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public AllHddMetricsApiResponse GetHddMetrics(GetAllHddMetricsApiRequest request)
    {
        Uri agentAddress = request.AgentAddress;
        long fromTime = Convert.ToInt64(request.FromTime.TotalSeconds);
        long toTime = Convert.ToInt64(request.ToTime.TotalSeconds);

        var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{agentAddress}api/metrics/hdd/left/from/{fromTime}/to/{toTime}");

        try
        {
            HttpResponseMessage httpResponse = _httpClient.SendAsync(httpRequest).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                var response = httpResponse.Content.ReadAsStringAsync().Result;

                return (AllHddMetricsApiResponse)JsonConvert.DeserializeObject(response, typeof(AllHddMetricsApiResponse));
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }

        return null;
    }

    public AllRamMetricsApiResponse GetRamMetrics(GetAllRamMetricsApiRequest request)
    {
        Uri agentAddress = request.AgentAddress;
        long fromTime = Convert.ToInt64(request.FromTime.TotalSeconds);
        long toTime = Convert.ToInt64(request.ToTime.TotalSeconds);

        var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{agentAddress}api/metrics/ram/available/from/{fromTime}/to/{toTime}");

        try
        {
            HttpResponseMessage httpResponse = _httpClient.SendAsync(httpRequest).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                var response = httpResponse.Content.ReadAsStringAsync().Result;

                return (AllRamMetricsApiResponse)JsonConvert.DeserializeObject(response, typeof(AllRamMetricsApiResponse));
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }

        return null;
    }

    public AllCpuMetricsApiResponse GetCpuMetrics(GetAllCpuMetricsApiRequest request)
    {
        Uri agentAddress = request.AgentAddress;
        long fromTime = Convert.ToInt64(request.FromTime.TotalSeconds);
        long toTime = Convert.ToInt64(request.ToTime.TotalSeconds);

        var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{agentAddress}api/metrics/cpu/from/{fromTime}/to/{toTime}");

        try
        {
            HttpResponseMessage httpResponse = _httpClient.SendAsync(httpRequest).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                var response = httpResponse.Content.ReadAsStringAsync().Result;

                return (AllCpuMetricsApiResponse)JsonConvert.DeserializeObject(response, typeof(AllCpuMetricsApiResponse));
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }

        return null;
    }

    public AllDotnetMetricsApiResponse GetDotnetMetrics(GetAllDotnetMetricsApiRequest request)
    {
        Uri agentAddress = request.AgentAddress;
        long fromTime = Convert.ToInt64(request.FromTime.TotalSeconds);
        long toTime = Convert.ToInt64(request.ToTime.TotalSeconds);

        var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{agentAddress}api/metrics/dotnet/errors-count/from/{fromTime}/to/{toTime}");

        try
        {
            HttpResponseMessage httpResponse = _httpClient.SendAsync(httpRequest).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                var response = httpResponse.Content.ReadAsStringAsync().Result;

                return (AllDotnetMetricsApiResponse)JsonConvert.DeserializeObject(response, typeof(AllDotnetMetricsApiResponse));
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }

        return null;
    }

    public AllNetworkMetricsApiResponse GetNetworkMetrics(GetAllNetworkMetricsApiRequest request)
    {
        Uri agentAddress = request.AgentAddress;
        long fromTime = Convert.ToInt64(request.FromTime.TotalSeconds);
        long toTime = Convert.ToInt64(request.ToTime.TotalSeconds);

        var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{agentAddress}api/metrics/network/from/{fromTime}/to/{toTime}");

        try
        {
            HttpResponseMessage httpResponse = _httpClient.SendAsync(httpRequest).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                var response = httpResponse.Content.ReadAsStringAsync().Result;

                return (AllNetworkMetricsApiResponse)JsonConvert.DeserializeObject(response, typeof(AllNetworkMetricsApiResponse));
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }

        return null;
    }
}
