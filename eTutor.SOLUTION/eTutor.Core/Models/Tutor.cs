using System.Collections.Generic;

namespace eTutor.Core.Models
{
    public sealed class Tutor : EntityBase
    {
        public int UserId { get; set; }
        
        public User User { get; set; }
        
        public string Address { get; set; }
        
        public string Name { get; set; }
        
        public string LastName { get; set; }

        public ISet<TutorTopic> Topics { get; set; }

        public ISet<Rating> Ratings { get; set; }
    }
}