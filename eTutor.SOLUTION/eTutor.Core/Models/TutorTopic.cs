namespace eTutor.Core.Models
{
    public sealed class TutorTopic : EntityBase, IEntityBase
    {
        public int TutorId { get; set; }

        public User Tutor { get; set; }

        public int TopicId { get; set; }

        public Topic Topic { get; set; }
    }
}