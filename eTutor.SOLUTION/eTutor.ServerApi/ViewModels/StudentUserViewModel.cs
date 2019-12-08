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

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string FullName => $"{Name} {LastName}";
    }
}