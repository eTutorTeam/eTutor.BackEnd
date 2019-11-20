namespace eTutor.Core.Models
{
    public class TopicInterest : EntityBase
    {
        public int TopicId { get; set; }

        public Subject Subject { get; set; }

        public int StudentId { get; set; }

        public User Student { get; set; }
    }
}