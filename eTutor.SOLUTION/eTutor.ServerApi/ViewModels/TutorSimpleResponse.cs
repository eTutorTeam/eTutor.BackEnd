﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTutor.Core.Enums;

namespace eTutor.ServerApi.ViewModels
{
    public sealed class TutorSimpleResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }
        
        public decimal Ratings { get; set; }

        public string Email { get; set; }

        public Gender Gender { get; set; }

        public string FullName => $"{Name} {LastName}";

        public string ProfileImageUrl { get; set; }
		public string PhoneNumber { get; set; }
    }
}
