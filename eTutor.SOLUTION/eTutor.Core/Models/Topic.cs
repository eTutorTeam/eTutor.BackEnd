using System.Collections.Generic;

namespace eTutor.Core.Models
{
    public sealed class Topic : EntityBase, IEntityBase
    {
        public string Name { get; set; }

        public string Description { get; set; }
        
        public ISet<TutorTopic> Tutors { get; set; }
    }
}