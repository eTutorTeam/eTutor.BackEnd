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

            CreateMap<Meeting, TutorMeetingSummary>()
                .ForMember(dest => dest.MeetingId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.FullName))
                .ForMember(dest => dest.StudentImg, opt => opt.MapFrom(src => src.Student.ProfileImageUrl))
                .ForMember(dest => dest.MeetingDate, opt => opt.MapFrom(src => src.StartDateTime.Date))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartDateTime ))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndDateTime));
        }
    }
}