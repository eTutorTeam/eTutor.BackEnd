namespace eTutor.Core.Models
{
    public sealed class RejectedMeeting : EntityBase
    {
        public int MeetingId { get; set; }
        
        public Meeting Meeting { get; set; }
        
        public int TutorId { get; set; }
        
        public User Tutor { get; set; }
    }
}