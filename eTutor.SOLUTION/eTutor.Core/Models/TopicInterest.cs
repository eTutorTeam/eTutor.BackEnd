namespace eTutor.Core.Models
{
    public class TopicInterest : EntityBase
    {
        public int TopicId { get; set; }

        public Topic Topic { get; set; }

        public int StudentId { get; set; }

        public Student Student { get; set; }
    }
}