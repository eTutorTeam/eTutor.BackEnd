﻿using System;
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
            CreateMap<User, TutorSimpleResponse>().ReverseMap();
        }
    }
}