using System.ComponentModel.DataAnnotations;

namespace eTutor.Core.Models
{
    public class Rating : EntityBase
    {
        public int MeetingId { get; set; }

        public Meeting Meeting { get; set; }

        public int? StudentId { get; set; }

        public Student Student { get; set; }

        public int? TutorId { get; set; }

        public Tutor Tutor { get; set; }
        
        [Range(0,10)]
        public int Calification { get; set; }
    }
}