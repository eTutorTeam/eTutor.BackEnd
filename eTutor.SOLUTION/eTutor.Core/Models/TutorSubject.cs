namespace eTutor.Core.Models
{
    public sealed class TutorSubject : EntityBase, IEntityBase
    {
        public int TutorId { get; set; }

        public User Tutor { get; set; }

        public int SubjectId { get; set; }

        public Subject Subject { get; set; }
    }
}