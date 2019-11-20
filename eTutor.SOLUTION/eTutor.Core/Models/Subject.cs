using System.Collections.Generic;

namespace eTutor.Core.Models
{
    public sealed class Subject : EntityBase, IEntityBase
    {
        public string Name { get; set; }

        public string Description { get; set; }
        
        public ISet<TutorSubject> Tutors { get; set; }
    }
}