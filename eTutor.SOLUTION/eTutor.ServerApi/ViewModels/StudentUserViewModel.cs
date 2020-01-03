using System;

namespace eTutor.ServerApi.ViewModels
{
    public sealed class StudentUserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public DateTime BirthDate { get; set; }
        
        public string ProfileImageUrl { get; set; }
        
        public int Age { get; set; }
        
        public decimal Ratings { get; set; }
        
        public string FullName => $"{Name} {LastName}";
        
        public string AboutMe { get; set; }
    }
}