using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTutor.Core.Enums;

namespace eTutor.ServerApi.ViewModels
{
    public sealed class UserProfileUpdateRequest
    {
        public string Address { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string Email { get; set; }
        public string AboutMe { get; set; }
        public string PersonalId { get; set; }
        public string UserName { get; set; }

    }
}
