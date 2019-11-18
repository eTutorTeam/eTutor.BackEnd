using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTutor.ServerApi.ViewModels
{
    public class UserResponse
    {
        public string Name { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string FullName => $"{Name} {LastName}";

    }
}
