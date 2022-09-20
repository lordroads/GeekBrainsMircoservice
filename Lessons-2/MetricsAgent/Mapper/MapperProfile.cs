using AutoMapper;
using MetricsAgent.Models;
using MetricsAgent.Models.Dto;
using MetricsAgent.Models.Requests;
using MetricsAgent.Models.Response;

namespace MetricsAgent.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<CpuMetric, CpuMetricDto>();
        CreateMap<List<CpuMetricDto>, AllCpuMetricsApiResponse>()
            .ForMember(x => x.Metrics, opt => opt.MapFrom(src => src));
        CreateMap<CpuMetricCreateRequest, CpuMetric>()
            .ForMember(x => x.Value, opt => opt.MapFrom(src => src.Value))
            .ForMember(x => x.Time, opt => opt.MapFrom(src => (long)src.Time.TotalSeconds));

        CreateMap<DotnetMetric, DotnetMetricDto>();
        CreateMap<List<DotnetMetricDto>, AllDotnetMetricsApiResponse>()
            .ForMember(x => x.Metrics, opt => opt.MapFrom(src => src));
        CreateMap<DotnetMetricCreateRequest, DotnetMetric>()
            .ForMember(x => x.Value, opt => opt.MapFrom(src => src.Value))
            .ForMember(x => x.Time, opt => opt.MapFrom(src => (long)src.Time.TotalSeconds));

        CreateMap<HddMetric, HddMetricDto>();
        CreateMap<List<HddMetricDto>, AllHddMetricsApiResponse>()
            .ForMember(x => x.Metrics, opt => opt.MapFrom(src => src));
        CreateMap<HddMetricCreateRequest, HddMetric>()
            .ForMember(x => x.Value, opt => opt.MapFrom(src => src.Value))
            .ForMember(x => x.Time, opt => opt.MapFrom(src => (long)src.Time.TotalSeconds));

        CreateMap<NetworkMetric, NetworkMetricDto>();
        CreateMap<List<NetworkMetricDto>, AllNetworkMetricsApiResponse>()
            .ForMember(x => x.Metrics, opt => opt.MapFrom(src => src));
        CreateMap<NetworkMetricCreateRequest, NetworkMetric>()
            .ForMember(x => x.Value, opt => opt.MapFrom(src => src.Value))
            .ForMember(x => x.Time, opt => opt.MapFrom(src => (long)src.Time.TotalSeconds));

        CreateMap<RamMetric, RamMetricDto>();
        CreateMap<List<RamMetricDto>, AllRamMetricsApiResponse>()
            .ForMember(x => x.Metrics, opt => opt.MapFrom(src => src));
        CreateMap<RamMetricCreateRequest, RamMetric>()
            .ForMember(x => x.Value, opt => opt.MapFrom(src => src.Value))
            .ForMember(x => x.Time, opt => opt.MapFrom(src => (long)src.Time.TotalSeconds));

    }
}
