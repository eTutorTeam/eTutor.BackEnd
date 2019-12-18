using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using eTutor.Core.Models;
using eTutor.ServerApi.ViewModels;

namespace eTutor.ServerApi.MapperProfiles
{
    public class TutorsProfile : Profile 
    {
        public TutorsProfile()
        {
            CreateMap<User, TutorSimpleResponse>()
                .ForMember(dest => dest.ProfileImageUrl, opt => opt.MapFrom(src => src.ProfileImageUrl 
                                                                                   ?? "https://immedilet-invest.com/wp-content/uploads/2016/01/user-placeholder.jpg"))
                .ReverseMap();
        }
    }
}
