using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using eTutor.Core.Models;
using eTutor.ServerApi.ViewModels;

namespace eTutor.ServerApi.MapperProfiles
{
    public class SubjectsProfile : Profile
    {
        public SubjectsProfile()
        {
            CreateMap<Subject, SubjectResponse>()
                .ForMember(dest => dest.TutorsCount, 
                    opt => opt.MapFrom(src => src.Tutors != null ? src.Tutors.Count : 0));

            CreateMap<Subject, SubjectUnrestrictedResponse>();

            CreateMap<Subject, SubjectResponseTutorDetail>();

            CreateMap<SubjectCreateRequest, Subject>();
        }
    }
}
