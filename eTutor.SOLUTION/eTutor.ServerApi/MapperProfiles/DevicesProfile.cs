using AutoMapper;
using eTutor.Core.Models;
using eTutor.ServerApi.ViewModels;

namespace eTutor.ServerApi.MapperProfiles
{
    public class DevicesProfile : Profile
    {
        public DevicesProfile()
        {
            CreateMap<DeviceTokenRequest, Device>()
                .ForMember(dest => dest.Platform, opt => opt.MapFrom(src => string.Join(',', src.Platforms)));
        }
    }
}