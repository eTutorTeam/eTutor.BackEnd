using AutoMapper;
using eTutor.Core.Models;
using eTutor.ServerApi.ViewModels;

namespace eTutor.ServerApi.MapperProfiles
{
    public class MeetingProfile : Profile
    {
        public MeetingProfile()
        {
            CreateMap<MeetingStudentRequest, Meeting>().ReverseMap();

            CreateMap<Meeting, MeetingResponse>()
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name))
                .ForMember(dest => dest.TutorName, opt => opt.MapFrom(src => src.Tutor.FullName))
                .ForMember(dest => dest.TutorImage, opt => opt.MapFrom(src => src.Tutor.ProfileImageUrl));
        }
    }
}