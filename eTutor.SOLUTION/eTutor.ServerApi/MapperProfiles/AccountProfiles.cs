using AutoMapper;
using eTutor.Core.Models;
using eTutor.ServerApi.ViewModels;

namespace eTutor.ServerApi.MapperProfiles
{
    public class AccountProfiles : Profile

    {
        public AccountProfiles()
        {
            CreateMap<TutorUserRegistrationRequest, User>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName ?? src.Email))
                .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => (src.UserName ?? src.Email).ToUpper()))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.PersonalId, opt => opt.MapFrom(src => src.PersonalId ?? ""));

            CreateMap<StudentUserRegistrationRequest, User>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName ?? src.Email))
                .ForMember(dest => dest.NormalizedUserName,
                    opt => opt.MapFrom(src => (src.UserName ?? src.Email).ToUpper()))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate));

            CreateMap<ParentUserRegistrationRequest, User>();

			CreateMap<User, UserResponse>()
				.ForMember(dest => dest.ProfileImageUrl, opt => opt.MapFrom(
					src => src.ProfileImageUrl ?? "https://immedilet-invest.com/wp-content/uploads/2016/01/user-placeholder.jpg"))
				.ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ReverseMap();

            CreateMap<ChangePassword, ChangePasswordResponse>();
        }
    }
}