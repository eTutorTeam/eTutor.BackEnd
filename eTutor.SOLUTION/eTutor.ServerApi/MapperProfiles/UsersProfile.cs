using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using eTutor.Core.Models;
using eTutor.ServerApi.ViewModels;

namespace eTutor.ServerApi.MapperProfiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<UserProfileUpdateRequest, User>()
				.ForMember(dest=>dest.PhoneNumber, opt=>opt.MapFrom(src => src.PhoneNumber));

            CreateMap<User, SimpleUserResponse>();

            CreateMap<User, UserAdminResponse>();

            CreateMap<User, StudentUserViewModel>()
                .ForMember(dest => dest.ProfileImageUrl, opt => 
                    opt.MapFrom(src => src.ProfileImageUrl
                                       ?? "https://immedilet-invest.com/wp-content/uploads/2016/01/user-placeholder.jpg"))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => DateTime.Now.Year - src.BirthDate.Year));
        }
    }
}
