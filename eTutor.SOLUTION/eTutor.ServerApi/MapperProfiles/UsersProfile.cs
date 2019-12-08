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
            CreateMap<UserProfileUpdateRequest, User>();

            CreateMap<User, SimpleUserResponse>();

            CreateMap<User, UserAdminResponse>();

            CreateMap<User, StudentUserViewModel>();
        }
    }
}
