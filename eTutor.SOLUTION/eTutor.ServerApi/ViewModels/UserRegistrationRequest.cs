using System.Collections.Generic;
using eTutor.Core.Enums;

namespace eTutor.ServerApi.ViewModels
{
    public sealed class UserRegistrationRequest
    {
        public string Name { get; set; }
        
        public string LastName { get; set; }
        
        public Gender Gender { get; set; } 
        
        public string Password { get; set; }
        
        public ISet<RoleTypes> Roles { get; set; }
        
        public string Email { get; set; }
        
        public string UserName { get; set; }
        
        public string PersonalId { get; set; }
    }
}