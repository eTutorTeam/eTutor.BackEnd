﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTutor.Core.Enums;

namespace eTutor.ServerApi.ViewModels
{
    public sealed class StudentUserRegistrationRequest
    {
        public string Name { get; set; }

        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string ParentEmail { get; set; }

        public DateTime BirthDate { get; set; }
    }
}