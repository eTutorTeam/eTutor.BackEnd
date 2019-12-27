using AutoMapper;
using eTutor.Core.Models;
using eTutor.ServerApi.ViewModels;

namespace eTutor.ServerApi.MapperProfiles
{
    public sealed class ParentsProfile : Profile
    {

        public ParentsProfile()
        {
            CreateMap<ParentAuthorization, ParentAuthorizationResponse>().ReverseMap();
        }
        
    }
}