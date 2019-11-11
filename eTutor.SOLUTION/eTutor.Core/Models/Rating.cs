using System.ComponentModel.DataAnnotations;

namespace eTutor.Core.Models
{
    public class Rating : EntityBase
    {
        public int MeetingId { get; set; }

        public Meeting Meeting { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
        
        [Range(0,10)]
        public int Calification { get; set; }
    }
}